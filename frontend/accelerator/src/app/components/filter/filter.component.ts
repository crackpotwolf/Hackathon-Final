import {Component, OnInit} from '@angular/core';
import {FormFieldBase} from "../dynamic-forms/entities/_field-base";
import {FormGroup} from "@angular/forms";
import {TypeFilter} from "./Data/type-filter";
import {CheckboxField} from "../dynamic-forms/entities/checkbox";
import {GroupCheckboxField} from "../dynamic-forms/entities/group-checkbox";
import {RangeField} from "../dynamic-forms/entities/slider";
import {HttpClient} from "@angular/common/http";
import {ProjectsService} from "../../../services/projects/projects.service";
import {ProjectServiceEventData} from "../../../services/projects/data/ProjectServiceEventData";
import {ProjectServiceEventType} from "../../../services/projects/data/ProjectServiceEventType";
import {Project} from "../../data/Project";
import * as moment from "moment";

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.sass']
})
export class FilterComponent implements OnInit {
  formFields: FormFieldBase<any>[] = [];
  form!: FormGroup;
  filters: any[] = [];

  constructor(private http: HttpClient,
              private projectsService: ProjectsService) {
    this.projectsService.onEvents.subscribe((e) => this.onEventProjectSevice(e));
  }

  ngOnInit(): void {
  }


  /**
   * Обработка событий сервиса проектов
   * @param e
   * @private
   */
  private onEventProjectSevice(e: ProjectServiceEventData) {
    switch (e.type) {
      case ProjectServiceEventType.LoadingComplete:
        this.loadingFilters(e.data);
        break;
    }
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
  private loadingFilters(allProjects: Project[]) {
    console.log(allProjects);
    //TODO: Сделать загрузку из БД

    this.addCheckboxGroupFilter(allProjects, 'order.stage', "Стадия готовности");
    this.addCheckboxGroupFilter(allProjects, 'order.sertification', "Cертификация");
    this.filters.push(
      {
        name: "Сколько человек в организации",
        key: 'order.peopleCount',
        type: TypeFilter.checkbox,
        values: [{key: "3", value: "Менее 20"},
          {key: "4", value: "от 20 до 100"},
          {key: "5", value: "от 100 до 500"},
          {key: "5", value: "более 500"}]
      });
    this.addTimingFilter(allProjects);
    this.setFormField();
  }


  /**
   * Получение всех уникальных значений по ключу
   * @param data Список данных
   * @param key Ключ
   */
  getUniqueValuesByField(data: any[], key: string): any[] {
    let spl = key.split('.');
    let _data = [...data];
    for (const _key of spl) {
      _data = _data.map(p => p[_key]);
    }
    return [...new Set(_data)];
  }

  /**
   * Добавление фильтра с группой чекбоксов по полю
   * @param allProjects Все проекты
   * @param fieldKey Поле
   * @param name Название фильтра
   */
  addCheckboxGroupFilter(allProjects: Project[], fieldKey: string, name: string) {
    let values = this.getUniqueValuesByField(allProjects, fieldKey);
    console.log(values);
    values = values.map(p => {
      return {key: p, value: p};
    });
    this.filters.push({
      name: name,
      key: fieldKey,
      type: TypeFilter.checkbox,
      values: values
    });
  }

  /**
   * Получение фильтра "Сроки реализации, месяцев"
   * @param allProjects Проекты
   */
  addTimingFilter(allProjects: Project[]) {
    let maxDate = Math.max.apply(Math, allProjects.map(p => moment(p.timing, 'M-YYYY')));
    let diffMonths = moment(maxDate).diff(moment(), 'months');
    if (diffMonths > 0) {
      this.filters.push({
        name: "Сроки реализации, месяцев",
        key: 'timing',
        type: TypeFilter.range,
        values: {min: 0, max: diffMonths}
      });
    }
  }


  /**
   * Инициализация полей фильтров
   * @param filters Конфигурация фильтров
   * @private
   */
  private setFormField() {
    for (const filter of this.filters) {
      switch (filter.type) {
        case TypeFilter.checkbox:
          this.formFields.push(new GroupCheckboxField({label: filter.name, key: filter.key, options: filter.values}));
          break;
        case TypeFilter.range:
          this.formFields.push(new RangeField({
            label: filter.name,
            key: filter.key,
            value: filter.value,
            min: filter.values.min,
            max: filter.values.max
          }));
          break;
      }
    }
    this.formFields = this.formFields.map(p => {
      p.wide = true;
      p.colon = true;
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
