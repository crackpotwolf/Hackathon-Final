import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from "./pages/account/login/login.component";
import {OnlyAnonymousGuard} from "../guards/OnlyAnonymous/only-anonymous.guard";
import {MainComponent} from "./pages/main/main.component";
import {WrapperComponent} from "./layouts/wrapper/wrapper.component";
import {AuthGuard} from "../guards/Auth/auth-guard.service";
import {OnlyRoleGuard} from "../guards/only-role.guard";
import {RegistrationComponent} from "./pages/account/registration/registration.component";
import {SetPasswordComponent} from "./pages/account/set-password/set-password.component";
// import {AuthGuard} from "../guards/Auth/auth-guard.service";
// import {OnlyRoleGuard} from "../guards/only-role.guard";

const routes: Routes = [
  // {path: 'login', component: LoginComponent, canActivate: [OnlyAnonymousGuard]},
  // {path: 'logout', component: LoginComponent},
  // {path: 'registration', component: RegistrationComponent},
  // {path: 'set-password', component: SetPasswordComponent},
  {
    path: '',
    component: WrapperComponent,
    loadChildren: () => import('./pages/pages.module').then(m => m.PagesModule),
    // canActivate: [AuthGuard, OnlyRoleGuard]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
