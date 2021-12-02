import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from '@angular/router';
import {Observable} from 'rxjs';
import {JwtHelperService} from "@auth0/angular-jwt";

@Injectable({
  providedIn: 'root'
})
export class OnlyRoleGuard implements CanActivate {
  pages: { [index: string]: any } = {
    '/users': ['Manager', 'Admin']
  };
  helper: JwtHelperService;


  constructor(private router: Router) {
    this.helper = new JwtHelperService();
  }


  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    let path = state.url.split('?')[0];
    //console.log(path);
    if (path in Object.keys(this.pages)) {
      for (const role of this.helper.decodeToken(localStorage.getItem('token') ?? undefined).Role || []) {
        if (this.pages[path].indexOf(role) != -1)
          return true;
      }
      this.router.navigate(['/employees']);
      return false;
    }
    return true;
  }

}
