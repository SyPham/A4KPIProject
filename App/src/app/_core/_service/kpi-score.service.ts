import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { KPIScore } from '../_model/kpi-score';
import { UtilitiesService } from './utilities.service';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class KPIScoreService extends CURDService<KPIScore> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"KPIScore", utilitiesService);
  }
  getFisrtByAccountId(scoreBy): Observable<KPIScore> {
    return this.http
      .get<KPIScore>(`${this.base}${this.entity}/GetFisrtByAccountId?scoreby=${scoreBy}`, {})
      .pipe(catchError(this.handleError));
  }
}
