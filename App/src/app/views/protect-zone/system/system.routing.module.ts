import { AccountGroupPeriodComponent } from './account-group-period/account-group-period.component';
import { AccountGroupComponent } from './account-group/account-group.component';
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "src/app/_core/_guards/auth.guard";
import { AccountComponent } from './account/account.component';
import { ProgressComponent } from './progress/progress.component';
import { PeriodComponent } from './period/period.component';

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'System',
      breadcrumb: 'System'
    },
    children: [
      {
        path: 'account',
        component: AccountComponent,
        data: {
          title: 'Account',
          breadcrumb: 'Account',
          functionCode: 'account'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'account-group',
        component: AccountGroupComponent,
        data: {
          title: 'Account Group',
          breadcrumb: 'Account Group',
          functionCode: 'account-group'
        },
        // canActivate: [AuthGuard]
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
        component: PeriodComponent,
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
      }
    ]
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemRoutingModule { }
