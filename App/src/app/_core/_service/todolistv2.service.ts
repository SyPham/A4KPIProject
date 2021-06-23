import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { UtilitiesService } from './utilities.service';
import { ToDoList, ToDoListByLevelL1L2Dto, ToDoListL1L2, ToDoListOfQuarter } from '../_model/todolistv2';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class Todolistv2Service extends CURDService<ToDoList> {

  constructor(http: HttpClient, utilitiesService: UtilitiesService) {
    super(http, "Todolist", utilitiesService);
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
  /**
    * // TODO: Lấy dữ kiệu cho vai trò là L1, L2
    */
  getAllObjectiveByL1L2(): Observable<ToDoListL1L2[]> {
    return this.http
      .get<ToDoListL1L2[]>(`${this.base}${this.entity}/GetAllObjectiveByL1L2`, {})
      .pipe(catchError(this.handleError));
  }
  /**
    * // TODO: Lấy dữ kiệu cho vai trò là L1, L2 khi click vào KPI Score Button
    */
  getAllInCurrentQuarterByAccountGroup(accountId): Observable<ToDoListByLevelL1L2Dto[]> {
    return this.http
      .get<ToDoListByLevelL1L2Dto[]>(`${this.base}${this.entity}/GetAllInCurrentQuarterByAccountGroup?accountId=${accountId}`, {})
      .pipe(catchError(this.handleError));
  }
}
