import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  sectionList: Section[] = [];

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http.get<{ sectionList: Section[] }>(baseUrl + 'account/sectionList')
      .subscribe(data => {
        this.sectionList = data.sectionList;
        console.log(baseUrl + 'account/sectionList');
        console.log(this.sectionList);
      });
  }


  
}

interface Section {
  id: number;
  sectionName: string;
}
