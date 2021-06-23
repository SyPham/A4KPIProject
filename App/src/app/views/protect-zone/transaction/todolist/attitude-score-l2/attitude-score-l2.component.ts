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
import { AttitudeScoreService } from 'src/app/_core/_service/attitude-score.service';
import { AttitudeScore } from 'src/app/_core/_model/attitude-score';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { KPI } from 'src/app/_core/_model/kpi';
import { Comment } from 'src/app/_core/_model/commentv2';
import { Commentv2Service } from 'src/app/_core/_service/commentv2.service';
import { AttitudeService } from 'src/app/_core/_service/attitude.service';
import { Attitude } from 'src/app/_core/_model/attitude';
import { forkJoin } from 'rxjs';
@Component({
  selector: 'app-attitude-score-l2',
  templateUrl: './attitude-score-l2.component.html',
  styleUrls: ['./attitude-score-l2.component.scss']
})
export class AttitudeScoreL2Component implements OnInit {
  @ViewChild('grid') grid: GridComponent;
  @Input() data: ToDoListL1L2;
  gridData: object;
  toolbarOptions = ['Add', 'Delete', 'Search'];
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  model: ToDoList;
  attitudeScoreModel: AttitudeScore;
  attitudeScoreData: AttitudeScore;
  point: number;
  fields: object = { text: 'point', value: 'point' };
  filterSettings = { type: 'Excel' };
  content = '';
  commentModel: Comment;
  attitudeData: Attitude[];
  constructor(
    public activeModal: NgbActiveModal,
    public service: Todolistv2Service,
    public attitudeScoreService: AttitudeScoreService,
    public attitudeService: AttitudeService,
    public commentService: Commentv2Service,
    private alertify: AlertifyService,
    private utilitiesService: UtilitiesService
  ) { }

  ngOnInit(): void {
    this.attitudeScoreModel = {
      id: 0,
      periodType: "Quarter",
      period: this.utilitiesService.getQuarter(new Date()),
      point: this.point,
      objectiveId: this.data.id,
      scoreBy: +JSON.parse(localStorage.getItem('user')).id,
      createdTime: new Date().toDateString(),
      modifiedTime: null
    }
    this.commentModel =  {
      id: 0,
      content: this.content,
      createdBy: +JSON.parse(localStorage.getItem('user')).id,
      objectiveId: this.data.id,
      modifiedBy: null,
      createdTime: new Date().toDateString(),
      modifiedTime: null
    }


    this.loadData();
    this.loadAttitudeScoreData();
    this.loadKPIData();
    this.getFisrtByObjectiveIdAndScoreBy();
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
  loadAttitudeScoreData() {
    this.attitudeScoreService.getById(this.data.id).subscribe(data => {
      this.attitudeScoreData = data;
    });
  }
  getFisrtByObjectiveIdAndScoreBy() {
    this.attitudeScoreService.getFisrtByObjectiveIdAndScoreBy(this.data.id, +JSON.parse(localStorage.getItem('user')).id).subscribe(data => {
      this.point = data?.point;
      this.attitudeScoreModel.id = data?.id;
    });
  }
  getFisrtCommentByObjectiveId() {
    this.commentService.getFisrtByObjectiveId(this.data.id, +JSON.parse(localStorage.getItem('user')).id).subscribe(data => {
      this.content = data?.content;
      this.commentModel.id = data?.id;
    });
  }
  loadKPIData() {
    this.attitudeService.getAll().subscribe(data => {
      this.attitudeData = data;
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

  addAttitudeScore() {
    if (!this.point) {
      this.alertify.warning('Not yet complete. Can not submit!', true);
      return;
    }
    this.attitudeScoreModel.point = this.point;
    this.attitudeScoreService.add(this.attitudeScoreModel).subscribe( res => {
      if (res.success) {
        this.alertify.success(MessageConstants.CREATED_OK_MSG);
      } else {
        this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
      }
    }, err => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG));
  }
  addComment() {
    this.commentModel.content = this.content;
    return this.commentService.add(this.commentModel);
  }
  finish() {
    this.addAttitudeScore();
  }
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }
}
