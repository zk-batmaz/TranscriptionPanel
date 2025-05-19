import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'http://localhost:5105/api/auth/login';

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<any> {
    return this.http.post(this.apiUrl, { username, password });
  }

  logout(): void {
    localStorage.removeItem('token');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  getRole(): string | null {
    const token = localStorage.getItem('token');
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      console.log('Token payload in getRole:', payload);
      
      // JWT'den role bilgisini farklı formatlarda aramayı dene
      // ClaimTypes.Role olarak gelen "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" anahtarı
      return payload['role'] || 
             payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || 
             null;
    } catch (e) {
      console.error('Token parse error in getRole:', e);
      return null;
    }
  }

  isAdmin(): boolean {
    const token = localStorage.getItem('token');
    if (!token) return false;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      console.log('Token payload in isAdmin:', payload);
      
      // .NET'in ClaimTypes.Role özelliğine karşılık gelen claim anahtarını kontrol et
      const role = payload['role'] || 
                  payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      
      return role === 'admin';
    } catch (e) {
      console.error('Token parse error in isAdmin:', e);
      return false;
    }
  }
  
  isEditor(): boolean {
    const token = localStorage.getItem('token');
    if (!token) return false;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const role = payload['role'] || 
                  payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      
      return role === 'editor';
    } catch (e) {
      console.error('Token parse error in isEditor:', e);
      return false;
    }
  }
}