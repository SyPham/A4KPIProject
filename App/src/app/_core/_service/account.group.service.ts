import { AccountGroup } from './../_model/account.group';
import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { UtilitiesService } from './utilities.service';
@Injectable({
  providedIn: 'root'
})
export class AccountGroupService extends CURDService<AccountGroup> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"AccountGroup", utilitiesService);
  }

}
