import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { UtilitiesService } from './utilities.service';
import { ToDoList, ToDoListOfQuarter } from '../_model/todolistv2';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class Todolistv2Service extends CURDService<ToDoList> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Todolist", utilitiesService);
  }
  getAllByObjectiveId(objectiveId): Observable<ToDoList[]> {
    return this.http
      .get<ToDoList[]>(`${this.base}${this.entity}/GetAllByObjectiveId?objectiveId=${objectiveId}`, {})
      .pipe(catchError(this.handleError));
  }
  getAllInCurrentQuarterByObjectiveId(objectiveId): Observable<ToDoListOfQuarter[]> {
    return this.http
      .get<ToDoListOfQuarter[]>(`${this.base}${this.entity}/GetAllInCurrentQuarterByObjectiveId?objectiveId=${objectiveId}`, {})
      .pipe(catchError(this.handleError));
  }
}
