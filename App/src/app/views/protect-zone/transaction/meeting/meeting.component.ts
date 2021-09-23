import { filter } from 'rxjs/operators';
import { OcPolicyService } from './../../../../_core/_service/OcPolicy.service';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { EditService, ToolbarService, PageService, GridComponent, QueryCellInfoEventArgs, ColumnModel } from '@syncfusion/ej2-angular-grids';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { Account2Service } from 'src/app/_core/_service/account2.service';
import { Account } from 'src/app/_core/_model/account';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { AccountGroupService } from 'src/app/_core/_service/account.group.service';
import { AccountGroup } from 'src/app/_core/_model/account.group';
import { OcService } from 'src/app/_core/_service/oc.service';
import { MeetingService } from 'src/app/_core/_service/meeting.service';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import * as Chart from 'chart.js';
import { EmitType } from '@syncfusion/ej2-base';
import { DatePipe } from '@angular/common';
import { Todolist2Service } from 'src/app/_core/_service/todolist2.service';
@Component({
  selector: 'app-meeting',
  templateUrl: './meeting.component.html',
  styleUrls: ['./meeting.component.scss'],
  providers: [DatePipe]
})
export class MeetingComponent extends BaseComponent implements OnInit {

  data: Account[] = [];
  password = '';
  modalReference: NgbModalRef;
  fields: object = { text: 'name', value: 'id' };
  leaderFields: object = { text: 'fullName', value: 'id' };
  managerFields: object = { text: 'fullName', value: 'id' };
  // toolbarOptions = ['Search'];
  passwordFake = `aRlG8BBHDYjrood3UqjzRl3FubHFI99nEPCahGtZl9jvkexwlJ`;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  @ViewChild('grid') public grid: GridComponent;
  accountCreate: Account;
  accountUpdate: Account;
  setFocus: any;
  locale = localStorage.getItem('lang');
  accountGroupData: AccountGroup[];
  accountGroupItem: any;
  leaders: any[] = [];
  managers: any[] = [];
  leaderId: number;
  managerId: number;
  accounts: any[];
  oclv3Data: any [];
  modalRef: NgbModalRef;
  policyData: Object;
  @ViewChild('detailModal') detailModal: NgbModalRef;
  unit: string = null;
  period: string = null
  plugins: any = [pluginDataLabels]
  options: any = {
    responsive: true,
    maintainAspectRatio: false,
    showScale: false,
    plugins: {
      datalabels: {
        backgroundColor: function(context) {
          return context.dataset.backgroundColor;
        },
        borderRadius: 4,
        color: 'white',
        font: {
          weight: 'bold'
        },
        formatter: function(value, context) {
          return value
        }
      }
    },
    title: {
      display: true,
      text: '',
      fontSize: 14,
      fontColor: 'black'
    },
    elements: {
      point: {
        radius: 0
      },
      line: {
        tension: 0.2
      }
    },
    scales: {
      yAxes: [
        {
          stacked: true,
          display: true,
          position: 'left',
          ticks: {
            beginAtZero: true,
            padding: 10,
            fontSize: 12,
            stepSize: 10,
            min: -5
          },
          scaleLabel: {
            display: true,
            labelString: this.unit,
            fontSize: 12,
            fontStyle: 'normal'
          }
        }
      ],
      xAxes: [
        {
          gridLines: {
            display: true,
            tickMarkLength: 8
          },
          ticks: {
            fontSize: 12
          },
          scaleLabel: {
            display: true,
            labelString: this.period,
            fontSize: 12,
            fontStyle: 'normal'
          }
        }
      ]
    }
  }
  pointBackgroundColors: any[] = []
  chart: any = {}
  datasets: any;
  targets: any;
  perfomance: any;
  labels: any;
  unitId: any;
  unitName: any;
  dataTable: any;
  @ViewChild('gridDataTable') public grid2: GridComponent;
  dataLevel: any
  dataPic: any
  levelFields: object = { text: 'name', value: 'id' };
  picFields: object = { text: 'name', value: 'id' };
  policyDataTamp: any;
  levelId: number = 0
  picId: number = 0
  currentTime: any;
  policyTitle: string
  kpiTitle: string
  levelTitle: string
  public monthColumns = [
    {
      headerText: '內容',
      textAlign: 'Center',
    }
  ];
  public PColumns = [
    {
      headerText: '月份計劃',
      textAlign: 'Center',
    }
  ];
  public TargetColumns = [
    {
      headerText: '目標值',
      textAlign: 'Center',
    }
  ];
  public DeadlineColumns = [
    {
      headerText: '完成期限',
      textAlign: 'Center',
    }
  ];
  public DColumns = [
    {
      headerText: '執行狀況',
      textAlign: 'Center',
    }
  ];
  public AchievementColumns = [
    {
      headerText: '實績',
      textAlign: 'Center',
    }
  ];
  public StatusColumns = [
    {
      headerText: '狀態',
      textAlign: 'Center',
    }
  ];
  public CColumns = [
    {
      headerText: '執行分析檢討',
      textAlign: 'Center',
    }
  ];
  public AColumns = [
    {
      headerText: '計畫執行',
      textAlign: 'Center',
    }
  ];
  public AttatchmentColumns = [
    {
      headerText: '附檔',
      textAlign: 'Center',
    }
  ];

