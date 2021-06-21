import { ResultOfMonth, ResultOfMonthRequest } from './../../../../../_core/_model/result-of-month';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { Objective } from 'src/app/_core/_model/objective';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { ObjectiveService } from 'src/app/_core/_service/objective.service';
import { ResultOfMonthService } from 'src/app/_core/_service/result-of-month.service';
import { Todolistv2Service } from 'src/app/_core/_service/todolistv2.service';

@Component({
  selector: 'app-update-result',
  templateUrl: './update-result.component.html',
  styleUrls: ['./update-result.component.scss']
})
export class UpdateResultComponent implements OnInit {
  @ViewChild('grid') grid: GridComponent;
  @Input() data: Objective;
  gridData: object;
  gridResultOfMonthData: ResultOfMonth[];
  toolbarOptions = ['Search'];
  toolbarOptions2 = [''];
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  model: ResultOfMonthRequest;
  title: string;
  constructor(
    public activeModal: NgbActiveModal,
    private service: ObjectiveService,
    private todolistService: Todolistv2Service,
    private alertify: AlertifyService,
    private resultOfMonthService: ResultOfMonthService
  ) { }

  ngOnInit(): void {
    this.loadCurrentResultOfMonthData();
    this.loadData();
    this.model = {
      id: 0,
      title: ''
    };
  }
  loadData() {
    this.todolistService.getAllByObjectiveId(this.data.id).subscribe(data => {
      this.gridData = data;
    });
  }
  loadCurrentResultOfMonthData() {
    this.resultOfMonthService.getAllByObjectiveId(this.data.id).subscribe(data => {
      this.gridResultOfMonthData = data;
      this.title =  data[0].title;
    });
  }
  updateResultOfMonth() {
    const data = [];
    const gridData = this.gridResultOfMonthData || [];
    if (gridData.length === 0) {
      this.alertify.warning('Not yet complete. Can not submit!', true);
      return;
    }
    this.model.title = this.title
    this.model.id = gridData[0].id
    this.resultOfMonthService.updateResultOfMonth(this.model).subscribe(
      (res) => {
        if (res.success === true) {
          this.alertify.success(MessageConstants.UPDATED_OK_MSG);
          this.loadCurrentResultOfMonthData();
          // this.activeModal.close();
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
    this.updateResultOfMonth();
  }
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }
}
