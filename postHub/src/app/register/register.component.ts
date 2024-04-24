import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerUsername : string = "";
  registerEmail : string = "";
  registerPassword : string = "";
  registerPasswordConfirm : string = "";

  constructor(public userService : UserService, public router : Router) { }

  ngOnInit() {}

  async register() : Promise<void>{
    await this.userService.register(this.registerUsername, this.registerEmail, this.registerPassword, this.registerPasswordConfirm);
    this.router.navigate(["/login"]);
  }

}
