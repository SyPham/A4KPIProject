import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Period } from '../_model/period';
import { UtilitiesService } from './utilities.service';
@Injectable({
  providedIn: 'root'
})
export class PeriodService extends CURDService<Period> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Period", utilitiesService);
  }

}
