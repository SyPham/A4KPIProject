import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent } from '@syncfusion/ej2-angular-grids';

@Component({
  selector: 'app-pdca',
  templateUrl: './pdca.component.html',
  styleUrls: ['./pdca.component.scss']
})
export class PdcaComponent implements OnInit {
  @Input() data:any;
  @ViewChild('grid') grid: GridComponent;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  toolbarOptions = ['Add', 'Delete','Search'];
  policy = '效率精進';
  kpi = 'SHC CTB IE 工時達成率';
  pic = '生產中心 Lai He';
  gridData = [{
    stt: 1,
    actionid: 1,
    content: 'content 1',
    target: 90,
    deadline: '9/13/2021',
    doContent: {
      content: 'Do 1',
      actionid: 1,
    },
    doAchievement: {
      achievement: 'Do 1',
      actionid: 1,
    },
    status: 'Processing'
  },
  {
    stt: 2,
    actionid: 2,
    content: 'content 2',
    target: 90,
    deadline: '9/13/2021',
    doContent: {
      content: 'Do 2',
      actionid: 2,
    },
    doAchievement: {
      achievement: 'Do 2',
      actionid: 2,
    },
    status: 'Processing'
  },
  {
    stt: 3,
    actionid: 3,
    content: 'content 3',
    target: 90,
    deadline: '9/13/2021',
    doContent: {
      content: 'Do 3',
      actionid: 3,
    },
    doAchievement: {
      achievement: 'Do 3',
      actionid: 3,
    },
    status: 'Processing'
  }]
  constructor(
    public activeModal: NgbActiveModal,
  ) { }

  ngOnInit() {
  }
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }
  alert(a) {
    console.log(a);
    a.doContent.content = 'Do 2321'
  }
}
