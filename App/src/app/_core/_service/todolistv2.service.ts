import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { UtilitiesService } from './utilities.service';
import { SelfScore, ToDoList, ToDoListByLevelL1L2Dto, ToDoListL1L2, ToDoListOfQuarter } from '../_model/todolistv2';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Objective } from '../_model/objective';
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
  l0(currentTime): Observable<any[]> {
    return this.http
      .get<any[]>(`${this.base}${this.entity}/L0?currentTime=${currentTime}`, {})
      .pipe(catchError(this.handleError));
  }
  l1(currentTime): Observable<any[]> {
    return this.http
      .get<any[]>(`${this.base}${this.entity}/L1?currentTime=${currentTime}`, {})
      .pipe(catchError(this.handleError));
  }
  l2(currentTime): Observable<any[]> {
    return this.http
      .get<any[]>(`${this.base}${this.entity}/L2?currentTime=${currentTime}`, {})
      .pipe(catchError(this.handleError));
  }
  fho(currentTime): Observable<any[]> {
    return this.http
      .get<any[]>(`${this.base}${this.entity}/fho?currentTime=${currentTime}`, {})
      .pipe(catchError(this.handleError));
  }
  ghr(currentTime): Observable<any[]> {
    return this.http
      .get<any[]>(`${this.base}${this.entity}/ghr?currentTime=${currentTime}`, {})
      .pipe(catchError(this.handleError));
  }
  gm(currentTime): Observable<any[]> {
    return this.http
      .get<any[]>(`${this.base}${this.entity}/gm?currentTime=${currentTime}`, {})
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
  /**
    * // TODO: Lấy dữ kiệu cho vai trò là L1, L2 khi click vào KPI Score Button
    */
   getAllKPIScoreByAccountId(): Observable<ToDoListByLevelL1L2Dto[]> {
    return this.http
      .get<ToDoListByLevelL1L2Dto[]>(`${this.base}${this.entity}/GetAllKPIScoreL0ByAccountId`, {})
      .pipe(catchError(this.handleError));
  }
  /**
    * // TODO: Lấy dữ kiệu cho vai trò là L1, L2 khi click vào KPI Score Button
    */
   getAllKPISelfScoreByObjectiveId(objectiveId): Observable<SelfScore[]> {
    return this.http
      .get<SelfScore[]>(`${this.base}${this.entity}/getAllKPISelfScoreByObjectiveId?objectiveId=${objectiveId}`, {})
      .pipe(catchError(this.handleError));
  }
  /**
    * // TODO: Lấy dữ kiệu cho vai trò là L1, L2 khi click vào KPI Score Button
    */
   getAllKPIScoreL1L2ByAccountId(accountId): Observable<[]> {
    return this.http
      .get<[]>(`${this.base}${this.entity}/GetAllKPIScoreL1L2ByAccountId?accountId=${accountId}`, {})
      .pipe(catchError(this.handleError));
  }
  getQuarterlySetting(): Observable<any[]> {
    return this.http
      .get<any[]>(`${this.base}${this.entity}/getQuarterlySetting`, {})
      .pipe(catchError(this.handleError));
  }
 /**
    * // TODO: Lấy dữ kiệu cho vai trò là L1, L2 khi click vào KPI Score Button
    */
  getAllAttitudeScoreL1L2ByAccountId(accountId): Observable<[]> {
    return this.http
      .get<[]>(`${this.base}${this.entity}/GetAllAttitudeScoreL1L2ByAccountId?accountId=${accountId}`, {})
      .pipe(catchError(this.handleError));
  }
  import(file,uploadBy) {
    const formData = new FormData();
    formData.append('UploadedFile', file);
    formData.append('UploadBy', uploadBy);
    return this.http.post(uploadBy + 'todolist/Import', formData);
  }
}
