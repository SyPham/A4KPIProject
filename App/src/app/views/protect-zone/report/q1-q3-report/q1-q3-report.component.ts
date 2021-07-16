import { AccountGroup } from './../../../../_core/_model/account.group';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { ReportService } from 'src/app/_core/_service/report.service';

@Component({
  selector: 'app-q1-q3-report',
  templateUrl: './q1-q3-report.component.html',
  styleUrls: ['./q1-q3-report.component.scss']
})
export class Q1Q3ReportComponent extends BaseComponent implements OnInit {
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
  detail: any;
  itemData: any;

  constructor(
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private service: ReportService,
    private route: ActivatedRoute,
  ) { super(); }

  ngOnInit() {
    this.loadData();
  }
  openModal(data) {
    this.itemData = data;
    this.getQ1Q3ReportInfo(data.id);
    const modalRef = this.modalService.open(this.detailModal, { size: 'xl', backdrop: 'static' });
    modalRef.result.then((result) => {
    }, (reason) => {
    });
  }
  toolbarClick(args) {
    switch (args.item.id) {
      case 'grid_excelexport':
        this.grid.excelExport({ hierarchyExportMode: 'All' });
        break;
      default:
        break;
    }
  }
  exportExcel() {
    this.service.q1q3ExportExcel(this.itemData.id).subscribe((data: any) => {
      const blob = new Blob([data],
        { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

      const downloadURL = window.URL.createObjectURL(data);
      const link = document.createElement('a');
      link.href = downloadURL;
      link.download = 'Q1,Q3 Report 季報表.xlsx';
      link.click();
    });
  }

  // end life cycle ejs-grid

  // api

  loadData() {
    this.service.geH1H2Data().subscribe(data => {
      this.data = data;
    });
  }
  getQ1Q3ReportInfo(id) {
    this.service.getQ1Q3ReportInfo(id).subscribe(data => {
      this.detail = data;
    });
  }
  // end api
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

}
