import {Component, Input, OnInit, Output} from '@angular/core';
import {FormFieldBase} from "../entities/_field-base";
import {FormGroup} from "@angular/forms";
import {DynamicFormControlService} from "../dynamic-form-control-service.service";
import {EventEmitter} from '@angular/core';

@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.sass']
})
export class DynamicFormComponent implements OnInit {
  @Input() fields: FormFieldBase<string>[] = [];
  /**
   *
   */
  @Input() wholeWidth: boolean = false;

  form!: FormGroup;
  @Output() onFormChanged = new EventEmitter<FormGroup>();

  lastState: any;


  constructor(private formControlService: DynamicFormControlService) {

  }

  ngOnInit() {
    this.form = this.formControlService.toFormGroup(this.fields as FormFieldBase<string>[]);
    this.onFormChanged.emit(this.form);

    this.form.valueChanges.subscribe(event => this.onFormChange(event));
  }

  /**
   * Обработка события изменения полей формы
   * @param currentState Текущее состояние формы
   */
  onFormChange(currentState: any) {
    if (!this.lastState) {
      this.lastState = currentState;
      return;
    }

    let changes = this.changedValues(this.lastState, currentState);
    this.lastState = currentState;


    for (const key of Object.keys(changes)) {
      let handler = this.fields.find(p => p.key == key)?.onChange;
      if (handler) {
        // Обработка изменения элемента формы
        let causedChanges = handler(changes[key].newValue);

        // Вызвать порожденные изменения, если есть
        if (typeof (causedChanges) == 'object') {
          for (const causedKey of Object.keys(causedChanges)) {
            let newVal = causedChanges[causedKey];
            if (this.form.controls[causedKey].value != newVal) {
              this.form.controls[causedKey].patchValue(newVal);
            }
          }
        }
      }
    }
  }

  /**
   * Поиск измененнных значений
   * @param lastState Предыдущее состояние формы
   * @param currentState Текущее состояние формы
   */
  changedValues(lastState: any, currentState: any): { [index: string]: any } {
    // debugger;
    let findChanges: { [index: string]: any } = {};
    for (const key of Object.keys(lastState)) {
      if (lastState[key] != currentState[key]) {
        findChanges[key] = {
          "oldValue": lastState[key],
          "newValue": currentState[key]
        }
      }
    }
    return findChanges;
  }

  /**
   * Получение значение полей формы
   */
  public getFormJson() {
    return this.fields.length == 0 ? {} : this.form.getRawValue();
  }
}
