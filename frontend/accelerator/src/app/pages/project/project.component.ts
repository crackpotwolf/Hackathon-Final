import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {Project} from "../../data/Project";
import {forkJoin} from "rxjs";
import {Activities} from "../../data/Activities";
import {Budget} from "../../data/Budget";
import {Team} from "../../data/Team";
import {Status} from "../../data/Status";
import {Stage} from "../../data/Stage";
import {Meeting} from "../../data/Meeting";
import {Material} from "../../data/Material";
import {Effects} from "../../data/Effects";
import * as moment from "moment";
import {ProjectsService} from "../../../services/projects/projects.service";
import {ProjectServiceEventData} from "../../../services/projects/data/ProjectServiceEventData";
import {ProjectServiceEventType} from "../../../services/projects/data/ProjectServiceEventType";

moment.locale('ru');

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.sass']
})
export class ProjectComponent implements OnInit {

  project: Project;
  isError: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private projectsService: ProjectsService,
  ) {
    let guidProject = this.route.snapshot.paramMap.get('guid');
    console.log(guidProject);
    this.loadProject(guidProject);
    this.loadPhotos(guidProject);
    this.loadSimilar(guidProject);
  }


  ngOnInit(): void {
  }


  linksPhoto: string[];
  responsiveOptions = [
    {
      breakpoint: '1024px',
      numVisible: 3,
      numScroll: 3
    },
    {
      breakpoint: '768px',
      numVisible: 2,
      numScroll: 2
    },
    {
      breakpoint: '560px',
      numVisible: 1,
      numScroll: 1
    }
  ];

  loadPhotos(guidProject: string | null) {
    this.http.get<string[]>(`/api/accelerator/v1/project-photos/${guidProject}/photos/url`)
      .subscribe(resp => {
        this.linksPhoto = [...resp, ...resp, ...resp, ...resp];
      });
  }

  similarProjects: any[];

  loadSimilar(guidProject: string | null) {
    this.http.get<any[]>(`api/accelerator/v1/project/get-similar?guid=${guidProject}`)
      .subscribe(resp => {
        this.similarProjects = resp;
      });
  }


  /**
   * Загрузка информации о проекте
   * @param guidProject Идентификатор проекта
   * @private
   */
  private loadProject(guidProject: string | null) {
    forkJoin({
      project: this.http.get<Project>(`/api/accelerator/v1/project/lite?guid=${guidProject}`),
      activities: this.http.get<Activities[]>(`/api/accelerator/v1/project/activities?projectGuid=${guidProject}`),
      budget: this.http.get<Budget[]>(`/api/accelerator/v1/project/budget?projectGuid=${guidProject}`),
      effects: this.http.get<Effects[]>(`/api/accelerator/v1/project/effects?projectGuid=${guidProject}`),
      materials: this.http.get<Material[]>(`/api/accelerator/v1/project/materials?projectGuid=${guidProject}`),
      meetings: this.http.get<Meeting[]>(`/api/accelerator/v1/project/meetings?projectGuid=${guidProject}`),
      stages: this.http.get<Stage[]>(`/api/accelerator/v1/project/stages?projectGuid=${guidProject}`),
      statuses: this.http.get<Status[]>(`/api/accelerator/v1/project/statuses?projectGuid=${guidProject}`),
      teams: this.http.get<Team[]>(`/api/accelerator/v1/project/teams?projectGuid=${guidProject}`),
    }).subscribe(resp => {
      this.project = resp.project;
      this.project.activities = resp.activities;
      this.project.budget = resp.budget;
      this.project.effects = resp.effects;
      this.project.material = resp.materials;
      this.project.meeting = resp.meetings;
      this.project.stage = resp.stages.sort((a, b) => moment(a.date, 'M.YYYY') < moment(b.date, 'M.YYYY') ? -1 : 1).map(p => {
        p.date = moment(p.date, 'M.YYYY').format("MMMM YYYY");
        p.date = p.date[0].toUpperCase() + p.date.slice(1);
        return p;
      });
      this.project.status = resp.statuses;
      this.project.team = resp.teams;

      this.project.timingMonths = moment(this.project.timing, 'M.YYYY').diff(moment(), 'months');
      this.project.totalBudget = resp.project.budget.length ? (resp.project.budget.map(p => parseFloat(p.value)).reduce((a, b) => a + b) / 1000).toFixed(2) : '';


      let favorites: any[] = JSON.parse(localStorage.getItem('favorites')) ?? [];
      this.project.isFavorite = favorites.indexOf(this.project.guid) != -1;
    }, err => {
      this.isError = true;
    });
  }

  onClickFile(file: Material) {
    window.open(file.link, '_blank').focus();
  }

  onClickFavorites() {
    this.project.isFavorite = !this.project.isFavorite;
    this.projectsService.onEvents.emit(new ProjectServiceEventData({
      type: ProjectServiceEventType.onClickFavorites,
      data: this.project.guid
    }));
  }
}
