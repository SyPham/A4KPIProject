import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { AttitudeScore } from '../_model/attitude-score';
import { UtilitiesService } from './utilities.service';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AttitudeScoreService extends CURDService<AttitudeScore> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"AttitudeScore", utilitiesService);
  }
  getFisrtByObjectiveIdAndScoreBy(objectiveId, scoreBy): Observable<AttitudeScore> {
    return this.http
      .get<AttitudeScore>(`${this.base}${this.entity}/GetFisrtByObjectiveIdAndScoreBy?objectiveId=${objectiveId}&scoreby=${scoreBy}`, {})
      .pipe(catchError(this.handleError));
  }
}
