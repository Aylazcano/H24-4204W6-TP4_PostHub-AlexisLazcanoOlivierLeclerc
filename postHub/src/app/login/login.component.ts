import { HubService } from './../services/hub.service';
import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { Hub } from '../models/hub';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginUsername : string = "";
  loginPassword : string = "";

  constructor(public userService : UserService, public hubService : HubService, public router : Router) { }

  ngOnInit() {}

  async login() : Promise<void>{
    await this.userService.login(this.loginUsername, this.loginPassword);

    let x : Hub[] = await this.hubService.getUserHubs();
    localStorage.setItem("myHubs", JSON.stringify(x));

    this.router.navigate(["/postList", "index"]);
  }

}
