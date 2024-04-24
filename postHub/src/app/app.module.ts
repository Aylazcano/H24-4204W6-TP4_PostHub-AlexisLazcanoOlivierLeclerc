import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { PostComponent } from './post/post.component';
import { PostListComponent } from './postList/postList.component';
import { ProfileComponent } from './profile/profile.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { FullPostComponent } from './fullPost/fullPost.component';
import { CommentComponent } from './comment/comment.component';
import { EditPostComponent } from './editPost/editPost.component';
import { FormsModule } from '@angular/forms';
import { AuthInterceptor } from './auth.interceptor';
import { NewHubComponent } from './newHub/newHub.component';

import { registerLocaleData } from '@angular/common';
import localeFr from '@angular/common/locales/fr';
import { CommentListComponent } from './commentList/commentList.component';
registerLocaleData(localeFr, "fr");

// Si vous cherchez les règles de routage elles sont dans app-routing.module.ts !
// Normalement, il n'y a rien à modifier dans les modules... mais vous pouvez le faire.

@NgModule({
  declarations: [												
    AppComponent,
      PostComponent,
      PostListComponent,
      ProfileComponent,
      RegisterComponent,
      LoginComponent,
      FullPostComponent,
      CommentComponent,
      EditPostComponent,
      NewHubComponent,
      CommentListComponent
   ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FontAwesomeModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [
    {provide : HTTP_INTERCEPTORS, useClass : AuthInterceptor, multi:true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
