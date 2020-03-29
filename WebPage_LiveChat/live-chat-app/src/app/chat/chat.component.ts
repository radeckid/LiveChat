import { Component, OnInit } from '@angular/core';
import { Collection } from '../collection';
import { MessageDTO } from '../messageDTO';
import { Message } from '../message';
import { MessageSignalRService } from '../services/message-signal-r.service';
import { ControlService } from '../services/control.service';
import { HttpService } from '../services/http.service';
import { User } from '../user';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  user: User;
  input: string;
  chat: Collection<Message>;

  // tslint:disable-next-line: max-line-length
  constructor(private messageSignalRService: MessageSignalRService, private controlService: ControlService, private httpService: HttpService) {
    this.chat = new Collection<Message>();
    console.log(this.user);
    this.controlService.getUser().subscribe(user => {
      this.user = user;
    });
  }

  ngOnInit(): void {
  }

  send() {
    const message: MessageDTO = {
      SenderId: this.user.id,
      ReceiverId: this.controlService.getReceiver().id,
      Date: new Date(),
      Content: this.input};
    this.httpService.sendMessage(message).subscribe(value => {});
    this.input = '';
  }

  loadPrevious() {

  }
}
