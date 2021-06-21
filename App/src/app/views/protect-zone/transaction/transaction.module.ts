import { AttitudeScoreComponent } from './todolist/attitude-score/attitude-score.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { DateInputsModule } from '@progress/kendo-angular-dateinputs';
import { CheckBoxAllModule, SwitchModule } from '@syncfusion/ej2-angular-buttons';
import { DropDownListModule, MultiSelectModule } from '@syncfusion/ej2-angular-dropdowns';
import { GridAllModule } from '@syncfusion/ej2-angular-grids';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { loadCldr } from '@syncfusion/ej2-base';

import { HttpClient } from '@angular/common/http';
import { ObjectiveComponent } from './objective/objective.component';
import { TransactionRoutingModule } from './transaction.routing.module';
import { TodolistComponent } from './todolist/todolist.component';
import { TabModule, ToolbarModule } from '@syncfusion/ej2-angular-navigations';
import { ActionComponent } from './todolist/action/action.component';
import { UpdateResultComponent } from './todolist/update-result/update-result.component';
import { SelfScoreComponent } from './todolist/self-score/self-score.component';
import { KpiScoreComponent } from './todolist/kpi-score/kpi-score.component';



export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}
declare var require: any;
let defaultLang: string;
const lang = localStorage.getItem('lang');
loadCldr(
  require('cldr-data/supplemental/numberingSystems.json'),
  require('cldr-data/main/en/ca-gregorian.json'),
  require('cldr-data/main/en/numbers.json'),
  require('cldr-data/main/en/timeZoneNames.json'),
  require('cldr-data/supplemental/weekdata.json')); // To load the culture based first day of week

loadCldr(
  require('cldr-data/supplemental/numberingSystems.json'),
  require('cldr-data/main/vi/ca-gregorian.json'),
  require('cldr-data/main/vi/numbers.json'),
  require('cldr-data/main/vi/timeZoneNames.json'),
  require('cldr-data/supplemental/weekdata.json')); // To load the culture based first day of week
if (lang === 'vi') {
  defaultLang = lang;
} else {
  defaultLang = 'en';
}
@NgModule({
  declarations: [
    ObjectiveComponent,
    TodolistComponent,
    ActionComponent,
    UpdateResultComponent,
    SelfScoreComponent,
    KpiScoreComponent,
    AttitudeScoreComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    DropDownListModule,
    GridAllModule,
    CheckBoxAllModule,
    SwitchModule,
    TransactionRoutingModule,
    DateInputsModule ,
    MultiSelectModule,
    ToolbarModule,
    TabModule,
    TranslateModule.forChild({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      },
      defaultLanguage: defaultLang
    }),
  ]
})
export class TransactionModule { }
