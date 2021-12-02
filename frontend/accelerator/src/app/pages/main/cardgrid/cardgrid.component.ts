import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import {ProjectService} from "./services/projectservices";
import {SelectItem} from 'primeng/api';
import { PrimeNGConfig } from 'primeng/api';

@Component({
  selector: 'app-cardgrid',
  templateUrl: './cardgrid.component.html',
  styleUrls: ['./cardgrid.component.sass']
})
export class CardgridComponent implements OnInit {

  projects: any = [];

  sortOptions: SelectItem[];

  sortOrder: number;

  sortField: string;

  constructor(private projectService: ProjectService, private http: HttpClient,
              private primengConfig: PrimeNGConfig) { }

  ngOnInit(): void {
    this.projectService.getProjects().then(data => this.projects = data);

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
}
