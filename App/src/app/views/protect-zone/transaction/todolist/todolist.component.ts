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
  constructor(
    private service: ObjectiveService,
    private accountGroupService: AccountGroupService,
    public modalService: NgbModal,
  ) { }

  ngOnInit(): void {
    this.loadAccountGroupData();
    this.loadData();
  }
  getGridTemplate(index): TemplateRef<any> {
   return this.Gridtemplates.toArray()[index - 1];
  }
  loadData() {
    this.service.getAll().subscribe(data => {
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

  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }
}
