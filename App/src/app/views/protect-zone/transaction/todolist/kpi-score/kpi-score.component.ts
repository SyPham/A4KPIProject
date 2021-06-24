import { UtilitiesService } from './../../../../../_core/_service/utilities.service';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { Objective } from 'src/app/_core/_model/objective';
import { ToDoList, ToDoListL1L2, ToDoListOfQuarter } from 'src/app/_core/_model/todolistv2';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { Todolistv2Service } from 'src/app/_core/_service/todolistv2.service';

import { QueryCellInfoEventArgs } from '@syncfusion/ej2-angular-grids';
import { EmitType } from '@syncfusion/ej2-base';
import { KPIScoreService } from 'src/app/_core/_service/kpi-score.service';
import { KPIScore } from 'src/app/_core/_model/kpi-score';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { KPIService } from 'src/app/_core/_service/kpi.service';
import { KPI } from 'src/app/_core/_model/kpi';
import { Comment } from 'src/app/_core/_model/commentv2';
import { Commentv2Service } from 'src/app/_core/_service/commentv2.service';
import { forkJoin } from 'rxjs';
import { PeriodType } from 'src/app/_core/enum/system';
@Component({
  selector: 'app-kpi-score',
  templateUrl: './kpi-score.component.html',
  styleUrls: ['./kpi-score.component.scss']
})
export class KpiScoreComponent implements OnInit {
  @ViewChild('grid') grid: GridComponent;
  @Input() data: ToDoListL1L2;
  @Input() periodTypeCode: PeriodType;
  gridData: object;
  toolbarOptions = ['Add', 'Delete', 'Search'];
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  model: ToDoList;
  kpiScoreModel: KPIScore;
  kpiScoreData: KPIScore;
  point: number;
  kpiData: KPI[];
  fields: object = { text: 'point', value: 'point' };
  filterSettings = { type: 'Excel' };
  content = '';
  commentModel: Comment;
  constructor(
    public activeModal: NgbActiveModal,
    public service: Todolistv2Service,
    public kpiScoreService: KPIScoreService,
    public kpiService: KPIService,
    public commentService: Commentv2Service,
    private alertify: AlertifyService,
    private utilitiesService: UtilitiesService
  ) { }

  ngOnInit(): void {
    this.kpiScoreModel = {
      id: 0,
      periodTypeId: 0,
      periodTypeCode: this.periodTypeCode,
      period: this.utilitiesService.getQuarter(new Date()),
      point: this.point,
      scoreBy: +JSON.parse(localStorage.getItem('user')).id,
      modifiedTime: null,
      createdTime: new Date().toDateString(),
      accountId: +JSON.parse(localStorage.getItem('user')).id
    }
    this.commentModel = {
      id: 0,
      content: this.content,
      createdBy: +JSON.parse(localStorage.getItem('user')).id,
      objectiveId: this.data.id,
      modifiedBy: null,
      createdTime: new Date().toDateString(),
      modifiedTime: null
    }

    this.loadData();
    this.loadKPIScoreData();
    this.loadKPIData();
    this.getFisrtByAccountId();
    this.getFisrtCommentByObjectiveId();
  }
  getMonthListInCurrentQuarter() {
    const currentQuarter = this.utilitiesService.getQuarter(new Date());
    const listMonthOfEachQuarter = [
        ["Result of Feb.","Result of Mar.","Result of Apr."],
        ["Result of May.","Result of Jun.","Result of Jul."],
        ["Result of Aug.","Result of Sep.","Result of Oct."],
        ["Result of Nov.","Result of Dec.","Result of Jan."]
    ];
    const listMonthOfCurrentQuarter = listMonthOfEachQuarter[currentQuarter - 1];
    return listMonthOfCurrentQuarter;
  }
  loadData() {
    this.service.getAllInCurrentQuarterByAccountGroup(+JSON.parse(localStorage.getItem('user')).id).subscribe(data => {
      this.gridData = data;
    });
  }
  loadKPIScoreData() {
    this.kpiScoreService.getById(this.data.id).subscribe(data => {
      this.kpiScoreData = data;
    });
  }
  getFisrtByAccountId() {
    this.kpiScoreService.getFisrtByAccountId(+JSON.parse(localStorage.getItem('user')).id).subscribe(data => {
      this.point = data?.point;
      this.kpiScoreModel.id = data?.id;
    });
  }
  getFisrtCommentByObjectiveId() {
    this.commentService.getFisrtByObjectiveId(this.data.id, +JSON.parse(localStorage.getItem('user')).id).subscribe(data => {
      this.content = data?.content;
      this.commentModel.id = data?.id;
    });
  }
  loadKPIData() {
    this.kpiService.getAll().subscribe(data => {
      this.kpiData = data;
    });
  }
  public queryCellInfoEvent: EmitType<QueryCellInfoEventArgs> = (args: QueryCellInfoEventArgs) => {
    const data = args.data as ToDoListOfQuarter;
    const fields = ['month'];
    if (fields.includes(args.column.field)) {
      args.rowSpan = (this.gridData as any).filter(
        item => item.month === data.month
      ).length;
    }
    if (args.column.field.includes("resultOfMonth")) {
      args.rowSpan = (this.gridData as any).filter(
        item => item.month === data.month
      ).length;
    }
  }

  addKPIScore() {

    this.kpiScoreModel.point = this.point;
    return this.kpiScoreService.add(this.kpiScoreModel);
  }
  addComment() {
    this.commentModel.content = this.content;
    return this.commentService.add(this.commentModel);
  }
  finish() {
    if (!this.point) {
      this.alertify.warning('Not yet complete. Can not submit!', true);
      return;
    }
    const kpiScore = this.addKPIScore();
    const comment = this.addComment();
    forkJoin([kpiScore, comment]).subscribe(response => {
      console.log(response)
      const arr = response.map(x=> x.success);
      const checker = arr => arr.every(Boolean);
      if (checker) {
        this.alertify.success(MessageConstants.CREATED_OK_MSG);
      } else {
        this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
      }
    })
  }
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }
}
