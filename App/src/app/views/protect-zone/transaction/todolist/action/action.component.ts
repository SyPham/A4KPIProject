import { ToDoList } from './../../../../../_core/_model/todolistv2';
import { Todolistv2Service } from './../../../../../_core/_service/todolistv2.service';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Objective, ObjectiveRequest } from 'src/app/_core/_model/objective';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { GridComponent } from '@syncfusion/ej2-angular-grids';

@Component({
  selector: 'app-action',
  templateUrl: './action.component.html',
  styleUrls: ['./action.component.scss']
})
export class ActionComponent implements OnInit {
  @ViewChild('grid') grid: GridComponent;
  @Input() data: Objective;
  gridData: object;
  toolbarOptions = ['Add', 'Delete', 'Search'];
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  model: ToDoList;
  constructor(
    public activeModal: NgbActiveModal,
    public service: Todolistv2Service,
    private alertify: AlertifyService,
  ) { }

  ngOnInit(): void {
    this.loadData();
  }
  loadData() {
    this.service.getAllByObjectiveId(this.data.id).subscribe(data => {
      this.gridData = data;
    });
  }
  delete(id) {
    this.service.delete(id).subscribe(
      (res) => {
        if (res.success === true) {
          this.alertify.success(MessageConstants.DELETED_OK_MSG);
          this.loadData();
        } else {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        }
      },
      (err) => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
    );

  }
  create() {
    this.model = {
      id: 0,
      yourObjective: '',
      action:  '',
      remark:  '',
      progressId: null,
      objectiveId: this.data.id,
      createdBy: 0,
      modifiedBy: null,
      createdTime: new Date().toLocaleDateString(),
      modifiedTime: null,
    };
    this.service.add(this.model).subscribe(
      (res) => {
        if (res.success === true) {
          this.alertify.success(MessageConstants.CREATED_OK_MSG);
          this.loadData();
          this.model = {} as ToDoList;
        } else {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        }

      },
      (error) => {
        this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
      }
    );
  }

  createRange() {
    const data = [];
    const gridData = this.grid.currentViewData as ToDoList[] || [];
    if (gridData.length === 0) {
      this.alertify.warning('Not yet complete. Can not submit!', true);
      return;
    }
    for (const item of gridData) {
      const model = {
        id: item.id,
        yourObjective: item.yourObjective,
        action:  item.action,
        remark:  '',
        progressId: null,
        objectiveId: this.data.id,
        createdBy: 0,
        modifiedBy: null,
        createdTime: new Date().toLocaleDateString(),
        modifiedTime: null,
      };
      data.push(model);
    }

    this.service.addRange(data).subscribe(
      (res) => {
        if (res.success === true) {
          this.alertify.success(MessageConstants.CREATED_OK_MSG);
          this.loadData();
        } else {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        }

      },
      (error) => {
        this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
      }
    );
  }
  update() {

    this.service.update(this.model).subscribe(
      (res) => {
        if (res.success === true) {
          this.alertify.success(MessageConstants.UPDATED_OK_MSG);
          this.loadData();
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
    this.createRange();
  }
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }
}
