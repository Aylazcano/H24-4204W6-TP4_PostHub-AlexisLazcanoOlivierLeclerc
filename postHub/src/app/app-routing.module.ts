import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PostListComponent } from './postList/postList.component';
import { ProfileComponent } from './profile/profile.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { FullPostComponent } from './fullPost/fullPost.component';
import { EditPostComponent } from './editPost/editPost.component';
import { NewHubComponent } from './newHub/newHub.component';
import { CommentListComponent } from './commentList/commentList.component';

const routes: Routes = [
  {path:"", redirectTo:"/postList/index", pathMatch:"full"},
  {path:"postList", redirectTo:"/postList/index", pathMatch:"full"},
  {path:"postList/:tab", component:PostListComponent}, // Route pour accueil / vos hubs
  {path:"postList/hub/:hubId", component:PostListComponent}, // Route pour un hub précis
  {path:"postList/search/:searchText", component:PostListComponent}, // Router pour une recherche
  {path:"profile", component:ProfileComponent},
  {path:"register", component:RegisterComponent},
  {path:"login", component:LoginComponent},
  {path:"post/:postId", component:FullPostComponent},
  {path:"editPost/:hubId", component:EditPostComponent},
  {path:"newHub", component:NewHubComponent},
  {path:"reports", component:CommentListComponent}
];

// Normalement il n'y a pas de routes à ajouter ni à retirer... mais vous avez le droit de faire des modifications.

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
