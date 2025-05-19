import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

export interface User {
  id: number;
  username: string;
  role: string;
}

export interface CreateUserRequest {
  username: string;
  password: string;
  role: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'http://localhost:5105/api/users';

  constructor(private http: HttpClient) {}

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl).pipe(
      catchError(this.handleError)
    );
  }

  addUser(user: CreateUserRequest): Observable<User> {
    return this.http.post<User>(this.apiUrl, user).pipe(
      catchError(this.handleError)
    );
  }

  deleteUser(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Bilinmeyen bir hata oluştu';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side hata
      errorMessage = `Hata: ${error.error.message}`;
    } else {
      // Server-side hata
      if (error.error && error.error.message) {
        errorMessage = error.error.message;
      } else if (error.status === 401) {
        errorMessage = 'Bu işlem için yetkiniz yok';
      } else if (error.status === 403) {
        errorMessage = 'Bu işlemi gerçekleştirmek için admin yetkisi gerekli';
      } else if (error.status === 404) {
        errorMessage = 'İstenen kaynak bulunamadı';
      } else {
        errorMessage = `Sunucu hatası: ${error.status} - ${error.message}`;
      }
    }
    
    console.error('UserService Error:', error);
    return throwError(() => new Error(errorMessage));
  }
}