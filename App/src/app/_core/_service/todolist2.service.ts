import { EnvService } from './env.service';
import { environment } from 'src/environments/environment';
import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { UtilitiesService } from './utilities.service';
import { SelfScore, ToDoList, ToDoListByLevelL1L2Dto, ToDoListL1L2, ToDoListOfQuarter } from '../_model/todolistv2';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Objective } from '../_model/objective';
import { OperationResult } from '../_model/operation.result';
@Injectable({
  providedIn: 'root'
})
export class Todolist2Service  {
  messageSource = new BehaviorSubject<boolean>(null);
  messageUploadSource = new BehaviorSubject<boolean>(null);
  currentMessage = this.messageSource.asObservable();
  currentUploadMessage = this.messageUploadSource.asObservable();
  entity = 'Todolist2';
  base = environment.apiUrl;
  // có thể subcribe theo dõi thay đổi value của biến này thay cho messageSource
  constructor(private http: HttpClient, public env: EnvService, utilitiesService: UtilitiesService) {
  }
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  changeUploadMessage(message) {
    this.messageUploadSource.next(message);
  }

  deleteAc(id) {
    return this.http.delete(`${this.env.apiUrl}${this.entity}/deleteAction/${id}`);
  }
  l0(currentTime, userId): Observable<any[]> {
    return this.http
      .get<any[]>(`${this.env.apiUrl}${this.entity}/L0/${currentTime}/${userId}`, {})
      .pipe(catchError(this.handleError));
  }
  submitUpdatePDCA(model): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.env.apiUrl}${this.entity}/SubmitUpdatePDCA`, model);
  }
  saveUpdatePDCA(model): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.env.apiUrl}${this.entity}/SaveUpdatePDCA`, model);
  }
  submitAction(model): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.env.apiUrl}${this.entity}/submitAction`, model);
  }

  saveAction(model): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.env.apiUrl}${this.entity}/saveAction`, model);
  }
  submitKPINew(kpiId): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.env.apiUrl}${this.entity}/SubmitKPINew?kpiId=${kpiId}`, {});
  }
  addOrUpdateStatus(request): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.env.apiUrl}${this.entity}/AddOrUpdateStatus`, request);
  }
  getStatus(): Observable<any[]> {
    return this.http
      .get<any[]>(`${this.env.apiUrl}${this.entity}/getStatus`, {})
      .pipe(catchError(this.handleError));
  }
  getActionsForL0(kpiNewId, userId): Observable<any> {
    return this.http
      .get<any>(`${this.env.apiUrl}${this.entity}/GetActionsForL0/${kpiNewId}/${userId}`, {})
      .pipe(catchError(this.handleError));
  }
  getPDCAForL0(kpiNewId,currentTime, userId ): Observable<any> {
    return this.http
      .get<any>(`${this.env.apiUrl}${this.entity}/GetPDCAForL0/${kpiNewId}/${currentTime}/${userId}`, {})
      .pipe(catchError(this.handleError));
  }

  getKPIForUpdatePDC(kpiNewId,currentTime ): Observable<any> {
    return this.http
      .get<any>(`${this.env.apiUrl}${this.entity}/GetKPIForUpdatePDC?kpiNewId=${kpiNewId}&currentTime=${currentTime}`, {})
      .pipe(catchError(this.handleError));
  }

  getTargetForUpdatePDCA(kpiNewId,currentTime ): Observable<any> {
    return this.http
      .get<any>(`${this.env.apiUrl}${this.entity}/GetTargetForUpdatePDCA?kpiNewId=${kpiNewId}&currentTime=${currentTime}`, {})
      .pipe(catchError(this.handleError));
  }

  getActionsForUpdatePDCA(kpiNewId,currentTime, userid ): Observable<any> {
    return this.http
      .get<any>(`${this.env.apiUrl}${this.entity}/GetActionsForUpdatePDCA/${kpiNewId}/${currentTime}/${userid}`, {})
      .pipe(catchError(this.handleError));
  }
  download(kpiId,uploadTime ) {
    return this.http
      .get(`${this.env.apiUrl}UploadFile/download?kpiId=${kpiId}&uploadTime=${uploadTime}`, { responseType: 'blob' })
      .pipe(catchError(this.handleError));
  }
  getAttackFiles(kpiId,uploadTime ) {
    return this.http
      .get(`${this.env.apiUrl}UploadFile/GetAttackFiles?kpiId=${kpiId}&uploadTime=${uploadTime}`)
      .pipe(catchError(this.handleError));
  }
  getDownloadFiles(kpiId,uploadTime ) {
    return this.http
      .get(`${this.env.apiUrl}UploadFile/GetDownloadFiles?kpiId=${kpiId}&uploadTime=${uploadTime}`)
      .pipe(catchError(this.handleError));
  }
  getDownloadFilesMeeting(kpiId,uploadTime ) {
    return this.http
      .get(`${this.env.apiUrl}UploadFile/GetDownloadFilesMeeting?kpiId=${kpiId}&uploadTime=${uploadTime}`)
      .pipe(catchError(this.handleError));
  }
  protected handleError(errorResponse: any) {
    if (errorResponse?.error?.message) {
        return throwError(errorResponse?.error?.message || 'Server error');
    }

    if (errorResponse?.error?.errors) {
        let modelStateErrors = '';

        // for now just concatenate the error descriptions, alternative we could simply pass the entire error response upstream
        for (const errorMsg of errorResponse?.error?.errors) {
            modelStateErrors += errorMsg + '<br/>';
        }
        return throwError(modelStateErrors || 'Server error');
    }
    return throwError('Server error');
}
}
