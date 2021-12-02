import {Injectable} from '@angular/core';
import {HttpClientModule, HttpClient} from "@angular/common/http";
import * as moment from "moment";
import {Router} from "@angular/router";
import {catchError, tap} from "rxjs/operators";
import {JwtHelperService} from "@auth0/angular-jwt";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  helper = new JwtHelperService();

  constructor(private http: HttpClient, private router: Router) {
  }

  /// Авторизация в системе
  login(email: string, password: string) {
    return this.http.post<any>('/api/authentication/v1/account/token', {email, password});
  }

  /// Выход из системы
  logout() {
    localStorage.removeItem('token');
    location.reload();
  }

  /// Авторизован ли пользователь
  public get isAuthorized(): boolean {
    return localStorage.getItem('token') !== null ?? !this.isTokenExpired;
  }

  /// Токен пользователя
  private get _token() : string | undefined {
    return localStorage.getItem('token') ?? undefined;
  }

  /// Просрочен ли токен
  public get isTokenExpired(): boolean {
    return this.helper.isTokenExpired(this._token);
  }

}
