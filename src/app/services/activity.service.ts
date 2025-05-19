import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ActivityLog } from '../models/activitylog.model';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {
  private apiUrl = 'http://localhost:5105/api/activity';

  constructor(private http: HttpClient) {}

  logActivity(log: ActivityLog): Observable<any> {
    return this.http.post(this.apiUrl, log);
  }

  getActivities(): Observable<ActivityLog[]> {
    return this.http.get<ActivityLog[]>(this.apiUrl);
  }
}
export type { ActivityLog };

