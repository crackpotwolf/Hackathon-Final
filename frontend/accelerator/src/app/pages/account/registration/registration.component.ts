import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {WrapperComponent} from "../../../layouts/wrapper/wrapper.component";
import {MessageService} from "primeng/api";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.sass']
})
export class RegistrationComponent implements OnInit {
  form: FormGroup;

  /// Флаг ошибки авторизации
  isErrorAuthentication: boolean = true;
  response: any;
  showSuccessModal: boolean = false;


  constructor(private fb: FormBuilder,
              private messageService: MessageService,
              private http: HttpClient,
  ) {
    this.form = this.fb.group({
      email: ['dereguzov34@gmail.com', Validators.required],
      lastName: ['Dereguzov', Validators.required],
      firstName: ['Kirill', Validators.required],
    });
  }

  ngOnInit(): void {
  }

  formIsValid() {
    return this.form.valid;
  }

  onRegistration() {
    this.http.post('/api/authentication/v1/account/registration', this.form.getRawValue())
      .subscribe(resp => {
        this.showSuccessModal = true;
      }, err => {
        if (err.error?.indexOf('existEmail') != -1) {
          this.messageService.add({
            severity: 'error',
            summary: 'Email уже занят',
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
