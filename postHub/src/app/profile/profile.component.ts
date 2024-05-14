import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UserService } from '../services/user.service';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  userIsConnected: boolean = false;

  // Vous êtes obligés d'utiliser ces trois propriétés
  oldPassword: string = "";
  newPassword: string = "";
  newPasswordConfirm: string = "";
  newModerator: string = "";

  username: string | null = null;
  isAdmin: boolean = false;
  isModerator: boolean = false;

  @ViewChild("AvatarViewChild", { static: false }) avatarInput?: ElementRef;
  avatarImage: any;

  constructor(public userService: UserService) { }

  ngOnInit() {
    this.userIsConnected = localStorage.getItem("token") != null;
    this.username = localStorage.getItem("username");
    let roles = localStorage.getItem("roles");
    if(roles?.includes("admin")){
      this.isAdmin = true;
    }
    if(roles?.includes("moderator")){
      this.isModerator = true;
    }
  }

  async changeAvatar() {
    if (this.avatarInput == undefined) {
      console.log("Input HTML non chargé.");
      return;
    }

    let file = this.avatarInput.nativeElement.files[0];
    if (file == null) {
      console.log("Input HTML ne contient aucune image.");
      return;
    }

    let formData = new FormData();
    formData.append("avatarImage", file);
    await this.userService.changeAvatar(formData);
    window.location.reload();
  }

  async changePassword(){
    let formData = new FormData();
    formData.append("oldPassword", this.oldPassword)
    formData.append("newPassword", this.newPassword)
    formData.append("newPasswordConfirm", this.newPasswordConfirm)
    await this.userService.changePassword(formData)
  }

  async makeUserModerator(){
    await this.userService.makeUserModerator(this.newModerator)
  }
}