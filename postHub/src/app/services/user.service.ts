import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(public http: HttpClient) { }

  // S'inscrire
  async register(username: string, email: string, password: string, passwordConfirm: string): Promise<void> {

    let registerDTO = {
      username: username,
      email: email,
      password: password,
      passwordConfirm: passwordConfirm
    };

    let x = await lastValueFrom(this.http.post<any>("https://localhost:7007/api/Users/Register", registerDTO));
    console.log(x);
  }

  // Se connecter
  async login(username: string, password: string): Promise<void> {
    let loginDTO = {
      username: username,
      password: password
    };

    let x = await lastValueFrom(this.http.post<any>("https://localhost:7007/api/Users/Login", loginDTO));
    console.log(x);

    // N'hésitez pas à ajouter d'autres infos dans le stockage local... pourrait vous aider pour la partie admin / modérateur
    localStorage.setItem("token", x.token);
    localStorage.setItem("username", x.username);
  }

  async changeAvatar(formData: FormData): Promise<void> {
    let x = await lastValueFrom(this.http.put<any>("https://localhost:7007/api/Users/ChangeAvatar", formData));
    console.log(x);
  }

  async changePassword(formData: FormData): Promise<void> {
    let x = await lastValueFrom(this.http.put<any>("https://localhost:7007/api/Users/ChangePassword", formData));
    console.log(x);
  }

  async makeUserModerator(username: string): Promise<void> {
    let x = await lastValueFrom(this.http.put<any>("https://localhost:7007/api/Users/MakeUserModerator/" +username, null));
    console.log(x);
  }
}
