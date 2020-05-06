import { Component, OnInit, ViewChild, ElementRef, QueryList, ViewChildren, AfterViewInit } from '@angular/core';
import { MessageDTO } from '../messageDTO';
import { Message } from '../message';
import { ControlService } from '../services/control.service';
import { User } from '../user';
import { Chat } from '../chat';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, AfterViewInit {

  @ViewChild('chat') private myScrollContainer: ElementRef;
  @ViewChildren('messagesCointener') messagesCointener: QueryList<any>;

  user: User;
  input: string;
  chat: Chat;
  messages: Array<Message>;
  isRefreshRequired: boolean;
  isAlert: boolean;

  // tslint:disable-next-line: max-line-length
  constructor(private controlService: ControlService) {
    this.controlService.getUser().subscribe(user => {
      this.user = user;
    });

    this.controlService.getChat().subscribe(chat => {
      this.chat = chat;
      if (this.chat.id !== 0) {
        this.controlService.getLastMessages();
      }
    });

    this.controlService.getMessages().subscribe(messages => {
      this.messages = messages;
      this.down();
    });
  }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
    this.messagesCointener.changes.subscribe(t => {
      this.down();
    });
  }

  send() {
    const message: MessageDTO = {
      senderId: this.user.id.toString(),
      chatId: this.chat.id.toString(),
      date: '01-02-2020',
      content: this.input};
    this.controlService.sendMessage(message);
    this.input = '';
  }

  down() {
    try {
      console.log('here');
      this.myScrollContainer.nativeElement.scrollTop = this.myScrollContainer.nativeElement.scrollHeight;
    } catch (err) {
      console.log("down");
    }
  }

  loadPrevious() {
    this.controlService.getLastMessages();
  }

  ok() {
    this.isAlert = false;
  }
}
