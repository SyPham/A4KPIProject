import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { HttpClient } from '@angular/common/http';
import { Account } from '../_model/account';
import { UtilitiesService } from './utilities.service';
import { OperationResult } from '../_model/operation.result';
import { catchError } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class Account2Service extends CURDService<Account> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Account", utilitiesService);
  }
  lock(id): Observable<OperationResult> {
    return this.http.put<OperationResult>(`${this.base}Account/lock?id=${id}`, {}).pipe(
      catchError(this.handleError)
    );
  }
}
