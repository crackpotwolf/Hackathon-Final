import {Component, OnInit} from '@angular/core';
import {FormFieldBase} from "../dynamic-forms/entities/_field-base";
import {FormGroup} from "@angular/forms";
import {TypeFilter} from "./Data/type-filter";
import {CheckboxField} from "../dynamic-forms/entities/checkbox";
import {GroupCheckboxField} from "../dynamic-forms/entities/group-checkbox";
import {RangeField} from "../dynamic-forms/entities/range";
import {HttpClient} from "@angular/common/http";
import {ProjectsService} from "../../services/projects.service";
import {ProjectServiceEventData} from "../../services/data/ProjectServiceEventData";
import {ProjectServiceEventType} from "../../services/data/ProjectServiceEventType";
import {Project} from "../../data/Project";

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.sass']
})
export class FilterComponent implements OnInit {
  formFields: FormFieldBase<any>[] = [];
  form!: FormGroup;

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
    let filters = [
      {
        name: "Стадия готовности",
        key: 'order.stage',
        type: TypeFilter.checkbox,
        values: [{key: "1", value: "Значение 1"}, {key: "2", value: "Значение 2"}]
      },
      {
        name: "Cертификация",
        key: 'order.sertification',
        type: TypeFilter.checkbox,
        values: [{key: "3", value: "Значение 3"}, {key: "4", value: "Значение 4"}, {key: "5", value: "Вова лол"}]
      },
      {
        name: "Сколько человек в организации",
        key: 'order.peopleCount',
        type: TypeFilter.range,
        values: {min: 0, max: 100}
      },
      {
        name: "Сроки реализации",
        key: 'timing',
        type: TypeFilter.range,
        values: {min: 0, max: 100}
      },
    ];
    this.setFormField(filters);
  }

  /**
   * Инициализация полей фильтров
   * @param filters Конфигурация фильтров
   * @private
   */
  private setFormField(filters: any) {
    for (const filter of filters) {
      switch (filter.type) {
        case TypeFilter.checkbox:
          this.formFields.push(new GroupCheckboxField({label: filter.name, key: filter.key, options: filter.values}));
          break;
        case TypeFilter.range:
          this.formFields.push(new RangeField({
            label: filter.name,
            key: filter.key,
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
