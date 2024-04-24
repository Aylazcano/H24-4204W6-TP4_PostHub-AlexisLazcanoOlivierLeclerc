import { Component, OnInit } from '@angular/core';
import { HubService } from '../services/hub.service';
import { Hub } from '../models/hub';
import { Router } from '@angular/router';

@Component({
  selector: 'app-newHub',
  templateUrl: './newHub.component.html',
  styleUrls: ['./newHub.component.css']
})
export class NewHubComponent implements OnInit {

  userIsConnected : boolean = false;
  hubName : string = "";

  constructor(public hubService : HubService, public router : Router) { }

  ngOnInit() {
    this.userIsConnected = localStorage.getItem("token") != null;
  }

  async createHub(){
    if(this.hubName == ""){
      alert("Met un nom niochon !");
      return;
    }
    let hub : Hub = await this.hubService.postHub(this.hubName);
    let jsonHubs : string | null = localStorage.getItem("myHubs");
    let hubList : Hub[] = [];
      if(jsonHubs != null) hubList = JSON.parse(jsonHubs);
    hubList.push(hub);
    localStorage.setItem("myHubs", JSON.stringify(hubList));
    this.router.navigate(["/postList/hub", hub.id]);
  }

}
