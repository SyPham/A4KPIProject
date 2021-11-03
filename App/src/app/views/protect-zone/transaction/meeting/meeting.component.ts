import { element } from 'protractor';
import { filter } from 'rxjs/operators';
import { OcPolicyService } from './../../../../_core/_service/OcPolicy.service';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
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
import { Tooltip } from '@syncfusion/ej2-angular-popups';
import { environment } from 'src/environments/environment';
import { DataService } from 'src/app/_core/_service/data.service';
import { Custom } from 'src/app/_core/_model/contribution';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-meeting',
  templateUrl: './meeting.component.html',
  styleUrls: ['./meeting.component.scss'],
  providers: [DatePipe]
})
export class MeetingComponent extends BaseComponent implements OnInit , AfterViewInit {

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
  editSettingsMeeting = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: false, allowDeleting: false, mode: 'Normal' };
  locale = localStorage.getItem('lang');
  accountGroupData: AccountGroup[];
  accountGroupItem: any;
  leaders: any[] = [];
  managers: any[] = [];
  leaderId: number;
  managerId: number;
  accounts: any[];
  typeId: any;
  oclv3Data: any [];
  modalRef: NgbModalRef;
  policyData: Object;
  @ViewChild('detailModal') detailModal: NgbModalRef;
  @ViewChild('fileModal') fileModal: NgbModalRef;
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
  dataFact: any
  dataCenter: any
  dataDept: any
  dataPic: any
  levelFields: object = { text: 'name', value: 'id' };
  factoryFields: object = { text: 'name', value: 'name' };
  picFields: object = { text: 'name', value: 'id' };
  policyDataTamp: any;
  levelId: number = 0
  factId: number = 0
  centerId: number = 0
  deptId: number = 0
  picId: number = 0
  currentTime: any;
  policyTitle: string
  kpiTitle: string
  levelTitle: string
  picTitle: string
  @ViewChild('content', { static: true }) elementView: ElementRef;
  contentHeight: number;
  files = [];
  kpiId: any;
  base = environment.apiUrl.replace('/api/', '');
  YTD: any;
  targetYTD: any;
  dataOc: any;
  typeText: any;
  factName: any;
  centerName: any;
  deptName: any;
  changeLocalHome = [];
  dataHeight: any = [];
  ytds: any;
  roleUser: any;
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
    private spinner: NgxSpinnerService,
    private dataService: DataService
  ) { super();


  }

  ngOnInit() {
    this.getAllOcLv3();
    this.getAllKpi();
    this.getAllOc();
    this.roleUser = JSON.parse(localStorage.getItem('level')).code;
    this.currentTime = new Date();

  }
  ngOnDestroy() {
    this.changeLocalHome.forEach(item => item.unsubscribe());
  }
  getAllOc(){
    this.ocService.getAll().subscribe((res: any) => {
      this.dataOc = res
      this.dataFact = res.filter(x => x.level === 3)
      this.dataFact.unshift({ name: "All", id: 0 });

    })
  }
  ngAfterViewInit() {

  }

  filterFact(args) {
    this.factId = args.value
    this.factName = args.itemData.name
    this.dataCenter = this.dataOc.filter(x => x.parentId === this.factId)
    this.dataCenter.unshift({ name: "All", id: 0 });
    if(this.factId === 0 && this.centerId === 0 && this.deptId === 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp
    }
    if(this.factId > 0 && this.centerId === 0 && this.deptId === 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName))
    }
    if(this.factId > 0 && this.centerId > 0 && this.deptId === 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.centerName.includes(this.centerName))
    }
    if(this.factId > 0 && this.centerId > 0 && this.deptId > 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.centerName.includes(this.centerName) && x.deptName.includes(this.deptName))
    }
    if(this.factId > 0 && this.centerId > 0 && this.deptId > 0 && this.levelId > 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.centerName.includes(this.centerName) && x.deptName.includes(this.deptName) && x.level == this.levelId)
    }
  }

  filterCenter(args) {
    this.centerId = args.value
    this.centerName = args.itemData.name
    this.dataDept = this.dataOc.filter(x => x.parentId === this.centerId)
    this.dataDept.unshift({ name: "All", id: 0 });
    if(this.centerId === 0 && this.factId === 0 && this.deptId === 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp
    }
    if(this.centerId > 0 && this.factId === 0 && this.deptId === 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.centerId.includes(this.centerName))
    }
    if(this.centerId > 0 && this.factId > 0 && this.deptId === 0 && this.levelId > 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.centerName.includes(this.centerName) && x.level == this.levelId)
    }
    if(this.centerId === 0 && this.factId > 0 && this.deptId === 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName))
    }
    if(this.centerId > 0 && this.factId > 0 && this.deptId === 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.centerName.includes(this.centerName))
    }
    if(this.factId > 0 && this.centerId > 0 && this.deptId > 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.centerName.includes(this.centerName) && x.deptName.includes(this.deptName))
    }
    if(this.factId > 0 && this.centerId > 0 && this.deptId > 0 && this.levelId > 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.centerName.includes(this.centerName) && x.deptName.includes(this.deptName) && x.level == this.levelId)
    }
  }

  filterDept(args) {
    this.deptId = args.value
    this.deptName = args.itemData.name
    if(this.deptId === 0 && this.factId === 0 && this.centerId === 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp
    }
    if(this.deptId > 0 && this.factId === 0 && this.centerId === 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.deptName.includes(this.deptName))
    }
    if(this.deptId > 0 && this.factId > 0 && this.centerId === 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.deptName.includes(this.deptName))
    }
    if(this.deptId > 0 && this.centerId > 0 && this.factId > 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.centerName.includes(this.centerName) && x.deptName.includes(this.deptName))
    }
    if(this.factId > 0 && this.centerId > 0 && this.deptId > 0 && this.levelId > 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.centerName.includes(this.centerName) && x.deptName.includes(this.deptName) && x.level == this.levelId)
    }
    if(this.deptId === 0 && this.centerId > 0 && this.factId > 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.centerName.includes(this.centerName))
    }
  }

  filterlevel(args) {
    this.levelId = args.value
    if(this.levelId === 0 && this.factId === 0 && this.centerId === 0 && this.deptId === 0) {
      this.policyData = this.policyDataTamp
    }
    if(this.levelId > 0 && this.factId === 0 && this.centerId === 0 && this.deptId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.level == this.levelId)
    }
    if(this.levelId > 0 && this.factId > 0 && this.deptId === 0 && this.centerId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.level == this.levelId)
    }
    if(this.levelId > 0 && this.factId > 0 && this.centerId > 0 && this.deptId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.centerName.includes(this.centerName) && x.level == this.levelId)
    }
    if(this.levelId === 0 && this.factId > 0 && this.centerId > 0 && this.deptId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.centerName.includes(this.centerName))
    }
    if(this.factId > 0 && this.centerId > 0 && this.deptId > 0 && this.levelId > 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.centerName.includes(this.centerName) && x.deptName.includes(this.deptName) && x.level == this.levelId)
    }
    if(this.factId > 0 && this.centerId > 0 && this.deptId > 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) && x.centerName.includes(this.centerName) && x.deptName.includes(this.deptName))
    }
    if(this.factId > 0 && this.centerId === 0 && this.deptId === 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.factName.includes(this.factName) )
    }
    if(this.factId === 0 && this.centerId > 0 && this.deptId === 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.centerName.includes(this.centerName) )
    }

    if(this.factId === 0 && this.centerId === 0 && this.deptId > 0 && this.levelId === 0) {
      this.policyData = this.policyDataTamp.filter(x => x.deptName.includes(this.deptName))
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

  }
  scroll(el: HTMLElement) {
    el.scrollIntoView();
  }
  openModal(data, model) {
    this.typeText = data.typeText
    this.kpiTitle = data.name
    this.levelTitle = data.level
    this.picTitle = data.picName
    this.unitId = data.typeId
    this.unitName = data.typeName
    this.kpiId = data.id
    this.loadDataModel2(this.kpiId)
    this.modalRef = this.modalService.open(model, { size: 'lg', backdrop: 'static' });
    this.modalRef.result.then((result) => {
      this.perfomance = []
      this.targets = []
      this.labels = []
      this.dataTable = []
      // this.changeLocalHome.unsubscribe();
      this.dataHeight = []
    }, (reason) => {
      this.perfomance = []
      this.targets = []
      this.labels = []
      this.dataTable = []
      // this.changeLocalHome.unsubscribe();
      this.dataHeight = []

    });

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
  download(date, model) {
    this.modalRef = this.modalService.open(model, { size: 'sm', backdrop: 'static' });
    this.todolist2Service.getDownloadFilesMeeting(this.kpiId,date).subscribe((res: any) => {
      const files = res as any || [];
      this.files = files.map(x=> {
        return {
          name: x.name,
          path: this.base + x.path
        }
      });
    })
    // this.todolist2Service.download(this.kpiId, date).subscribe((data: any) => {
    //   const blob = new Blob([data],
    //     { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

    //   const downloadURL = window.URL.createObjectURL(data);
    //   const link = document.createElement('a');
    //   link.href = downloadURL;
    //   const ct = new Date();
    //   link.download = `${ct.getFullYear()}${ct.getMonth()}${ct.getDay()}_archive.zip`;
    //   link.click();
    // });
  }
  headerCellInfo(args) {
    if ( args.cell.column.field === 'CustomerID') {
     const toolcontent = args.cell.column.headerText;
     const tooltip: Tooltip = new Tooltip({
       content: toolcontent
   });
    tooltip.appendTo(args.node);
    }
 }
  loadDataModel2(id) {
    this.spinner.show()
    this.meetingService.getChartWithTime(id,this.datePipe.transform(this.currentTime, "YYYY-MM-dd HH:mm")).subscribe((res: any) => {
      this.ytds = res.ytds
      this.policyTitle = res.policy
      this.typeId = res.typeId,
      this.YTD = res.ytd
      this.targetYTD = res.targetYTD
      this.perfomance = res.perfomances
      this.targets = res.targets
      this.labels = res.labels

      this.dataTable = res.dataTable.filter(x => x.currentMonthData.length > 0)
      const dataTable = res.dataTable.filter(x => x.currentMonthData.length > 0)

      this.createChart(
        'planet-chart',
        this.labels,
        this.unitName
      )
      this.changeLocalHome.push(this.dataService.currentMessage.subscribe((res: any)=>{
        if(res === 0)
          return
        if(res.value > 0 || res.value !== undefined)
          this.dataHeight = []
          this.dataHeight.push(
            {
              value: res.value,
              actionId: res.actionId,
              month: res.month
            }
          )
          for (let item of dataTable) {
              for (let items in item.nextMonthData) {
                let i = Number(items);
                if(item.nextMonthData[i].actionId === res.actionId && item.nextMonthData[i].month === res.month) {
                  item.nextMonthData[i].heightA = res.value
                  break;
                }
              }
          }
      }))
      this.changeLocalHome.push(this.dataService.currentMessagesTarget.subscribe((res: any)=>{
        if(res === 0)
          return
        if(res.value > 0 || res.value !== undefined)
          this.dataHeight = []
          this.dataHeight.push(
            {
              value: res.value,
              actionId: res.actionId,
              month: res.month
            }
          )
          for (let item of dataTable) {
              for (let items in item.nextMonthData) {
                let i = Number(items);
                if(item.nextMonthData[i].actionId === res.actionId && item.nextMonthData[i].month === res.month) {
                  item.nextMonthData[i].heightT = res.value
                  break;
                }
              }
          }
      }))
      this.spinner.hide()

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

      //pic
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
      //end pic

      //factory
      // const factory = res.map((item: any) => {
      //   return {
      //     id: item.factId,
      //     name: item.factName
      //   }
      // })
      // this.dataFact = [...new Map(factory.map(items => [items[items.id], items])).values()];
      // this.dataFact = factory.filter((thing, i, arr) => {
      //   return arr.indexOf(arr.find(t => t.id === thing.id)) === i;
      // });
      // //end pic

      // //center
      // const center = res.map((item: any) => {
      //   return {
      //     id: item.centerId,
      //     name: item.centerName
      //   }
      // })
      // this.dataCenter = [...new Map(center.map(items => [items[items.id], items])).values()];
      // this.dataCenter = center.filter((thing, i, arr) => {
      //   return arr.indexOf(arr.find(t => t.id === thing.id)) === i;
      // });
      // //end center

      // //dept
      // const dept = res.map((item: any) => {
      //   return {
      //     id: item.deptId,
      //     name: item.deptName
      //   }
      // })
      // this.dataDept = [...new Map(dept.map(items => [items[items.id], items])).values()];
      // this.dataDept = dept.filter((thing, i, arr) => {
      //   return arr.indexOf(arr.find(t => t.id === thing.id)) === i;
      // });
      //end dept

      this.dataLevel.unshift({ name: "All", id: 0 });
      // this.dataDept.unshift({ name: "All", id: 0 });
      // this.dataCenter.unshift({ name: "All", id: 0 });
      // this.dataFact.unshift({ name: "All", id: 0 });
      // this.dataPic.unshift({ name: "All", id: 0 });
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
