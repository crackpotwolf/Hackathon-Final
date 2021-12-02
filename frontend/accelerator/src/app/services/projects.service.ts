import {EventEmitter, Injectable} from '@angular/core';
import {ProjectServiceEventType} from "./data/ProjectServiceEventType";
import {Project} from "../data/Project";
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
  }

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
}
