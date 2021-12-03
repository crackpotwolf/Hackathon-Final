import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import {SelectItem} from 'primeng/api';
import { PrimeNGConfig } from 'primeng/api';
import {ProjectsService} from "../../../services/projects.service";
import {ProjectServiceEventData} from "../../../services/data/ProjectServiceEventData";
import {ProjectServiceEventType} from "../../../services/data/ProjectServiceEventType";

@Component({
  selector: 'app-cardgrid',
  templateUrl: './cardgrid.component.html',
  styleUrls: ['./cardgrid.component.css']
})
export class CardgridComponent implements OnInit {

  projects: any = [];

  sortOptions: SelectItem[];

  sortOrder: number;

  sortField: string;

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
    }
    else {
      this.sortOrder = 1;
      this.sortField = value;
    }
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
