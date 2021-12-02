import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.sass']
})
export class ProjectComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
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

  }
}
