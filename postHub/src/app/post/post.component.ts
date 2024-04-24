import { Component, Input, OnInit } from '@angular/core';
import { faDownLong, faEllipsis, faMessage, faUpLong } from '@fortawesome/free-solid-svg-icons';
import { Post } from '../models/post';
@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit {

  @Input() post : Post | null = null;

  // Icônes Font Awesome
  faEllipsis = faEllipsis;
  faUpLong = faUpLong;
  faDownLong = faDownLong;
  faMessage = faMessage;

  constructor() { }

  ngOnInit(): void {
    
  }

}
