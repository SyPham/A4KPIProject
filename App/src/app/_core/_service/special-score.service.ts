import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { SpecialScore } from '../_model/special-score';
import { UtilitiesService } from './utilities.service';
import { EnvService } from './env.service';
@Injectable({
  providedIn: 'root'
})
export class SpecialScoreService extends CURDService<SpecialScore> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService, env: EnvService)
  {
    super(http,"SpecialScore", utilitiesService , env);
  }

}
