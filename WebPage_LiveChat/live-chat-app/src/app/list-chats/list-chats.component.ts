import { Component, OnInit } from '@angular/core';
import { ControlService } from '../services/control.service';
import { HttpService } from '../services/http.service';
import { User } from '../user';
import { Chat } from '../chat';

@Component({
  selector: 'app-list-chats',
  templateUrl: './list-chats.component.html',
  styleUrls: ['./list-chats.component.css']
})
export class ListChatsComponent implements OnInit {

  isLogged: boolean;
  chats: Array<Chat>;
  user: User;

  constructor(private controlService: ControlService, private httpService: HttpService) {
    this.controlService.getUser().subscribe(user => {
      this.user = user;
    });
    this.controlService.getLogged().subscribe( isLogged => {
      if (isLogged) {
        this.controlService.getChats().subscribe(chats => {
          this.chats = chats;
        });
      }
    });
   }

  ngOnInit(): void {

  }

  setChat(chat: Chat) {
    this.controlService.setChat(chat);
  }

  deleteChat(chat: Chat) {
    this.controlService.deleteChat(chat.id, 'reason');
  }
}
