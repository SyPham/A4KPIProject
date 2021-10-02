import { EnvService } from './env.service';
import { Injectable } from '@angular/core';
import { PaginatedResult } from '../_model/pagination';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MeetingService {

  baseUrl = environment.apiUrl;
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(
    private http: HttpClient,
    public env: EnvService
    ) { }
  getOCs() {
    return this.http.get(`${this.env.apiUrl}Ocs/GetListTree`);
  }
  getAllKpi() {
    return this.http.get(`${this.env.apiUrl}Meeting/GetAllKPI`);
  }
  getChart(kpiId) {
    return this.http.get(`${this.env.apiUrl}Meeting/GetChart/${kpiId}`);
  }
  getChartWithTime(kpiId, dateTime) {
    return this.http.get(`${this.env.apiUrl}Meeting/GetChartWithDateTime/${kpiId}/${dateTime}`);
    // return this.http.get<any>(`${this.env.apiUrl}Meeting/GetChartWithDateTime?kpiId=${kpiId}?currentTime=${dateTime}`, {});
  }
  addPolicy(model) {
    return this.http.post(`${this.env.apiUrl}Ocpolicy/MappingPolicyOc`, model);
  }
  updatePolicy(model) {
    return this.http.post(`${this.env.apiUrl}Ocpolicy/RemovePolicyOC`, model);
  }
  deletePolicy(id) {
    return this.http.delete(`${this.env.apiUrl}Ocpolicy/DeletePolicy/${id}`);
  }
  getAllPolicy(){
    return this.http.get(`${this.env.apiUrl}Ocpolicy/GetAllPolicy`);
  }

}
