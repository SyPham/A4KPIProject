import { ReportService } from './../../../../_core/_service/report.service';
import { AccountGroup } from './../../../../_core/_model/account.group';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-h1-h2-report',
  templateUrl: './h1-h2-report.component.html',
  styleUrls: ['./h1-h2-report.component.scss']
})
export class H1H2ReportComponent extends BaseComponent implements OnInit, AfterViewInit {
  data: any[] = [];
  modalReference: NgbModalRef;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  @ViewChild('grid') public grid: GridComponent;
  accountCreate: AccountGroup;
  accountUpdate: AccountGroup;
  setFocus: any;
  locale = localStorage.getItem('lang');
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: false, allowDeleting: false, mode: 'Normal' };
  toolbarOptions = ['Search'];
  @ViewChild('detailModal') detailModal: NgbModalRef;
  itemData: any;

  constructor(
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private service: ReportService,
    private route: ActivatedRoute,
  ) { super(); }
  ngAfterViewInit(): void {
  }

  ngOnInit() {
    this.loadData();
  }
  // end life cycle ejs-grid
  // api
  loadData() {
    this.service.geH1H2Data().subscribe(data => {
      this.data = data;
    });
  }
  openModal(data) {
    this.itemData = data;
    const modalRef = this.modalService.open(this.detailModal, { size: 'xl', backdrop: 'static' });
    modalRef.result.then((result) => {
    }, (reason) => {
    });
  }
  exportExcel() {}
  // end api
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

}