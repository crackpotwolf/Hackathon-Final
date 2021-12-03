import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {Project} from "../../data/Project";

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
  ) {
    let guidProject = this.route.snapshot.paramMap.get('guid');
    console.log(guidProject);
    this.loadProject(guidProject);
  }


  ngOnInit(): void {
  }

  /**
   * Загрузка информации о проекте
   * @param guidProject Идентификатор проекта
   * @private
   */
  private loadProject(guidProject: string | null) {
    this.http.get<Project>(`/api/accelerator/v1/project?guid=${guidProject}`)
      .subscribe(resp => {
        this.project = resp;
      }, err => {
        this.isError = true;
      });
  }
}
