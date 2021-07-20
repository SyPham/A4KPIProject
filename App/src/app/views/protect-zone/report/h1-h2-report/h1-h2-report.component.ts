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
  detailH1: any;
  detailH2: any;
  avg: number = 0;
  modalRef: NgbModalRef;
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
    this.service.getQ1Q3Data().subscribe(data => {
      this.data = data;
    });
  }
  openModal(data , model) {
    this.getH1H2ReportInfo(data.id);
    this.modalRef = this.modalService.open(model, { size: 'xl', backdrop: 'static' });

  }

  getH1H2ReportInfo(id) {
    this.service.getH1H2ReportInfo(id).subscribe((data: any) => {
      this.detailH1 = data.h1;
      console.log(data);
      this.detailH2 = data.h2;
      this.avg = data.avg
    });
  }
  exportExcel() {

  }
  exportExcel2(tableID, filename = "") {
    var downloadLink;
    var dataType = "application/vnd.ms-excel";
    var tableSelect = document.getElementById(tableID);
    var tableHTML = tableSelect.outerHTML.replace(/ /g, "%20");

    filename = filename ? filename + ".xls" : "excel_data.xls";

    downloadLink = document.createElement("a");

    document.body.appendChild(downloadLink);

    if (navigator.msSaveOrOpenBlob) {
      var blob = new Blob(["\ufeff", tableHTML], {
        type: dataType,
      });
      navigator.msSaveOrOpenBlob(blob, filename);
    } else {
      downloadLink.href = "data:" + dataType + ", " + tableHTML;

      downloadLink.download = filename;
      downloadLink.click();
    }
  }
  // end api
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

}
