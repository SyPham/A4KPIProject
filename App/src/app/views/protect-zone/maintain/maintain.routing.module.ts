import { RoleComponent } from './role/role.component';

import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { AuthGuard } from 'src/app/_core/_guards/auth.guard'
import { AccountGroupPeriodComponent } from '../system/account-group-period/account-group-period.component';
import { AccountGroupComponent } from '../system/account-group/account-group.component';
import { AccountComponent } from '../system/account/account.component';
import { KpiCreateComponent } from '../system/kpi-create/kpi-create.component';
import { KpiCreate2Component } from '../system/kpi-create2/kpi-create2.component';
import { Kpi2nd3rdComponent } from '../system/kpi2nd3rd/kpi2nd3rd.component';
import { OcUserComponent } from '../system/oc-user/oc-user.component';
import { OcComponent } from '../system/oc/oc.component';
import { PeriodTypeComponent } from '../system/period-type/period-type.component';
import { PolicyComponent } from '../system/policy/policy.component';
import { ProgressComponent } from '../system/progress/progress.component';
import { SettingMonthlyComponent } from '../system/setting-monthly/setting-monthly.component';
import { ViewKPIComponent } from '../system/viewKPI/viewKPI.component';
import { PrivilegeComponent } from './role/privilege/privilege.component';
import { ChangePasswordComponent } from '../transaction/change-password/change-password.component';



// import { PeriodComponent } from './period/period.component';
const routes: Routes = [
  {
    path: '',
    data: {
      title: '',
      breadcrumb: ''
    },
    children: [
      {
        path: 'account',
        component: AccountComponent,
        data: {
          title: 'Account',
          breadcrumb: 'Account',
          functionCode: 'Account'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'account-group',
        component: AccountGroupComponent,
        data: {
          title: 'Account Group',
          breadcrumb: 'Account Group',
          functionCode: 'Account Group'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'setting-monthly',
        component: SettingMonthlyComponent,
        data: {
          title: 'Setting Monthly',
          breadcrumb: 'Setting Monthly',
          functionCode: 'Monthly Setting'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'progress',
        component: ProgressComponent,
        data: {
          title: 'Progress',
          breadcrumb: 'Progress',
          functionCode: 'progress'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'period',
        component: PeriodTypeComponent,
        data: {
          title: 'Period',
          breadcrumb: 'Period',
          functionCode: 'period'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'account-group-period',
        component: AccountGroupPeriodComponent,
        data: {
          title: 'Account Group Period',
          breadcrumb: 'Account Group Period',
          functionCode: 'account-group-period'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'oc',
        component: OcComponent,
        data: {
          title: 'OC',
          breadcrumb: 'OC',
          functionCode: 'OC'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'oc-user',
        component: OcUserComponent,
        data: {
          title: 'OC User',
          breadcrumb: 'OC User',
          functionCode: 'oc-user'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'policy',
        component: PolicyComponent,
        data: {
          title: 'Policy',
          breadcrumb: 'Policy',
          functionCode: 'policy'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'kpi-create',
        component: KpiCreateComponent,
        data: {
          title: 'KPI Create',
          breadcrumb: 'KPI Create',
          functionCode: 'kpi-create'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'kpi-create2',
        component: KpiCreate2Component,
        data: {
          title: 'KPI Create',
          breadcrumb: 'KPI Create',
          functionCode: 'KPI Create'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'kpi-2nd3rd',
        component: Kpi2nd3rdComponent,
        data: {
          title: 'KPI 2nd & 3rd Create',
          functionCode: 'KPI 2nd & 3rd Create'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'view-kpi',
        component: ViewKPIComponent,
        data: {
          title: 'KPI View',
          functionCode: 'View KPI'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'role',
        data: {
          title: 'Role',
          functionCode: 'Role'
        },
        canActivate: [AuthGuard],
        children: [
          {
            path: '',
            component: RoleComponent
          },
          {
            path: 'privilege/:id',
            component: PrivilegeComponent,
            data: {
              title: 'Privilege',
              functionCode: 'Privilege'
            }
          }
        ]
      },
      {
        path: 'change-password',
        component: ChangePasswordComponent,
        canActivate: [AuthGuard],
        data: {
          title: 'Change Password',
          breadcrumb: 'Change Password',
          functionCode: 'Change-Password'
        },
        // canActivate: [AuthGuard]
      },

    ]
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MaintainRoutingModule { }
