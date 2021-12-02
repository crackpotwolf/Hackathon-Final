import {Component, OnInit} from '@angular/core';
import {ProjectsService} from "../../services/projects.service";

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.sass']
})
export class MainComponent implements OnInit {

  constructor(private projectsService: ProjectsService) {
  }

  ngOnInit(): void {
    this.projectsService.loadProjects();
  }

}
