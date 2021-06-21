
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "src/app/_core/_guards/auth.guard";

const routes: Routes = [
  {
    path: '',
    data: {
      title: '',
      breadcrumb: 'System'
    },
    children: [
      // {
      //   path: 'account',
      //   component: AccountComponent,
      //   data: {
      //     title: 'Account',
      //     breadcrumb: 'Account',
      //     functionCode: 'account'
      //   },
      //   // canActivate: [AuthGuard]
      // },

    ]
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportRoutingModule { }
