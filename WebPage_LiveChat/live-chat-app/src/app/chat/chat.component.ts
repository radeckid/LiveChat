import { Component, OnInit } from '@angular/core';
import { Collection } from '../collection';
import { MessageDTO } from '../messageDTO';
import { Message } from '../message';
import { MessageSignalRService } from '../services/signal-r.service';
import { ControlService } from '../services/control.service';
import { HttpService } from '../services/http.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  input: string;
  chat: Collection<Message>;

  constructor(private signalRService: MessageSignalRService, private controlService: ControlService, private httpService: HttpService) {
    this.chat = new Collection<Message>();

    this.signalRService.getDate().subscribe(x => {
      x.forEach(chart => {
        this.controlService.
      });
    });
  }

  ngOnInit(): void {
  }

  send() {
    const message: Message = {, owner: 'test', date: new Date(1995, 11, 17)};
    this.httpService.sendMessage
    this.input = '';
  }

  loadPrevious() {

  }
}
