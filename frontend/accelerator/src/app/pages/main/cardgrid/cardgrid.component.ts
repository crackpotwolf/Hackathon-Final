import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ProjectsService} from "../../../services/projects.service";
import {ProjectServiceEventData} from "../../../services/data/ProjectServiceEventData";
import {ProjectServiceEventType} from "../../../services/data/ProjectServiceEventType";

@Component({
  selector: 'app-cardgrid',
  templateUrl: './cardgrid.component.html',
  styleUrls: ['./cardgrid.component.sass']
})
export class CardgridComponent implements OnInit {
  public projects: any = []

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
        this.projects = e.data;
        break;
    }
  }
}
