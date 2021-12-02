import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from '@angular/router';
import {Observable} from 'rxjs';
import {AuthService} from "../../services/auth/auth.service";

@Injectable({
  providedIn: 'root'
})
export class OnlyAnonymousGuard implements CanActivate {


  constructor(private authService: AuthService, private router: Router) {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot) {

    if (this.authService.isAuthorized) {
      this.router.navigate(['/']);
      return false
    }

    return true;
  }

}