  picTitle: string
  kpiId: any;
  constructor(
    private service: Account2Service,
    private accountGroupService: AccountGroupService,
    public modalService: NgbModal,
    private ocService: OcService,
    private datePipe: DatePipe,
    private ocPolicyService: OcPolicyService,
    private meetingService: MeetingService,
    private alertify: AlertifyService,
    public todolist2Service: Todolist2Service,
    private route: ActivatedRoute,
  ) { super(); }

  ngOnInit() {
    this.getAllOcLv3();
    this.getAllKpi();
    this.currentTime = new Date();
  }
  filterlevel(args) {
    this.levelId = args.value
    if(this.levelId === 0) {
      this.policyData = this.policyDataTamp
    }
    if(this.levelId > 0) {
      this.policyData = this.policyDataTamp.filter(x => x.level == this.levelId)
    }

    if(this.levelId === 0 && this.picId > 0) {
      this.policyData = this.policyDataTamp.filter(x =>  x.picId == this.picId)
    }
  }
  filterPic(args) {
    this.picId = args.value
    if(this.levelId === 0 && this.picId === 0) {
      this.policyData = this.policyDataTamp
    }
    if(this.levelId > 0 && this.picId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.level == this.levelId)
    }
    if(this.levelId > 0 && this.picId > 0) {
      this.policyData = this.policyDataTamp.filter(x => x.level == this.levelId && x.picId == this.picId)
    }
    if(this.levelId === 0 && this.picId > 0) {
      this.policyData = this.policyDataTamp.filter(x => x.picId == this.picId)
    }
  }
  public queryCellInfoEvent: EmitType<QueryCellInfoEventArgs> = (args: QueryCellInfoEventArgs) => {
    // console.log(args);
    const data = args.data as any;
    const dataTable = this.dataTable.filter((thing, i, arr) => {
      return arr.indexOf(arr.find(t => t.actionId === thing.actionId)) === i;
    });
    const fields = ['month', 'cContent'];
    if (fields.includes(args.column.field)) {
      args.rowSpan = this.dataTable.filter(
        item => item.month === data.month &&
          item.cContent === data.cContent
      ).length;
    }
  }
  scroll(el: HTMLElement) {
    el.scrollIntoView();
  }
  openModal(data, model) {
    this.policyTitle = data.policyName
    this.kpiTitle = data.name
    this.levelTitle = data.level
    this.picTitle = data.picName
    this.unitId = data.typeId
    this.unitName = data.typeName
    this.kpiId = data.id
    this.loadDataModel(data.id)
    this.loadDataModel2(data.id)
    this.modalRef = this.modalService.open(model, { size: 'lg', backdrop: 'static' });
  }
  createChart(chartId, labels, unit) {
    const ctx = document.getElementById(chartId) as HTMLCanvasElement;
    let optionss: any = {
      plugins: {
        datalabels: {
          backgroundColor: function(context) {
            return context.dataset.backgroundColor;
          },
          borderRadius: 4,
          color: 'white',
          font: {
            weight: 'bold'
          },
          formatter: function(value, context) {
            // if(unit === 'Percentage'){
            //   return value + '%';
            // }else {
            // }
            return value
          }
        }
      },
      scales: {
        yAxes: [
          {
            display: true,
            stacked: false,
            position: 'left',
            ticks: {
              beginAtZero: true,
              padding: 10,
              fontSize: 12,
              stepSize: 10,
              min: -5
            },
            scaleLabel: {
              display: true,
              fontSize: 12,
              fontStyle: 'normal'
            }
          }
        ],
        xAxes: [
          {
            gridLines: {
              display: true,
              tickMarkLength: 8
            },
            ticks: {
              fontSize: 12
            },
            scaleLabel: {
              display: true,
              fontSize: 12,
              fontStyle: 'normal'
            }
          }
        ]
      },
      title: {
        display: true,
        text: this.options.title.text,
        fontSize: 14,
        fontColor: 'black'
      },
      elements: {
        point: {
          radius: 0
        },
        line: {
          tension: 0.2
        }
      },
    }
    const myChart = new Chart(ctx, {
      type: 'line',
      data:{
        // tslint:disable-next-line: object-literal-shorthand
        labels: labels,
        datasets: [
          {
            // tslint:disable-next-line: object-literal-shorthand
            label: "This month perfomance",
            backgroundColor: '#008cff',
            borderColor: '#008cff',
            fill: false,
            data: this.perfomance,
          },
          {
            label: 'This month targets',
            data: this.targets,
            backgroundColor: '#ff8c00',
            borderColor: '#ff8c00',
            fill: false,
          }
        ]
      },
      options: optionss
    })
    this.chart = myChart
  }
  loadDataModel(id) {
    // this.meetingService.getChart(id).subscribe((res: any) => {
    //   this.perfomance = res.perfomances
    //   this.targets = res.targets
    //   this.labels = res.labels
    //   this.dataTable = res.dataTable
    //   this.createChart(
    //     'planet-chart',
    //     this.labels,
    //     this.unitName
    //   )
    // })
  }
  download(date) {
    this.todolist2Service.download(this.kpiId, date).subscribe((data: any) => {
      const blob = new Blob([data],
        { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

      const downloadURL = window.URL.createObjectURL(data);
      const link = document.createElement('a');
      link.href = downloadURL;
      const ct = new Date();
      link.download = `${ct.getFullYear()}${ct.getMonth()}${ct.getDay()}_archive.zip`;
      link.click();
    });
  }
  loadDataModel2(id) {
    this.meetingService.getChartWithTime(id,this.datePipe.transform(this.currentTime, "YYYY-MM-dd HH:mm")).subscribe((res: any) => {
      this.perfomance = res.perfomances
      this.targets = res.targets
      this.labels = res.labels
      this.dataTable = res.dataTable.filter(x => x.currentMonthData.length > 0)
      console.log(this.dataTable );
      this.createChart(
        'planet-chart',
        this.labels,
        this.unitName
      )

    })
  }

  getAllKpi() {
    this.meetingService.getAllKpi().subscribe((res: any) => {
      this.policyData = res
      this.policyDataTamp = res
      const level = res.map((item: any) => {
        return {
          id: item.level,
          name: "Level " + item.level
        }
      })
      this.dataLevel = level.filter((thing, i, arr) => {
        return arr.indexOf(arr.find(t => t.id === thing.id)) === i;
      });

      const pic = res.map((item: any) => {
        return {
          id: item.picId,
          name: item.picName
        }
      })
      this.dataPic = [...new Map(pic.map(items => [items[items.id], items])).values()];
      this.dataPic = pic.filter((thing, i, arr) => {
        return arr.indexOf(arr.find(t => t.id === thing.id)) === i;
      });

      this.dataLevel.unshift({ name: "All", id: 0 });
      this.dataPic.unshift({ name: "All", id: 0 });
    })
  }

  getAllPolicy(){
    this.ocPolicyService.getAllPolicy().subscribe(res => {
      this.policyData = res ;
    })
  }
  getAllOcLv3() {
    this.ocService.getAllLv3().subscribe((res: any) => {
      this.oclv3Data = res
    })
  }
  // life cycle ejs-grid
  createdManager($event, data) {
    this.managers = this.accounts;
    this.managers = this.managers.filter(x=> x.id !== data.id);
  }
  createdLeader($event, data) {
    this.leaders = this.accounts.filter(x=> x.isLeader);
    this.leaders = this.leaders.filter(x=> x.id !== data.id);
  }
  onDoubleClick(args: any): void {
    this.setFocus = args.column; // Get the column from Double click event
  }
  initialModel() {
    this.accountGroupItem = [];
  }
  updateModel(data) {
    this.accountGroupItem = data.factory;
  }
  actionBegin(args) {
    if (args.requestType === 'add') {
      this.initialModel();
    }
    if (args.requestType === 'beginEdit') {
      const item = args.rowData;
      this.updateModel(item);
    }
    if (args.requestType === 'save' && args.action === 'add') {

      const model = {
        Name: args.data.name,
        OcIdList: this.accountGroupItem
      }
      if (args.data.name === undefined) {
        this.alertify.error('Please key in policy name! <br> Vui lòng nhập Policy!');
        args.cancel = true;
        return;
      }

      this.create(model);

    }
    if (args.requestType === 'save' && args.action === 'edit') {
      const model = {
        ID: args.data.id,
        Name: args.data.name,
        OcIdList: this.accountGroupItem
      }
      this.update(model);
    }
    if (args.requestType === 'delete') {
      this.delete(args.data[0].id);
    }
  }
 deleteOption(id) {
  this.ocPolicyService.deletePolicy(id).subscribe(res => {
    if(res) {
      this.alertify.success(MessageConstants.CREATED_OK_MSG);
      this.getAllPolicy()
      this.accountGroupItem = []
    } else {
      this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
    }
  })
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
  actionComplete(args) {
    if (args.requestType === 'add') {
      args.form.elements.namedItem('name').focus(); // Set focus to the Target element
    }
  }

  // end life cycle ejs-grid

  // api

  lock(id): void {
    this.service.lock(id).subscribe(
      (res) => {
        if (res.success === true) {
          const message = res.message;
          this.alertify.success(message);
          this.loadData();
        } else {
           this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        }
      },
      (err) => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
    );
  }

  loadData() {
    this.service.getAll().subscribe(data => {
      this.data = data;
    });
  }
  getAccounts() {
    this.service.getAccounts().subscribe(data => {
      this.accounts = data;
      this.leaders = data.filter(x=> x.isLeader);
      this.managers = data;
    });
  }
  loadAccountGroupData() {
    this.accountGroupService.getAll().subscribe(data => {
      this.accountGroupData = data;
    });
  }
  delete(id) {
    this.ocPolicyService.deletePolicy(id).subscribe(res => {
      if(res) {
        this.alertify.success(MessageConstants.CREATED_OK_MSG);
        this.getAllPolicy()
        this.accountGroupItem = []
      } else {
        this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
      }
    })
  }
  create(model) {
    this.ocPolicyService.addPolicy(model).subscribe(res => {
      if(res) {
        this.alertify.success(MessageConstants.CREATED_OK_MSG);
        this.getAllPolicy()
        this.accountGroupItem = []
      } else {
        this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
      }
    })
  }
  update(model) {
    this.ocPolicyService.updatePolicy(model).subscribe(res => {
      if(res) {
        this.alertify.success(MessageConstants.CREATED_OK_MSG);
        this.getAllPolicy()
        this.accountGroupItem = []
      } else {
        this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
      }
    })
  }
  // end api
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

}
