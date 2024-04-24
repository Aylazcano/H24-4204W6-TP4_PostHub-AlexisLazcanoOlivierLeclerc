import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { Hub } from '../models/hub';

@Injectable({
  providedIn: 'root'
})
export class HubService {

  constructor(public http : HttpClient) { }

  // Créer un nouveau hub
  async postHub(hubName : string) : Promise<Hub>{

    let newHub = new Hub(0, hubName, false);

    let x = await lastValueFrom(this.http.post<Hub>("https://localhost:7007/api/Hubs/PostHub", newHub));
    console.log(x);
    return x;

  }

  // Obtenir un hub précis quand on affiche ses posts
  async getHub(id : number) : Promise<Hub>{
    console.log(id);
    let x = await lastValueFrom(this.http.get<Hub>("https://localhost:7007/api/Hubs/GetHub/" + id));
    console.log(x);
    return x;
  }

  // Obtenir la liste de hubs de l'utilisateur
  async getUserHubs() : Promise<Hub[]>{

    let x = await lastValueFrom(this.http.get<Hub[]>("https://localhost:7007/api/Hubs/GetUserHubs"));
    console.log(x);
    return x;

  }

  // Rejoindre / quitter un hub
  async toggleHubJoin(id : number) : Promise<void>{
    let x = await lastValueFrom(this.http.put<any>("https://localhost:7007/api/Hubs/ToggleJoinHub/" + id, null));
    console.log(x);
  }

}
