import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';
import { Contribution } from 'src/app/_core/_model/contribution';
import { HttpClient } from '@angular/common/http';
import { UtilitiesService } from './utilities.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class ContributionService extends CURDService<Contribution> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Contribution", utilitiesService);
  }
  getFisrtByObjectiveId(objectiveId, createdBy): Observable<Contribution> {
    return this.http
      .get<Contribution>(`${this.base}${this.entity}/GetFisrtByObjectiveId?objectiveId=${objectiveId}&createdBy=${createdBy}`, {})
      .pipe(catchError(this.handleError));
  }
}
