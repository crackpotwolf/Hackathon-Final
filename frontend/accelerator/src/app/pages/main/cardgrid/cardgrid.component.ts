import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {PrimeNGConfig, SelectItem} from 'primeng/api';
import {ProjectsService} from "../../../../services/projects/projects.service";
import {ProjectServiceEventData} from "../../../../services/projects/data/ProjectServiceEventData";
import {ProjectServiceEventType} from "../../../../services/projects/data/ProjectServiceEventType";
import {Project} from "../../../data/Project";

@Component({
  selector: 'app-cardgrid',
  templateUrl: './cardgrid.component.html',
  styleUrls: ['./cardgrid.component.scss']
})
export class CardgridComponent implements OnInit {

  projects: Project[] = [];

  sortOptions: SelectItem[];

  sortOrder: number;

  sortField: string;

  // Флаг загрузки проектов
  projectIsLoading: boolean = false;
  // Найденные проекты с помощью поиска
  foundProjects?: Project[];

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
    //TODO: приоритет должны имет отфильтрованные проекты
    if (this.foundProjects != undefined) {
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
        this.projects = e.data;
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
    }
  }
}
