import {Component, OnInit} from '@angular/core';
import {ProjectsService} from "../../../../services/projects/projects.service";
import {ProjectServiceEventData} from "../../../../services/projects/data/ProjectServiceEventData";
import {ProjectServiceEventType} from "../../../../services/projects/data/ProjectServiceEventType";

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.sass']
})
export class SearchComponent implements OnInit {
  textGlobalSearch: string = '';

  constructor(private projectsService: ProjectsService) {
  }

  ngOnInit(): void {
  }

  /**
   * Открытие фильтров
   * @param $event
   */
  onClickOpenFilters(event: MouseEvent) {

  }


  timeoutTimer: any;
  isVisibleFilters: boolean = false;

  /**
   * Событие изменения текста фильтра
   * @param event Событие
   */
  onInputFilter(event: Event) {
    if (!this.textGlobalSearch) {
      clearTimeout(this.timeoutTimer);
      this.projectsService.onEvents.emit(new ProjectServiceEventData({
        type: ProjectServiceEventType.ClearSearch
      }));
      return;
    }

    clearTimeout(this.timeoutTimer);
    this.timeoutTimer = setTimeout(() => {
      this.projectsService.onEvents.emit(new ProjectServiceEventData({
        type: ProjectServiceEventType.RunninSearchByGlobal,
        data: this.textGlobalSearch
      }));
    }, 1000);


  }
}
