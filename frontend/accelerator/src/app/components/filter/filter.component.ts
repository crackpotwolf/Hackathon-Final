import {Component, OnInit} from '@angular/core';
import {FormFieldBase} from "../dynamic-forms/entities/_field-base";
import {FormGroup} from "@angular/forms";
import {TypeFilter} from "./Data/type-filter";
import {CheckboxField} from "../dynamic-forms/entities/checkbox";
import {GroupCheckboxField} from "../dynamic-forms/entities/group-checkbox";
import {RangeField} from "../dynamic-forms/entities/range";

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.sass']
})
export class FilterComponent implements OnInit {
  formFields: FormFieldBase<any>[] = [];
  form!: FormGroup;

  constructor() {
    this.loadingFilters();
    console.log(this.formFields);
  }

  ngOnInit(): void {
  }

  /**
   * Событие - изменение значений формы
   * @param form Значнеие формы - FormGroup
   * @param name Yазвание формы
   */
  onFormChanged(form: FormGroup, name: string) {
    this.form = form;
    console.log(form.getRawValue());
  }

  /**
   * Загрузка фильтров
   * @private
   */
  private loadingFilters() {
    //TODO: Сделать загрузку из БД
    let filters = {
      "Фильтр 1": {
        type: TypeFilter.checkbox,
        values: [{key: "1", value: "Значение 1"}, {key: "2", value: "Значение 2"}]
      },
      "Фильтр 2": {
        type: TypeFilter.checkbox,
        values: [{key: "3", value: "Значение 3"}, {key: "4", value: "Значение 4"}, {key: "5", value: "Вова лол"}]
      },
      "Фильтр для Вовы": {
        type: TypeFilter.range,
        values: {min: 0, max: 100}
      },
    };
    this.setFormField(filters);
  }

  /**
   * Инициализация полей фильтров
   * @param filters Конфигурация фильтров
   * @private
   */
  private setFormField(filters: any) {
    for (const filterName of Object.keys(filters)) {
      let filter = filters[filterName];
      switch (filter.type) {
        case TypeFilter.checkbox:
          this.formFields.push(new GroupCheckboxField({label: filterName, key: filterName, options: filter.values}));
          break;
        case TypeFilter.range:
          this.formFields.push(new RangeField({
            label: filterName,
            key: filterName,
            value: 58,
            min: filter.values.min,
            max: filter.values.max
          }));
          break;
      }
    }
    this.formFields = this.formFields.map(p => {
      p.wide = true;
      return p;
    });
  }

  /**
   * Вывод значений фильтров
   */
  onClick() {
    let rawValue = this.form?.getRawValue();
    console.log('\n\n\n')
    console.log(rawValue)

    for (const key of Object.keys(rawValue)) {
      if (rawValue[key]?.constructor.name == 'FormGroup') {
        rawValue[key] = rawValue[key].getRawValue();
      }
    }
    console.log(rawValue);
  }
}
