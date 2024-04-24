import { Component, OnInit } from '@angular/core';
import { PostService } from '../services/post.service';
import { Comment } from '../models/comment';

@Component({
  selector: 'app-commentList',
  templateUrl: './commentList.component.html',
  styleUrls: ['./commentList.component.css']
})
export class CommentListComponent implements OnInit {

  commentList : Comment[] = [];

  constructor(public postService : PostService) { }

  async ngOnInit() {
    // On doit remplir la liste commentList ici avec tous les commentaires signalés !
  }

  async deleteComment(comment : Comment){
    await this.postService.deleteComment(comment.id);
    for(let i = this.commentList.length - 1; i >= 0; i--){
      if(this.commentList[i].id == comment.id){
        this.commentList.splice(i, 1);
      }
    }
  }

}
