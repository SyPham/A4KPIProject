import { filter } from 'rxjs/operators';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { EditService, ToolbarService, PageService, GridComponent } from '@syncfusion/ej2-angular-grids';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { Account2Service } from 'src/app/_core/_service/account2.service';
import { Account } from 'src/app/_core/_model/account';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { AccountGroupService } from 'src/app/_core/_service/account.group.service';
import { AccountGroup } from 'src/app/_core/_model/account.group';
import { OcService } from 'src/app/_core/_service/oc.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
  providers: [ToolbarService, EditService, PageService]
})
export class AccountComponent extends BaseComponent implements OnInit {
  data: Account[] = [];
  password = '';
  modalReference: NgbModalRef;
  fields: object = { text: 'name', value: 'id' };
  leaderFields: object = { text: 'fullName', value: 'id' };
  managerFields: object = { text: 'fullName', value: 'id' };
  ocFields: object = { text: 'name', value: 'id' };
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
  factId: number;
  centerId: number;
  deptId: number;
  accounts: any[];
  dataOclv3: any;
  dataOclv4: any;
  dataOclv5: any;
  constructor(
    private service: Account2Service,
    private accountGroupService: AccountGroupService,
    public modalService: NgbModal,
    private ocService: OcService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
  ) { super(); }

  ngOnInit() {
    // this.Permission(this.route);
    this.loadData();
    this.getAccounts();
    this.getAllOc();
    this.loadAccountGroupData();
  }
  getAllOc(){
    this.ocService.getAll().subscribe((res: any) => {
      //Oclv3
      this.dataOclv3 = res.filter(x => x.level === 3)
      this.dataOclv3.unshift({ id: 0, name: 'N/A'  });
      //end Oclv3

      //Oclv4
      this.dataOclv4 = res.filter(x => x.level === 4)
      this.dataOclv4.unshift({ id: 0, name: 'N/A'  });
      //end Oclv4

      //Oclv5
      this.dataOclv5 = res.filter(x => x.level === 5)
      this.dataOclv5.unshift({ id: 0, name: 'N/A'  });

      //end Oclv5

    })
  }
  loadData() {
    this.service.getAll().subscribe(data => {
      console.log(data);
      this.data = data;
    });
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
    this.leaderId = 0;
    this.managerId = 0;
    this.factId = 0;
    this.centerId = 0;
    this.deptId = 0;
    this.accountCreate = {
      id: 0,
      username: null,
      password: null,
      fullName: null,
      email: null,
      accountTypeId: 2,
      isLock: false,
      createdBy: 0,
      createdTime: new Date().toLocaleDateString(),
      modifiedBy: 0,
      modifiedTime: null,
      accountGroupIds: null,
      accountGroupText: null,
      accountType: null,
      factId: this.factId,
      centerId: this.centerId,
      deptId: this.deptId,
      leader: this.leaderId,
      manager: this.managerId,
      leaderName: null,
      managerName: null,
    };

  }
  updateModel(data) {
    this.accountGroupItem = data.accountGroupIds;
    this.managerId = data.manager;
    this.leaderId = data.leader;
    this.factId = data.factId;
    this.centerId = data.centerId;
    this.deptId = data.deptId;
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
      this.accountCreate = {
        id: 0,
        username: args.data.username ,
        password: args.data.password,
        fullName: args.data.fullName,
        email: args.data.email,
        accountTypeId: 2,
        isLock: false,
        createdBy: 0,
        createdTime: new Date().toLocaleDateString(),
        modifiedBy: 0,
        modifiedTime: null,
        accountType: null,
        deptId: this.deptId,
        centerId: this.centerId,
        factId: this.factId,
        accountGroupIds: this.accountGroupItem,
        accountGroupText: null,
        leader: this.leaderId,
        manager: this.managerId,
        leaderName: null,
        managerName: null,
      };

      if (args.data.username === undefined) {
        this.alertify.error('Please key in a account! <br> Vui lòng nhập tài khoản đăng nhập!');
        args.cancel = true;
        return;
      }
      if (args.data.password === undefined) {
        this.alertify.error('Please key in a password! <br> Vui lòng nhập mật khẩu!');
        args.cancel = true;
        return;
      }
      this.create();
    }
    if (args.requestType === 'save' && args.action === 'edit') {
      this.accountUpdate = {
        id: args.data.id,
        username: args.data.username ,
        password: args.data.password,
        fullName: args.data.fullName,
        email: args.data.email,
        isLock: args.data.isLock,
        accountTypeId: args.data.accountTypeId,
        createdBy: args.data.createdBy,
        createdTime: args.data.createdTime,
        modifiedBy:args.data.modifiedBy,
        modifiedTime: args.data.modifiedTime,
        accountType: null,
        factId: this.factId,
        centerId: this.centerId,
        deptId: this.deptId,
        accountGroupIds: this.accountGroupItem,
        accountGroupText: null,
        leader: this.leaderId,
        manager: this.managerId,
        leaderName: null,
        managerName: null,
      };
      this.update();
    }
    if (args.requestType === 'delete') {
      this.delete(args.data[0].id);
    }
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
      args.form.elements.namedItem('username').focus(); // Set focus to the Target element
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
    this.service.add(this.accountCreate).subscribe(
      (res) => {
        if (res.success === true) {
          this.alertify.success(MessageConstants.CREATED_OK_MSG);
          this.loadData();
          this.getAccounts();

          this.accountCreate = {} as Account;
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
    this.service.update(this.accountUpdate).subscribe(
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
  // end api
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

}
