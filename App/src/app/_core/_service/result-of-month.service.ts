import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { ResultOfMonth, ResultOfMonthRequest } from '../_model/result-of-month';
import { UtilitiesService } from './utilities.service';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { OperationResult } from '../_model/operation.result';
@Injectable({
  providedIn: 'root'
})
export class ResultOfMonthService extends CURDService<ResultOfMonth> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"ResultOfMonth", utilitiesService);
  }
  getAllByObjectiveId(objectiveId): Observable<ResultOfMonth[]> {
    return this.http
      .get<ResultOfMonth[]>(`${this.base}${this.entity}/GetAllByObjectiveId?objectiveId=${objectiveId}`, {})
      .pipe(catchError(this.handleError));
  }
  updateResultOfMonth(model: ResultOfMonthRequest): Observable<OperationResult> {
    return this.http
      .put<OperationResult>(`${this.base}${this.entity}/UpdateResultOfMonth`, model)
      .pipe(catchError(this.handleError));
  }
}
