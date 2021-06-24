import { filter } from 'rxjs/operators'
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core'
import { AlertifyService } from 'src/app/_core/_service/alertify.service'
import { AccountService } from 'src/app/_core/_service/account.service'
import { Account2Service } from 'src/app/_core/_service/account2.service'
import { BaseComponent } from 'src/app/_core/_component/base.component'
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap'
import { MessageConstants } from 'src/app/_core/_constants/system'

import { OcService } from './../../../../_core/_service/oc.service'

@Component({
  selector: 'app-oc-user',
  templateUrl: './oc-user.component.html',
  styleUrls: ['./oc-user.component.scss']
})
export class OcUserComponent extends BaseComponent implements OnInit {

  toolbarBuilding: object;
  toolbarUser: object;
  data: any;
  ocID: number;
  userData: any;
  buildingUserData: any;
  ocName: number
  OcUserData: any
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  toolbarOptions = ['Add', 'Delete', 'Cancel', 'Search'];
  accountIdList: any = [];
  accountData: Account[];
  fields: object = { text: 'fullName', value: 'id' };
  accountList: any = [];
  @ViewChild('addUserOc') public addUserOc: TemplateRef<any>;
  model = {
    UserID: 0,
    OCID: 0,
    OCname: null,
    AccountIdList: this.accountIdList
  };
  modalReference: NgbModalRef;
  constructor(
    private ocService: OcService,
    private alertify: AlertifyService,
    public modalService: NgbModal,
    private accountService: Account2Service,
  ) {super(); }

  ngOnInit() {
    this.toolbarUser = ['Search'];
  }

  actionBegin(args) {
    if (args.requestType === 'delete') {
      const obj = {
        userID: args.data[0].ID,
        OCID: this.ocID,
        OCname: this.ocName
      }
      this.removeBuildingUser(obj);
   }
  }
  openModal(item) {
    this.getAllUsers();
    this.modalReference = this.modalService.open(this.addUserOc, { size: 'xl' });
  }

  toolbarClick(args: any): void {
    switch (args.item.id) {
      case 'grid_1331832070_0_add':
        args.cancel = true;
        this.openModal(this.addUserOc);
        break;
      case 'exportExcel':
        break;
      default:
        break;
    }
  }

  create() {
    this.model = {
      UserID: 0,
      OCID: this.ocID,
      OCname: this.ocName,
      AccountIdList: this.accountIdList
    };
    this.ocService.mapRangeUserOC(this.model).subscribe((res: any) => {
      if (res.status) {
        this.alertify.success(MessageConstants.CREATED_OK_MSG);
        this.getUserByOcID(this.ocID);
        this.accountIdList = [];
        this.model = {
          UserID: 0,
          OCID: 0,
          OCname: null,
          AccountIdList: this.accountIdList
        };
        this.modalService.dismissAll();
      } else {
        this.alertify.warning(res.message);
      }
    })

  }

  removing(args) {
    const filteredItems = this.accountIdList.filter(item => item !== args.itemData.id);
    this.accountIdList = filteredItems;
    this.accountList = this.accountList.filter(item => item.id !== args.itemData.id);
  }

  onSelect(args) {
    const data = args.itemData;
    this.accountIdList.push(data.id);
    this.accountList.push({ objectiveId: 0 , id: data.id, username: data.username});
  }

  created() {
    this.getBuildingsAsTreeView();
  }

  createdUsers() {
    // this.getAllUsers();
  }

  rowSelected(args) {
    const data = args.data.entity || args.data;
    this.ocID = Number(data.id);
    this.ocName = data.name;
    if (args.isInteracted) {
      // this.getUserByOcName(this.ocName);
      this.getUserByOcID(this.ocID);
    }
  }

  getBuildingsAsTreeView() {
    this.ocService.getOCs().subscribe(res => {
      this.data = res;
    });
  }

  mappingUserWithBuilding(obj) {
    this.ocService.mapUserOC(obj).subscribe((res: any) => {
      if (res.status) {
        this.alertify.success(res.message);
        // this.getUserByOcName(this.ocName);
        this.getUserByOcID(this.ocID);
      } else {
        this.alertify.warning(res.message);
        // this.getUserByOcName(this.ocName);
        this.getUserByOcID(this.ocID);
      }
    });
  }

  removeBuildingUser(obj) {
    this.ocService.removeUserOC(obj).subscribe((res: any) => {
      if (res.status) {
        this.alertify.success(res.message);
        //this.getUserByOcName(this.ocName);
        this.getUserByOcID(this.ocID);
      } else {
        this.alertify.warning(res.message);
        //this.getUserByOcName(this.ocName);
        this.getUserByOcID(this.ocID);

      }
    });
  }

  getAllUsers() {
    this.accountService.getAll().subscribe((res: any) => {
      const data = res.map((i: any) => {
        return {
          ID: i.id,
          Username: i.fullName,
          Email: i.email,
          Status: this.checkStatus(i.id)
        };
      });
      this.userData = data.filter(x => x.Status);
      // this.userData = data;
      this.accountData = res ;
    });
  }

  getUserByOcName(ocName) {
    this.ocService.GetUserByOCname(ocName).subscribe(res => {
      this.OcUserData = res || [];
      this.getAllUsers();
    });
  }

  getUserByOcID(ocID) {
    this.ocService.GetUserByOcID(ocID).subscribe(res => {
      this.OcUserData = res || [];
      this.getAllUsers();
    });
  }

  checkStatus(userID) {
    this.OcUserData = this.OcUserData || [];
    const item = this.OcUserData.filter(i => {
      return i.userID === userID && i.ocid === this.ocID;
    });
    if (item.length <= 0) {
      return false;
    } else {
      return true;
    }

  }

  onChangeMap(args, data) {
    if (this.ocID > 0) {
      if (args.checked) {
        const obj = {
          userID: data.ID,
          OCID: this.ocID,
          OCname: this.ocName
        };
        this.mappingUserWithBuilding(obj);
      } else {
        const obj = {
          userID: data.ID,
          OCID: this.ocID,
          OCname: this.ocName
        }
        this.removeBuildingUser(obj);
      }
    } else {
      this.alertify.warning('Please select a building!');
    }
  }

}
