import { AttitudeScoreService } from 'src/app/_core/_service/attitude-score.service';
import { filter } from 'rxjs/operators';
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
import { PeriodType, SystemScoreType } from 'src/app/_core/enum/system';
import { AttitudeScore } from 'src/app/_core/_model/attitude-score';
@Component({
  selector: 'app-self-score',
  templateUrl: './self-score.component.html',
  styleUrls: ['./self-score.component.scss']
})
export class SelfScoreComponent implements OnInit {
  @ViewChild('grid') grid: GridComponent;
  @Input() data: any;
  @Input() periodTypeCode: PeriodType;
  @Input() scoreType: SystemScoreType;
  gridData: object;
  toolbarOptions = ['Search'];
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
  quarterlySettingsData = [];
  columns = [];
  attitudeScoreData:  AttitudeScore;
  constructor(
    public activeModal: NgbActiveModal,
    public service: Todolistv2Service,
    public kpiScoreService: KPIScoreService,
    public attitudeScoreService: AttitudeScoreService,
    public kpiService: KPIService,
    public commentService: Commentv2Service,
    private alertify: AlertifyService,
    private utilitiesService: UtilitiesService
  ) { }

  ngOnInit(): void {
    this.kpiScoreModel = {
      id: 0,
      periodTypeId: this.data.periodTypeId,
      period: this.data.period,
      point: this.point,
      scoreBy: +JSON.parse(localStorage.getItem('user')).id,
      modifiedTime: null,
      createdTime: new Date().toDateString(),
      accountId: +JSON.parse(localStorage.getItem('user')).id,
      scoreType: this.scoreType
    }

    this.getQuarterlySetting();
    this.loadData();
    this.loadKPIData();
    this.getFisrtSelfScoreByAccountId();
    this.getFunctionalLeaderCommentByAccountId();
    this.getFunctionalLeaderAttitudeScoreByAccountId();
  }
  getMonthListInCurrentQuarter(index) {

    const listMonthOfEachQuarter =
        [
        "Result of Jan.",
        "Result of Feb.","Result of Mar.","Result of Apr.",
        "Result of May.","Result of Jun.","Result of Jul.",
        "Result of Aug.","Result of Sep.","Result of Oct.",
        "Result of Nov.","Result of Dec."
       ]
    ;
    const listMonthOfCurrentQuarter = listMonthOfEachQuarter[index - 1];
    return listMonthOfCurrentQuarter;
  }
  getQuarterlySetting() {
    this.quarterlySettingsData = this.data.settings || [];
    this.columns =[];
    for (const month of this.quarterlySettingsData) {
      this.columns.push({ field: `${month}`,
      headerText: this.getMonthListInCurrentQuarter(month),
      month: month
     })
    }
  }
  loadData() {
    this.service.getAllKPIScoreL0ByPeriod(this.data.period).subscribe(data => {
      this.gridData = data;
    });
  }

  getFunctionalLeaderAttitudeScoreByAccountId() {
    this.attitudeScoreService.getFunctionalLeaderAttitudeScoreByAccountId(
      +JSON.parse(localStorage.getItem('user')).id,
      this.data.periodTypeId,
      this.data.period
    ).subscribe(data => {
      this.attitudeScoreData = data;
    });
  }
  getFisrtSelfScoreByAccountId() {
    this.kpiScoreService.getFisrtSelfScoreByAccountId(
      +JSON.parse(localStorage.getItem('user')).id,
      this.data.periodTypeId,
      this.data.period,
      this.scoreType
    ).subscribe(data => {
      this.point = data?.point;
      this.kpiScoreModel.id = data?.id;
    });
  }

  getFunctionalLeaderCommentByAccountId() {
    this.commentService.getFunctionalLeaderCommentByAccountId(
      +JSON.parse(localStorage.getItem('user')).id,
      this.data.periodTypeId,
      this.data.period
    ).subscribe(data => {
      this.content = data?.content;
    });
  }
  loadKPIData() {
    this.kpiService.getAll().subscribe(data => {
      this.kpiData = data;
    });
  }
  public queryCellInfoEvent: EmitType<QueryCellInfoEventArgs> = (args: QueryCellInfoEventArgs) => {
    const data = args.data as any;
    const fields = ['month'];
    for (const month of this.quarterlySettingsData) {
      if (('' + month).includes(args.column.field)) {
        (args.cell as any).innerText = data.resultOfMonth.filter(x=>x.month === month)[0]?.title || "N/A";
      }
    }
  }

  addKPIScore() {
    this.kpiScoreModel.point = this.point;
    return this.kpiScoreService.add(this.kpiScoreModel);
  }

  finish() {
    if (!this.point) {
      this.alertify.warning('Not yet complete. Can not submit! 尚未完成，無法提交', true);
      return;
    }
    const kpiScore = this.addKPIScore();
    forkJoin([kpiScore]).subscribe(response => {
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
