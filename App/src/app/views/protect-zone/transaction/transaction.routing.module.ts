import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ObjectiveComponent } from './objective/objective.component';
import { TodolistComponent } from "./todolist/todolist.component";
import { UploadKpiComponent } from "./todolist/upload-kpi/upload-kpi.component";

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Transaction',
      breadcrumb: 'Transaction'
    },
    children: [
      {
        path: 'objective',
        component: ObjectiveComponent,
        data: {
          title: 'Objective',
          breadcrumb: 'KPI Objective',
          functionCode: 'objective'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'todolist',
        component: TodolistComponent,
        data: {
          title: 'To Do List',
          breadcrumb: 'To Do List',
          functionCode: 'todolist'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'upload-kpi-objective',
        component: UploadKpiComponent,
        data: {
          title: 'Upload KPI Objective',
          breadcrumb: 'Upload KPI Objective',
          functionCode: 'todolist'
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
export class TransactionRoutingModule { }
