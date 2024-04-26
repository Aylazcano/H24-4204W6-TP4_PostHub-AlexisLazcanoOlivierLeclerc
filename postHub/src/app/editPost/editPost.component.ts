import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { faDownLong, faEllipsis, faMessage, faUpLong } from '@fortawesome/free-solid-svg-icons';
import { Hub } from '../models/hub';
import { HubService } from '../services/hub.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PostService } from '../services/post.service';
import { Post } from '../models/post';

@Component({
  selector: 'app-editPost',
  templateUrl: './editPost.component.html',
  styleUrls: ['./editPost.component.css']
})
export class EditPostComponent implements OnInit {

  hub : Hub | null = null;
  postTitle : string = "";
  postText : string = "";
  @ViewChild("PicViewChild", {static:false}) picInput ?: ElementRef;

  // Icônes Font Awesome
  faEllipsis = faEllipsis;
  faUpLong = faUpLong;
  faDownLong = faDownLong;
  faMessage = faMessage;

  constructor(public hubService : HubService, public route : ActivatedRoute, public postService : PostService, public router : Router) { }

  async ngOnInit() {
    let hubId : string | null = this.route.snapshot.paramMap.get("hubId");

    if(hubId != null){
      this.hub = await this.hubService.getHub(+hubId);
    }
  }

  // Créer un nouveau post (et son commentaire principal)
  async createPost(){
    if(this.postTitle == "" || this.postText == ""){
      alert("Remplis mieux le titre et le texte niochon");
      return;
    }
    if(this.hub == null) return;

    let formData = new FormData();
    formData.append("PostTitle", this.postTitle);
    formData.append("PostText", this.postText);

    if(this.picInput != undefined){
      let files = this.picInput.nativeElement.files;
      if(files == null){
        console.log("Input ne contient pas d'image");
      } else {
        for(let file of files){
          formData.append("ImageUpload", file, file.fileName);
        }
      }
    }

    let newPost : Post = await this.postService.postPost(this.hub.id, formData);

    // On se déplace vers le nouveau post une fois qu'il est créé
    this.router.navigate(["/post", newPost.id]);
  }

}
