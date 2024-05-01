import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  userIsConnected : boolean = false;

  // Vous êtes obligés d'utiliser ces trois propriétés
  oldPassword : string = "";
  newPassword : string = "";
  newPasswordConfirm : string = "";

  username : string | null = null;
  
  @ViewChild("PicViewChild", { static: false }) picInput?: ElementRef;

  constructor(public userService : UserService) { }

  ngOnInit() {
    this.userIsConnected = localStorage.getItem("token") != null;
    this.username = localStorage.getItem("username");
  }

  // TODO ChangeAvatar
  async ChangeAvatar(){
    let formData = new FormData();
    if (this.picInput != undefined) {
      let file = this.picInput.nativeElement.file;
      if (file != null) {
        formData.append("pics", file, file.fileName);
      }
    }

  }

}
