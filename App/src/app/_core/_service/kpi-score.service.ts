import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { KPIScore } from '../_model/kpi-score';
import { UtilitiesService } from './utilities.service';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { EnvService } from './env.service';
@Injectable({
  providedIn: 'root'
})
export class KPIScoreService extends CURDService<KPIScore> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService, env: EnvService)
  {
    super(http,"KPIScore", utilitiesService, env);
  }

  getFisrtSelfScoreByAccountId(accountId, periodTypeId, period, scoreType): Observable<KPIScore> {
    const apiUrl =`${this.env.apiUrl}${this.entity}/GetFisrtSelfScoreByAccountId?accountId=${accountId}&periodTypeId=${periodTypeId}&period=${period}&scoreType=${scoreType}`;
    return this.http.get<KPIScore>(apiUrl, {}).pipe(catchError(this.handleError));
  }
  getFisrtSelfScoreL1ByAccountId(accountId, periodTypeId, period, scoreType): Observable<KPIScore> {
    const apiUrl =`${this.env.apiUrl}${this.entity}/GetFisrtSelfScoreL1ByAccountId?accountId=${accountId}&periodTypeId=${periodTypeId}&period=${period}&scoreType=${scoreType}`;
    return this.http.get<KPIScore>(apiUrl, {}).pipe(catchError(this.handleError));
  }
  // Lấy điểm đã chấm cho L0
  getFisrtByAccountId(accountId, periodTypeId, period, scoreType): Observable<KPIScore> {
    const apiUrl =`${this.env.apiUrl}${this.entity}/GetFisrtByAccountId?accountId=${accountId}&periodTypeId=${periodTypeId}&period=${period}&scoreType=${scoreType}`;
    return this.http.get<KPIScore>(apiUrl, {}).pipe(catchError(this.handleError));
  }
    // Lấy điểm L1 đã chấm cho L0
  getFisrtKPIScoreL1ByAccountId(accountId, periodTypeId, period, scoreType): Observable<KPIScore> {
      const apiUrl =`${this.env.apiUrl}${this.entity}/GetFisrtKPIScoreL1ByAccountId?accountId=${accountId}&periodTypeId=${periodTypeId}&period=${period}&scoreType=${scoreType}`;
      return this.http.get<KPIScore>(apiUrl, {}).pipe(catchError(this.handleError));
  }
}
