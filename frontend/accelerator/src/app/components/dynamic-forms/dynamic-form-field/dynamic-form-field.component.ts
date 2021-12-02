import {Component, Input, OnInit} from '@angular/core';
import {FormFieldBase} from "../entities/_field-base";
import {FormControl, FormGroup} from "@angular/forms";
import * as moment from 'moment';

@Component({
  selector: 'app-dynamic-form-field',
  templateUrl: './dynamic-form-field.component.html',
  styleUrls: ['./dynamic-form-field.component.sass']
})

/**
 * Динамический элемент формы
 */
export class DynamicFormFieldComponent implements OnInit{

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
      messages.push(`"${this.field.label}" не может быть пустым.`);
    return messages.join('\n')
  }

}
