import {Component, OnInit} from '@angular/core';
import {WrapperComponent} from "../../../layouts/wrapper/wrapper.component";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MessageService} from "primeng/api";
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-set-password',
  templateUrl: './set-password.component.html',
  styleUrls: ['./set-password.component.sass']
})
export class SetPasswordComponent implements OnInit {
  form: FormGroup;

  showSuccessModal: boolean = false;

  constructor(private fb: FormBuilder,
              private messageService: MessageService,
              private http: HttpClient,
              private router: Router,
              private activateRoute: ActivatedRoute,
  ) {
    let params = activateRoute.snapshot.queryParams;
    this.form = this.fb.group({
      userGuid: [params.UserGuid, Validators.required],
      securityStampEmail: [params.SecurityStampEmail, Validators.required],
      password: ['', [Validators.minLength(6), Validators.required]],
    });
  }

  ngOnInit(): void {
  }

  formIsValid() {
    return this.form.valid;
  }

  onSetPasswordClick() {
    this.http.post<any>('/api/authentication/v1/account/set-password', this.form.getRawValue())
      .subscribe(resp => {
        localStorage.setItem('token', resp.access_token);
        this.router.navigate(['']);
      }, err => {
        if ([504, 502].indexOf(err.status) == -1) {
          this.messageService.add({
            severity: 'error',
            summary: 'Ошибка установки пароля',
            detail: 'Произошла ошибка, но мы ещё научились обрабатывать её :(',
          });
        }
      })
  }

  notImplemented(event?: MouseEvent) {
    if (event) {
      event.preventDefault();
    }
    WrapperComponent.notImplemented(this.messageService)
  }
}
