import {EventEmitter, Injectable} from '@angular/core';
import {ProjectServiceEventType} from "./data/ProjectServiceEventType";
import {Project} from "../../app/data/Project";
import {MessageService} from "primeng/api";
import {HttpClient} from "@angular/common/http";
import {ProjectServiceEventData} from "./data/ProjectServiceEventData";

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {
  /**
   * Уведомления
   */
  onEvents = new EventEmitter<ProjectServiceEventData>();

  projects: Project[] = [];

  constructor(private http: HttpClient,
              private messageService: MessageService) {
    this.onEvents.subscribe(e => this.onEvent(e))
  }

  /**
   * Загрузка всех проектов
   */
  loadProjects() {
    this.http.get<Project[]>('/api/accelerator/v1/project/all')
      .subscribe(resp => {
        this.projects = resp;
        this.onEvents.emit(new ProjectServiceEventData({
          type: ProjectServiceEventType.LoadingComplete,
          data: this.projects
        }));
      }, err => {
        console.error(err);
        this.messageService.add({
          severity: 'error',
          summary: 'Не удалось получить список проектов',
        });
      });
  }


  /**
   * Обработка событий
   * @param e Событие
   * @private
   */
  private onEvent(e: ProjectServiceEventData) {
    switch (e.type) {
      case ProjectServiceEventType.RunninSearchByGlobal:
        this.searchProjectsByGlobalFilter(e.data);
        break;
    }
  }

  /**
   * Выполнить глобальный поиск (по всем, индексируемым, параметрам) проектов
   * @param inputText Текст глобального фильтра
   * @private
   */
  private searchProjectsByGlobalFilter(inputText: string) {
    this.http.get<string[]>(`/api/search/v1/search/guids/${inputText}`)
      .subscribe(resp => {
        let guids = Object.keys(resp);
        let _projects = this.projects.filter(p => guids.indexOf(p.guid) != -1).map(p => {
          p.relevance = resp[p.guid];
          return p;
        });
        let sorted = _projects.sort((a, b) => (a.relevance < b.relevance ? 1 : -1));
        this.onEvents.emit(new ProjectServiceEventData({
          type: ProjectServiceEventType.RunninSearchByGlobalDone,
          data: sorted
        }))
      }, err => {
        console.error(err);
        this.messageService.add({
          severity: 'error',
          summary: 'Не удалось выполнить поиск проектов',
        });
        this.onEvents.emit(new ProjectServiceEventData({
          type: ProjectServiceEventType.RunninSearchByGlobalError,
          data: err
        }));
      })
  }

}
