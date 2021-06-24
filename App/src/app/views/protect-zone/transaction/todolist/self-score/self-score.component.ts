import { UtilitiesService } from './../../../../../_core/_service/utilities.service';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { Objective } from 'src/app/_core/_model/objective';
import { ToDoList, ToDoListOfQuarter } from 'src/app/_core/_model/todolistv2';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { Todolistv2Service } from 'src/app/_core/_service/todolistv2.service';

import { QueryCellInfoEventArgs } from '@syncfusion/ej2-angular-grids';
import { EmitType } from '@syncfusion/ej2-base';
import { KPIScoreService } from 'src/app/_core/_service/kpi-score.service';
import { KPIScore } from 'src/app/_core/_model/kpi-score';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { KPIService } from 'src/app/_core/_service/kpi.service';
import { KPI } from 'src/app/_core/_model/kpi';
import { PeriodType } from 'src/app/_core/enum/system';
@Component({
  selector: 'app-self-score',
  templateUrl: './self-score.component.html',
  styleUrls: ['./self-score.component.scss']
})
export class SelfScoreComponent implements OnInit {
  @ViewChild('grid') grid: GridComponent;
  @Input() data: Objective;
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
  constructor(
    public activeModal: NgbActiveModal,
    public service: Todolistv2Service,
    public kpiScoreService: KPIScoreService,
    public kpiService: KPIService,
    private alertify: AlertifyService,
    private utilitiesService: UtilitiesService
  ) { }

  ngOnInit(): void {
    this.kpiScoreModel = {
      id: 0,
      periodTypeId: 0,
      periodTypeCode: PeriodType.Monthly,
      period: this.utilitiesService.getQuarter(new Date()),
      point: this.point,
      scoreBy: +JSON.parse(localStorage.getItem('user')).id,
      modifiedTime: null,
      createdTime: new Date().toDateString(),
      accountId: +JSON.parse(localStorage.getItem('user')).id
    }
    this.loadData();
    this.loadKPIScoreData();
    this.loadKPIData();
    this.getFisrtByAccountId();
  }
  loadData() {
    this.service.getAllKPISelfScoreByObjectiveId(this.data.id).subscribe(data => {
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
  loadKPIData() {
    this.kpiService.getAll().subscribe(data => {
      this.kpiData = data;
    });
  }
  public queryCellInfoEvent: EmitType<QueryCellInfoEventArgs> = (args: QueryCellInfoEventArgs) => {
    // const data = args.data as ToDoListOfQuarter;
    // const fields = ['month'];
    // if (fields.includes(args.column.field)) {
    //   args.rowSpan = (this.gridData as any).filter(
    //     item => item.month === data.month
    //   ).length;
    // }
    // if (args.column.field.includes("resultOfMonth")) {
    //   args.rowSpan = (this.gridData as any).filter(
    //     item => item.month === data.month
    //   ).length;
    // }
  }
  addKPIScore() {
    if (!this.point) {
      this.alertify.warning('Not yet complete. Can not submit!', true);
      return;
    }
    this.kpiScoreModel.point = this.point;
    this.kpiScoreService.add(this.kpiScoreModel).subscribe(
      (res) => {
        if (res.success === true) {
          this.alertify.success(MessageConstants.CREATED_OK_MSG);
          this.getFisrtByAccountId();
        } else {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        }
      },
      (error) => {
        this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
      }
    );
  }
  finish() {
    this.addKPIScore();
  }
}
