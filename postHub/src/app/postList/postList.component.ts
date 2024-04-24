import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Hub } from '../models/hub';
import { HubService } from '../services/hub.service';
import { PostService } from '../services/post.service';
import { Post } from '../models/post';

@Component({
  selector: 'app-postList',
  templateUrl: './postList.component.html',
  styleUrls: ['./postList.component.css']
})
export class PostListComponent implements OnInit {

  hub : Hub | null = null;
  posts : Post[] = [];
  sorting : string = "popular";

  constructor(public route : ActivatedRoute, public hubService : HubService, public postService : PostService) { }

  async ngOnInit() {

    this.route.paramMap.subscribe(p => {
      this.loadPosts();
    })

    this.loadPosts();

  }

  async loadPosts(){
    let userIsConnected = localStorage.getItem("token") != null;
    let tab : string | null = this.route.snapshot.paramMap.get("tab");
    let hubId : string | null = this.route.snapshot.paramMap.get("hubId");
    let searchText : string | null = this.route.snapshot.paramMap.get("searchText");

    if(hubId != null){
      this.hub = await this.hubService.getHub(+hubId);
      this.posts = await this.postService.getHubPosts(this.hub.id, this.sorting);
    }
    else if(tab == "index" && userIsConnected){
      this.posts = await this.postService.getPostList("myHubs", this.sorting);
    }
    else if(tab == "discover"){
      this.posts = await this.postService.getPostList("discover", this.sorting);
    }
    else if(searchText != null){
      this.posts = await this.postService.searchPosts(searchText, this.sorting);
    }
  }

  async toggleHubJoin(){
    if(this.hub == null) return;
    await this.hubService.toggleHubJoin(this.hub.id);
    this.hub.isJoined = !this.hub.isJoined;

    let hubListJson = localStorage.getItem("myHubs");
    let hubList : Hub[] = [];
    if(hubListJson != null) hubList = JSON.parse(hubListJson);

    if(this.hub.isJoined){
      hubList.push(this.hub);
    }
    else{
      for(let i = 0; i < hubList.length; i++){
        if(hubList[i].id == this.hub.id) hubList.splice(i, 1);
      }
    }

    localStorage.setItem("myHubs", JSON.stringify(hubList));
  }

  async toggleSorting(){
    this.posts = await this.postService.getPostList("myHubs", this.sorting);
  }

}
