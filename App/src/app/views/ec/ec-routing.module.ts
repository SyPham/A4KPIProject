import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/_core/_guards/auth.guard';
const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'setting',
        loadChildren: () =>
          import('./setting/setting.module').then(m => m.SettingModule),
          canActivate: [AuthGuard]
      },
      {
        path: 'establish',
        loadChildren: () =>
          import('./establish/establish.module').then(m => m.EstablishModule),
          canActivate: [AuthGuard]
      },
      // execution
      {
        path: 'execution',
        loadChildren: () =>
          import('./execution/execution.module').then(m => m.ExecutionModule),
        canActivate: [AuthGuard]
      },
      // end execution
      {
        path: 'system',
        loadChildren: () =>
          import('./system/system.module').then(m => m.SystemModule),
          canActivate: [AuthGuard]
      },
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ECRoutingModule { }
