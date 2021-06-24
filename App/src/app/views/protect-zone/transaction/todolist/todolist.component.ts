import { KpiScoreComponent } from './kpi-score/kpi-score.component';
import { AccountGroupService } from './../../../../_core/_service/account.group.service';
import { Component, OnInit, TemplateRef, ViewChild, QueryList, ViewChildren } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { ObjectiveService } from 'src/app/_core/_service/objective.service';
import { ActionComponent } from './action/action.component';
import { SelfScoreComponent } from './self-score/self-score.component';
import { UpdateResultComponent } from './update-result/update-result.component';
import { AccountGroup } from 'src/app/_core/_model/account.group';
import { AttitudeScoreComponent } from './attitude-score/attitude-score.component';
import { Todolistv2Service } from 'src/app/_core/_service/todolistv2.service';
import { PeriodType, SystemRole,ToDoListType } from 'src/app/_core/enum/system';
import { AttitudeScoreL2Component } from './attitude-score-l2/attitude-score-l2.component';

@Component({
  selector: 'app-todolist',
  templateUrl: './todolist.component.html',
  styleUrls: ['./todolist.component.scss']
})
export class TodolistComponent implements OnInit {
  @ViewChild('grid') grid: GridComponent;
  @ViewChildren('GridtemplateRef') public Gridtemplates: QueryList< TemplateRef<any>>;
  gridData: object;
  toolbarOptions = ['Search'];
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  accountGroupData: AccountGroup[];
  KPI = ToDoListType.KPI as string;
  Attitude = ToDoListType.Attitude as string;
  constructor(
    private service: ObjectiveService,
    public todolistService: Todolistv2Service,
    private accountGroupService: AccountGroupService,
    public modalService: NgbModal,
  ) { }

  ngOnInit(): void {
    this.loadAccountGroupData();
  }
  selected(args) {
    const index = args.selectedIndex + 1;
    switch (index) {
      case SystemRole.L0:
      this.loadDataL0();
      break;
      case SystemRole.L1:
        this.loadDataL1L2();
      break;
      case SystemRole.L2:
        this.loadDataL1L2();
      break;
      case SystemRole.FHO:
      break;
      case SystemRole.GHR:
        this.loadDataGHR();
      break;
      case SystemRole.GM:
      break;
    }
  }
  getGridTemplate(index): TemplateRef<any> {
   return this.Gridtemplates.toArray()[index - 1];
  }
  loadDataL0() {
    this.todolistService.l0().subscribe(data => {
      this.gridData = data;
    });
  }

  loadDataL1L2() {
    this.todolistService.l1().subscribe(data => {
      this.gridData = data;
    });
  }
  loadDataGHR() {
    this.todolistService.getAllObjectiveByL1L2().subscribe(data => {
      this.gridData = data;
    });
  }
  loadAccountGroupData() {
    this.accountGroupService.getAll().subscribe(data => {
      this.accountGroupData = data;
    });
  }
  createdToolbar() {}
  onClickToolbar(args) {}
  onFilter(args) {}
  openActionModalComponent(data) {
    const modalRef = this.modalService.open(ActionComponent, { size: 'xl', backdrop : 'static' });
    modalRef.componentInstance.data = data;
    modalRef.result.then((result) => {
    }, (reason) => {
    });
  }
  openUpdateResultModalComponent(data) {
    const modalRef = this.modalService.open(UpdateResultComponent, { size: 'xl', backdrop : 'static' });
    modalRef.componentInstance.data = data;
    modalRef.result.then((result) => {
    }, (reason) => {
    });
  }
  openSelfScoreModalComponent(data) {
    const modalRef = this.modalService.open(SelfScoreComponent, { size: 'xl', backdrop : 'static' });
    modalRef.componentInstance.data = data;
    modalRef.componentInstance.periodTypeCode = PeriodType.Monthly;
    modalRef.result.then((result) => {
    }, (reason) => {
    });
  }
  openKPIScoreModalComponent(data) {
    const modalRef = this.modalService.open(KpiScoreComponent, { size: 'xl', backdrop : 'static' });
    modalRef.componentInstance.data = data;
    modalRef.result.then((result) => {
    }, (reason) => {
    });
  }
  openAttitudeScoreModalComponent(data) {
    const modalRef = this.modalService.open(AttitudeScoreComponent, { size: 'xl', backdrop : 'static' });
    modalRef.componentInstance.data = data;
    modalRef.result.then((result) => {
    }, (reason) => {
    });
  }
  openAttitudeScoreL2ModalComponent(data) {
    const modalRef = this.modalService.open(AttitudeScoreL2Component, { size: 'xl', backdrop : 'static' });
    modalRef.componentInstance.data = data;
    modalRef.result.then((result) => {
    }, (reason) => {
    });
  }

  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }
}
