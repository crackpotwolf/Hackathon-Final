import {Component, Input, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {PrimeNGConfig, SelectItem} from 'primeng/api';
import {ProjectsService} from "../../../../services/projects/projects.service";
import {ProjectServiceEventData} from "../../../../services/projects/data/ProjectServiceEventData";
import {ProjectServiceEventType} from "../../../../services/projects/data/ProjectServiceEventType";
import {Project} from "../../../data/Project";
import * as moment from "moment";

@Component({
  selector: 'app-cardgrid',
  templateUrl: './cardgrid.component.html',
  styleUrls: ['./cardgrid.component.scss']
})
export class CardgridComponent implements OnInit {

  @Input() onlyFavorites: boolean = false;

  projects: Project[] = [];

  sortOptions: SelectItem[];

  sortOrder: number;

  sortField: string;

  // Флаг загрузки проектов
  projectIsLoading: boolean = false;
  // Найденные проекты с помощью поиска
  foundProjects?: Project[];
  filtresProjects?: Project[];

  constructor(private projectsService: ProjectsService, private http: HttpClient,
              private primengConfig: PrimeNGConfig) {

    this.projectsService.onEvents.subscribe((e) => this.onEventProjectSevice(e));
  }

  ngOnInit(): void {
    this.sortOptions = [
      {label: 'Price High to Low', value: '!price'},
      {label: 'Price Low to High', value: 'price'}
    ];

    this.primengConfig.ripple = true;
  }

  onSortChange(event) {
    let value = event.value;

    if (value.indexOf('!') === 0) {
      this.sortOrder = -1;
      this.sortField = value.substring(1, value.length);
    } else {
      this.sortOrder = 1;
      this.sortField = value;
    }
  }

  /**
   * Возвращает список отображаемых проектов
   */
  getShowProjects(): Project[] {
    if (this.filtresProjects != undefined) {
      return this.filtresProjects;
    } else if (this.foundProjects != undefined) {
      return this.foundProjects;
    }
    return this.projects;
  }

  /**
   * Обработка событий сервиса проектов
   * @param e
   * @private
   */
  private onEventProjectSevice(e: ProjectServiceEventData) {
    switch (e.type) {
      case ProjectServiceEventType.LoadingComplete:
        let favorites: any[] = JSON.parse(localStorage.getItem('favorites')) ?? [];
        this.projects = e.data;
        if (this.onlyFavorites) {
          this.projects = this.projects.filter(p => favorites.indexOf(p.guid) != -1);
        }
        break;
      case ProjectServiceEventType.RunninSearchByGlobal:
        this.projectIsLoading = true;
        break;
      case ProjectServiceEventType.RunninSearchByGlobalError:
        this.projectIsLoading = false;
        break;
      case ProjectServiceEventType.RunninSearchByGlobalDone:
        this.projectIsLoading = false;
        this.foundProjects = e.data;
        break;
      case ProjectServiceEventType.ClearSearch:
        this.foundProjects = undefined;
        break;
      case ProjectServiceEventType.ClearFilters:
        this.filtresProjects = undefined;
        this.projectsService.onEvents.emit(new ProjectServiceEventData({
          type: ProjectServiceEventType.CurrentProjects,
          data: this.getShowProjects()
        }));
        break;
      case ProjectServiceEventType.ApplyFilters:
        this.applyFilters(e.data);
        break;
    }
  }

  /**
   * Пименение фильтров
   * @param data Информация о фильтрах
   * @private
   */
  private applyFilters(data: any) {
    console.log(data);
    // Теги
    let projects = this.applyFilterTags(this.foundProjects ?? this.projects, data.tags);
    // Сроки реализации
    projects = this.applyFilterTiming(projects, data.timing);

    let otherFilters = ['order.stage', 'order.sertification', 'transportComplexOrganization', 'order.peopleCount'];
    for (const keyFilter of otherFilters) {
      projects = this.applyFilterByField(projects, keyFilter, data[keyFilter]);
    }

    console.log(projects);
    this.filtresProjects = projects;
  }

  /**
   * Получение фильтров со значением true
   * @param filters Фильтры
   */
  getFilterValues(filters: any) {
    let keys = Object.keys(filters);
    let res = [];
    for (const key of keys) {
      if (filters[key]) {
        res.push(key);
      }
    }
    return res;
  }

  /**
   * Применение фильтра по полю
   * @param projects Проекты
   * @param field Путь до поля
   * @param values Значения
   */
  applyFilterByField(projects: Project[], field: string, values: any) {
    let _values = this.getFilterValues(values);
    if (!_values.length) {
      return projects;
    }
    return projects.filter(p => _values.indexOf(this.getFieldByKey(p, field)) != -1);
  }

  /**
   * Получение значения поля по ключу
   * @param obj Объект
   * @param key Путь до поля
   */
  getFieldByKey(obj: any, key: string) {
    let spl = key.split('.');
    let value = obj[spl[0]];
    for (const _key of spl.slice(1)) {
      value = value[_key];
    }
    return value;
  }

  /**
   * Фильтр по тегам
   * @param projects Проекты
   * @param tags Теги
   */
  applyFilterTags(projects: Project[], tags: any) {
    let _tags = this.getFilterValues(tags);
    if (!_tags.length) {
      return projects;
    }
    return projects.filter(p => (p.tags.filter(x => _tags.includes(x))).length);
  }

  /**
   * Фильтр по срокам реализации
   * @param projects Проекты
   * @param timing Количество месяцев на реализацию
   * @private
   */
  private applyFilterTiming(projects: Project[], timing: number) {
    if (!timing) {
      return projects
    }
    return projects.filter(p => moment(p.timing, 'M-YYYY').diff(moment(), 'months') <= timing);
  }
}
