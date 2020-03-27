import { Component, OnInit } from '@angular/core';
import { Collection } from '../collection';
import { MessageDTO } from '../messageDTO';
import { Message } from '../message';
import { SignalRService } from '../services/signal-r.service';
import { ControlService } from '../services/control.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  input: string;
  chat: Collection<Message>;

  constructor(private signalRService: SignalRService, private controlService: ControlService) {
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
    this.chat.add({content: this.input, owner: 'test', date: new Date(1995, 11, 17)});
    this.input = '';
  }

  loadPrevious() {

  }
}
