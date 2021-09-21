import { Injectable } from '@angular/core';
import { PaginatedResult } from '../_model/pagination';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MonthlySettingService {

  baseUrl = environment.apiUrl;
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(private http: HttpClient) { }

  getAll() {
    return this.http.get(`${this.baseUrl}SettingMonth/getall`);
  }
  add(model) {
    return this.http.post(`${this.baseUrl}SettingMonth/add`, model);
  }
  update(model) {
    return this.http.put(`${this.baseUrl}SettingMonth/update`, model);
  }
  delete(id) {
    return this.http.delete(`${this.baseUrl}SettingMonth/delete/${id}`);
  }

}
