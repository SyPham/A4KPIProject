import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';
import { Comment } from 'src/app/_core/_model/commentv2';
import { HttpClient } from '@angular/common/http';
import { UtilitiesService } from './utilities.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class Commentv2Service extends CURDService<Comment> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Comment", utilitiesService);
  }

  getFisrtByAccountId(accountId, periodTypeId, period, scoreType): Observable<Comment> {
    const apiUrl =`${this.base}${this.entity}/GetFisrtByAccountId?accountId=${accountId}&periodTypeId=${periodTypeId}&period=${period}&scoreType=${scoreType}`;
    return this.http.get<Comment>(apiUrl, {}).pipe(catchError(this.handleError));
  }
}
