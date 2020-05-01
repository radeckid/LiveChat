import { Component, OnInit } from '@angular/core';
import { Collection } from '../collection';
import { MessageDTO } from '../messageDTO';
import { Message } from '../message';
import { MessageSignalRService } from '../services/message-signal-r.service';
import { ControlService } from '../services/control.service';
import { HttpService } from '../services/http.service';
import { User } from '../user';
import { ThemeService } from 'ng2-charts';
import { Chat } from '../chat';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  user: User;
  input: string;
  chat: Chat;
  messages: Array<Message>;
  isRefreshRequired: boolean;
  isAlert: boolean;

  // tslint:disable-next-line: max-line-length
  constructor(private messageSignalRService: MessageSignalRService, private controlService: ControlService, private httpService: HttpService) {
    this.controlService.getUser().subscribe(user => {
      this.user = user;
    });

    this.messages = new Array<Message>();

    this.controlService.getChat().subscribe(chat => {
      this.chat = chat;
      this.httpService.getAllMessages(this.user.id, this.chat.id).subscribe(messages => {
        this.messages = messages;
      }, err => {
        this.isAlert = true;
      });
    });

    this.controlService.getRefreshStatus().subscribe(number => {
        this.httpService.getAllMessages(this.user.id, this.chat.id).subscribe(messages => {
            this.messages = messages;
        });
      });
  }

  ngOnInit(): void {
  }

  send() {
    const message: MessageDTO = {
      senderId: this.user.id.toString(),
      chatId: this.chat.id.toString(),
      date: '01-02-2020',
      content: this.input};
    this.httpService.sendMessage(message).subscribe(value => console.log(value));
    this.input = '';
  }

  loadPrevious() {

  }

  ok() {
    this.isAlert = false;
  }
}
