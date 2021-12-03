import {Component, OnInit} from '@angular/core';
import {FormFieldBase} from "../dynamic-forms/entities/_field-base";
import {FormGroup} from "@angular/forms";
import {TypeFilter} from "./Data/type-filter";
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
    if (e.type == ProjectServiceEventType.LoadingComplete
      || e.type == ProjectServiceEventType.CurrentProjects) {
      this.loadingFilters(e.data);
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
    this.filters = [];
    this.formFields = [];
    setTimeout(() => {

      this.form = undefined;

      this.addTimingFilter(allProjects);
      this.addCheckboxGroupFilter(allProjects, 'order.stage', "Стадия готовности");
      this.addCheckboxGroupFilter(allProjects, 'order.sertification', "Cертификация");
      this.addCheckboxGroupFilter(allProjects, 'transportComplexOrganization', "Целевая организация");
      this.filters.push(
        {
          name: "Сколько человек в организации",
          key: 'order.peopleCount',
          type: TypeFilter.checkbox,
          values: [{key: "Менее 20", value: "Менее 20"},
            {key: "от 20 до 100", value: "От 20 до 100"},
            {key: "от 100 до 500", value: "От 100 до 500"},
            {key: "более 500", value: "Более 500"}]
        });
      this.addTagsFilter(allProjects);
      this.setFormField();
    }, 500)
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
      color: "#009A96",
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
    this.formFields = [];
    for (const filter of this.filters) {
      switch (filter.type) {
        case TypeFilter.checkbox:
          this.formFields.push(new GroupCheckboxField({
            label: filter.name,
            key: filter.key,
            options: filter.values,
            isExpansionPanel: true
          }));
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
   * Очистка фильтров
   */
  onClickClearFilters() {
    this.projectsService.onEvents.emit(new ProjectServiceEventData({type: ProjectServiceEventType.ClearFilters}));
    this.projectsService.onEvents.emit(new ProjectServiceEventData({type: ProjectServiceEventType.GetCurrentProjects}));
  }

  /**
   * Применение фильтров
   */
  onApplyFilters() {
    let rawValue = this.form?.getRawValue();

    for (const key of Object.keys(rawValue)) {
      if (rawValue[key]?.constructor.name == 'FormGroup') {
        rawValue[key] = rawValue[key].getRawValue();
      }
    }

    this.projectsService.onEvents.emit(new ProjectServiceEventData({
      type: ProjectServiceEventType.ApplyFilters,
      data: rawValue
    }));
    // console.log(rawValue);
  }


  /**
   * Добавление фильтра с тегами
   * @param allProjects
   * @private
   */
  private addTagsFilter(allProjects: Project[]) {
    // let tags = [...new Set(allProjects.map(p => p.tags).reduce((prev, curr) => [...prev, ...curr]))];
    let tags = [
      "Доступный и комфортный городской транспорт",
      "Новые виды мобильности",
      "Безопасность дорожного движения",
      "Здоровые улицы и экология",
      "Цифровые технологии в транспорте"
    ];
    if (tags.length) {
      this.filters.push({
        name: 'Технологическое направление',
        key: 'tags',
        type: TypeFilter.checkbox,
        values: tags.map(p => {
          return {key: p, value: p};
        })
      });
    }
  }
}
