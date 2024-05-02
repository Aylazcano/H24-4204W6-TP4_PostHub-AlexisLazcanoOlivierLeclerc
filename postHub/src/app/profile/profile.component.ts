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

  username: string | null = null;

  @ViewChild("AvatarViewChild", { static: false }) avatarInput?: ElementRef;
  avatarImage: any;

  constructor(public userService: UserService) { }

  ngOnInit() {
    this.userIsConnected = localStorage.getItem("token") != null;
    this.username = localStorage.getItem("username");
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

}