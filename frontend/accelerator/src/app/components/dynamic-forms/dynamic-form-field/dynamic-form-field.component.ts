import {Component, Input, OnInit} from '@angular/core';
import {FormFieldBase} from "../entities/_field-base";
import {FormControl, FormGroup, FormGroupDirective, NgForm} from "@angular/forms";
import * as moment from 'moment';
import {ErrorStateMatcher} from "@angular/material/core";

@Component({
  selector: 'app-dynamic-form-field',
  templateUrl: './dynamic-form-field.component.html',
  styleUrls: ['./dynamic-form-field.component.sass']
})


/**
 * Динамический элемент формы
 */
export class DynamicFormFieldComponent implements OnInit {

  /**
   * Информация о элементе формы
   */
  @Input() field!: FormFieldBase<any>;

  // /**
  //  * Контроллер поля
  //  */
  // controll: FormControl;

  /**
   * Форма
   */
  @Input() form!: FormGroup;

  constructor() {
    // this.controll = new FormControl(moment(this.field?.value) );
  }

  ngOnInit(): void {
  }


  get isValid() {
    // let control = this.form.controls[this.field.key];
    // const isSubmitted = !!this.form;
    // let test = !(control && !control.valid && (control.dirty || control.touched || isSubmitted));
    // return test;
    return this.form.controls[this.field.key]?.valid ?? true;
  }

  /**
   * Получение текста ошибок
   */
  getErrorMessage() {
    // debugger;
    let messages: string[] = [];
    // if(this.field.required)
    if (!this.isValid)
      messages.push(`Поле "${this.field.label}" не может быть пустым.`);
    return messages.join('\n')
  }

}
