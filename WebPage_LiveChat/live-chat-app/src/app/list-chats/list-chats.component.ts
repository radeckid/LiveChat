import { Component, OnInit } from '@angular/core';
import { ControlService } from '../services/control.service';
import { HttpService } from '../services/http.service';
import { User } from '../user';

@Component({
  selector: 'app-list-chats',
  templateUrl: './list-chats.component.html',
  styleUrls: ['./list-chats.component.css']
})
export class ListChatsComponent implements OnInit {

  isLogged: boolean;
  friendList: Array<User>;

  constructor(private controlService: ControlService, private httpService: HttpService) { }

  ngOnInit(): void {
    this.controlService.getLogged().subscribe( isLogged => {
      if(isLogged) {
        this.friendList = this.httpService.getAllFriend(this.controlService.user, this.controlService.token);
      }
    });
  }

}
