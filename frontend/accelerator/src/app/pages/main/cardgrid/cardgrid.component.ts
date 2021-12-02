import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";

@Component({
  selector: 'app-cardgrid',
  templateUrl: './cardgrid.component.html',
  styleUrls: ['./cardgrid.component.sass']
})
export class CardgridComponent implements OnInit {
  public data:any=[]
  constructor(private http: HttpClient) { }

  getData(){
    const url ='/api/accelerator/v1/project/full-all'
    this.http.get(url).subscribe((res)=>{
      this.data = res
      console.log(this.data)
    })
  }
  ngOnInit(): void {
    this.getData()
  }

}
