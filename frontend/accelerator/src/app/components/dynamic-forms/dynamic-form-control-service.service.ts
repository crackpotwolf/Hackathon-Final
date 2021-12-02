import {Injectable} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {FormFieldBase} from "./entities/_field-base";


@Injectable()
export class DynamicFormControlService {
  constructor() {
  }

  /**
   * Преобразование в FormGroup
   * @param formFields Список полей формы
   */
  toFormGroup(formFields: FormFieldBase<string>[]) {
    const group: any = {};

    formFields?.forEach(question => {
      group[question.key] = question.required ? new FormControl(question.value || '', Validators.required)
        : new FormControl(question.value || '');
    });
    return new FormGroup(group);
  }
}
