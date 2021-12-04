import {Component, OnInit} from '@angular/core';
import {ProjectsService} from "../../../../services/projects/projects.service";

@Component({
  selector: 'app-favorites',
  templateUrl: './favorites.component.html',
  styleUrls: ['./favorites.component.sass']
})
export class FavoritesComponent implements OnInit {

  constructor(private projectsService: ProjectsService) {
    this.projectsService.loadProjects();
  }

  ngOnInit(): void {
  }

}
