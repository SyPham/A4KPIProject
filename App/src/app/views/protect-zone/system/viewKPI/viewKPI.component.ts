import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core'
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap'
import { TreeGridComponent } from '@syncfusion/ej2-angular-treegrid'
import { ModalDirective } from 'ngx-bootstrap/modal'
import { MessageConstants } from 'src/app/_core/_constants/system'
import { Account2Service } from 'src/app/_core/_service/account2.service'
import { AlertifyService } from 'src/app/_core/_service/alertify.service'
import { KpinewService } from 'src/app/_core/_service/kpinew.service'
import { OcPolicyService } from 'src/app/_core/_service/OcPolicy.service'

import { OcService } from './../../../../_core/_service/oc.service'

@Component({
  selector: 'app-viewKPI',
  templateUrl: './viewKPI.component.html',
  styleUrls: ['./viewKPI.component.scss']
})
export class ViewKPIComponent implements OnInit {

  @ViewChild('content', { static: true }) content: TemplateRef<any>;
  toolbar: object;
  data: any;
  editing: any;
  contextMenuItems: any;
  pageSettings: any;
  editparams: { params: { format: string } };
  @ViewChild('childModal', { static: false }) childModal: ModalDirective;
  @ViewChild("treegrid")
  public treeGridObj: TreeGridComponent;
  @ViewChild("buildingModal")
  buildingModal: any;
  oc: { id: 0; name: ""; level: 1; parentID: null };
  edit: {
    Id: 0;
    name: "";
    PolicyId: 0,
    TypeId: 0,
    Pic: 0
  };
  title: string;
  picFields: object = { text: 'fullName', value: 'id' };
  typeFields: object = { text: 'name', value: 'id' };
  policyFields: object = { text: 'name', value: 'id' };
  policyData: Object;
  policyId: number = 0;
  picId: number = 0;
  typeId: number = 0;
  parentId: null
  level: number = 1
  typeData: Object;
  accountData: Account[];
  modalReference: NgbModalRef
  kpiname: any
  constructor(
    private ocService: OcService,
    private modalServices: NgbModal,
    private ocPolicyService: OcPolicyService,
    private accountService: Account2Service,
    private kpiNewService: KpinewService,
    private alertify: AlertifyService,

  ) {}

  ngOnInit() {
    this.editing = { allowDeleting: false, allowEditing: false, mode: "Row" };
    this.toolbar = ["Delete", "Search", "Update", "Cancel"];
    this.optionTreeGrid();
    this.onService();
    // this.getAllPolicy()
    this.getAllUsers();
    this.getAllType()
  }
  getAllType() {
    const lang = localStorage.getItem('lang');
    this.kpiNewService.getAllType(lang).subscribe(res => {
      this.typeData = res
    })
  }
  getAllUsers() {
    this.accountService.getAll().subscribe((res: any) => {
      this.accountData = res ;
    });
  }
  getAllPolicy() {
    this.ocPolicyService.getAllPolicy().subscribe(res => {
      this.policyData = res
    })
  }
  validation() {
    if (this.kpiname === null) {
      this.alertify.error('Please key in kpi name! <br> Vui lòng nhập KPI!');
      return false;
    }
    if (this.policyId === 0) {
      this.alertify.error('Please select Policy !');
      return false;
    }
    if (this.typeId === 0) {
      this.alertify.error('Please select a Type! ');
      return false;
    }
    if (this.picId === 0) {
      this.alertify.error('Please select a PIC! ');
      return false;
    }
    return true;

  }
  refreshData() {
    this.kpiname = null
    this.parentId = null
    this.level  = 1
    this.policyId = 0
    this.picId = 0
    this.typeId = 0
  }
  createOC() {
    const model = {
      Name: this.kpiname,
      PolicyId: this.policyId,
      TypeId: this.typeId,
      ParentId: this.parentId,
      Level: this.level,
      Pic: this.picId
    }
    if (this.validation()) {
      this.kpiNewService.add(model).subscribe(res => {
        if(res) {
          this.alertify.success(MessageConstants.CREATED_OK_MSG);
          this.modalReference.close();
          this.getBuildingsAsTreeView()
          this.refreshData()
        }else {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        }
      })
    }
  }

  optionTreeGrid() {
    this.contextMenuItems = [

    ];
    this.toolbar = [
      "Search",
      "ExpandAll",
      "CollapseAll"
    ];
    this.editing = {
      allowEditing: true,
      allowAdding: true,
      allowDeleting: true,
      mode: "Row",
    };
    this.pageSettings = { pageSize: 20 };
    this.editparams = { params: { format: "n" } };
  }

  created() {
    this.getBuildingsAsTreeView();
  }

  onService() {
    this.ocService.currentMessage.subscribe((arg) => {
      if (arg === 200) {
        this.getBuildingsAsTreeView();
      }
    });
  }

  toolbarClick(args) {
    switch (args.item.id) {
      case "treegrid_gridcontrol_add":
        args.cancel = true;
        this.openMainModal();
        break;
      default:
        break;
    }
  }

  contextMenuClick(args) {
    switch (args.item.id) {
      case "DeleteOC":
        this.delete(args.rowInfo.rowData.entity);
        break;
      case "Add-Sub-Item":
        this.openSubModal();
        break;
      default:
        break;
    }
  }

  delete(data) {
    this.alertify.confirm(
      "Delete KPI",
      'Are you sure you want to delete this KPI "' + data.name + '" ?',
      () => {
        this.kpiNewService.delete(data.id).subscribe(res => {
          if(res) {
            this.alertify.success(MessageConstants.CREATED_OK_MSG);
            this.getBuildingsAsTreeView()
            this.refreshData();
          }else {
            this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
          }
        })
      }
    );
  }

  updateModel(data) {
    this.policyId = data.policyId
    this.typeId = data.typeId
    this.picId = data.pic
  }
  actionComplete(args) {

    if (args.requestType === 'beginEdit') {
      const item = args.rowData.entity;
      this.updateModel(item);
    }
    if (args.requestType === 'save' && args.action === 'edit') {
      const model = {
        Id: args.data.entity.id,
        Name: args.data.entity.name,
        PolicyId: this.policyId,
        TypeId: this.typeId,
        Level: args.data.entity.level,
        ParentId: args.data.entity.parentId,
        Pic: this.picId
      }
      this.update(model);
    }

  }
  update(model) {

    this.kpiNewService.update(model).subscribe(res => {
      if(res) {
        this.alertify.success(MessageConstants.CREATED_OK_MSG);
        this.getBuildingsAsTreeView()
        this.refreshData()
      }else {
        this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
      }
    })

  }
  rowSelected(args) {

    this.parentId = args.data.entity.id
    this.level = args.data.entity.level + 1

  }

  getBuildingsAsTreeView() {

    const lang = localStorage.getItem('lang');
    this.kpiNewService.getTree(lang).subscribe((res) => {
      this.data = res;
    });

  }

  clearFrom() {
    this.oc = {
      id: 0,
      name: "",
      parentID: null,
      level: 1,
    };
  }

  rename() {
    this.ocService.rename(this.edit).subscribe((res) => {
      this.getBuildingsAsTreeView();
      this.alertify.success("The oc has been changed!!!");
    });
  }
  openMainModal() {
    this.modalReference = this.modalServices.open(this.content, { size: "lg"});
    this.title = "Add KPI";

  }
  openSubModal() {
    // this.getAllPolicy();
    this.modalReference = this.modalServices.open(this.content, {
      size: "lg",
    });
    this.title = "Add Sub KPI";
  }

}
