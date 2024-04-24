import { Component } from '@angular/core';
import { faChevronDown, faChevronUp, faMagnifyingGlass, faRightFromBracket, faRightToBracket, faStar } from '@fortawesome/free-solid-svg-icons';
import { HubService } from './services/hub.service';
import { Hub } from './models/hub';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  // Icônes Font Awesome
  faMagnifyingGlass = faMagnifyingGlass;
  faStar = faStar;
  faRightToBracket = faRightToBracket;
  faRightFromBracket = faRightFromBracket;
  faChevronDown = faChevronDown;

  searchText : string = "";

  hubsToggled : boolean = false;
  hubList : Hub[] = [];

  constructor(public hubService : HubService){}

  async toggleHubs(){
    this.faChevronDown = this.faChevronDown == faChevronDown ? faChevronUp : faChevronDown;
    this.hubsToggled = !this.hubsToggled;

    if(this.hubsToggled && localStorage.getItem("token") != null){
      let jsonHubs : string | null = localStorage.getItem("myHubs");
      if(jsonHubs != null) this.hubList = JSON.parse(jsonHubs);
    }
  }

  logout(){
    localStorage.clear();
    location.reload();
  }

}
