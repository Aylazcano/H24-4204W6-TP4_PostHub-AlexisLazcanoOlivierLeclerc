import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { Post } from '../models/post';
import { Comment } from '../models/comment';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  constructor(public http: HttpClient) { }

  // Obtenir une liste de posts en triant par nouveauté / popularité
  async getPostList(tab: string, sorting: string): Promise<Post[]> {
    let x = await lastValueFrom(this.http.get<Post[]>("https://localhost:7007/api/Comments/GetPosts/" + tab + "/" + sorting));
    console.log(x);
    return x;
  }

  // Obtenir tous les posts d'un hub triés par nouveauté / popularité
  async getHubPosts(hubId: number, sorting: string): Promise<Post[]> {
    let x = await lastValueFrom(this.http.get<Post[]>("https://localhost:7007/api/Comments/GetHubPosts/" + hubId + "/" + sorting));
    console.log(x);
    return x;
  }

  // Recherche des posts avec la barre du header (la phrase utilisée est chercher dans les titres des posts et dans les commentaires principaux des posts)
  async searchPosts(searchText: string, sorting: string): Promise<Post[]> {
    let x = await lastValueFrom(this.http.get<Post[]>("https://localhost:7007/api/Comments/SearchPosts/" + searchText + "/" + sorting));
    console.log(x);
    return x;
  }

  // Créer un post
  async postPost(hubId: number, formData: any): Promise<Post> {

    let x = await lastValueFrom(this.http.post<any>("https://localhost:7007/api/Comments/PostPost/" + hubId, formData));
    console.log(x);
    return x;
  }

  // Obtenir un post précis et tous ses commentaires classés par nouveauté / popularité
  async getPost(postId: number, sorting: string): Promise<Post> {
    let x = await lastValueFrom(this.http.get<Post>("https://localhost:7007/api/Comments/GetFullPost/" + postId + "/" + sorting));
    console.log(x);
    return x;
  }

  // Modifier un commentaire (que ce soit le commentaire principal d'un post ou un sous-commentaire)
  async editComment(formData: FormData, commentId: number): Promise<Comment> {

    let x = await lastValueFrom(this.http.put<any>("https://localhost:7007/api/Comments/PutComment/" + commentId, formData));
    console.log(x);
    return x;

  }

  // Créer un sous-commentaire (donc tous les commentaires qui ne sont pas le commentaire principal d'un post)
  async postComment(formData: FormData, parentCommentId: number): Promise<Comment> {

    let x = await lastValueFrom(this.http.post<any>("https://localhost:7007/api/Comments/PostComment/" + parentCommentId, formData));
    console.log(x);
    return x;

  }

  // Supprimer un commentaire (que ce soit le commentaire principal d'un post ou un sous-commentaire)
  async deleteComment(commentId: number): Promise<void> {

    let x = await lastValueFrom(this.http.delete<any>("https://localhost:7007/api/Comments/DeleteComment/" + commentId));
    console.log(x);

  }

  // Upvote un commentaire (que ce soit le commentaire principal d'un post ou un sous-commentaire)
  async upvote(commentId: number) {
    let x = await lastValueFrom(this.http.put<any>("https://localhost:7007/api/Comments/UpvoteComment/" + commentId, null));
    console.log(x);
  }

  // Downvote un commentaire (que ce soit le commentaire principal d'un post ou un sous-commentaire)
  async downvote(commentId: number) {
    let x = await lastValueFrom(this.http.put<any>("https://localhost:7007/api/Comments/DownvoteComment/" + commentId, null));
    console.log(x);
  }

}
