import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {environment} from '../../environment';
import {Observable, tap} from 'rxjs';
import { jwtDecode} from 'jwt-decode';

export interface IUser {
  email: string;
  password: string;
}

export interface IAuthResponse {
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiUrl = environment.apiUrl + '/Auth';
  tokenKey = environment.tokenKey;

  constructor(private http: HttpClient) { }

  register(email: string, password: string): Observable<IUser> {
    return this.http.post<IUser>(`${this.apiUrl}/register`, { email, password });
  }

  login(email: string, password: string): Observable<IAuthResponse> {
    return this.http.post<IAuthResponse>(`${this.apiUrl}/login`, { email, password })
      .pipe(tap(response => {
        localStorage.setItem(this.tokenKey, response.token);
      }));
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
  }

  getToken() {
    return localStorage.getItem(this.tokenKey);
  }

  isLoggedIn() {
    return !!this.getToken();
  }

  getEmail(): string | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token)
      return decoded.email
    } catch (e) {
      console.error('Invalid JWT: ', e);
      return null;
    }
  }
}
