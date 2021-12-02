import { HttpClient } from "@angular/common/http";
import { Injectable } from '@angular/core';

import {Project} from "../data/project";

@Injectable()
export class ProjectService {

  url: string ='/api/accelerator/v1/project/full-all';

  constructor(private http: HttpClient) { }

  getProjects() {
    return this.http.get<any>(this.url)
      .toPromise()
      .then(res => <Project[]>res)
      .then(data => { return data; });
  }
}
