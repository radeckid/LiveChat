import { Injectable } from '@angular/core';
import { Subject, Observable, BehaviorSubject } from 'rxjs';
import { User } from '../user';
import { Chat } from '../chat';
import { HttpService } from './http.service';
import { MessageChartModel } from '../message-chart-model';
import { UserNotification } from '../user-notification';
import { ActionNotification } from '../action-notification';
import { NotificationType } from '../notification-type.enum';
import { RelationDeletion } from '../relation-deletion';
import { RelationAdding } from '../relation-adding';
import { MessageDTO } from '../messageDTO';
import { Message } from '../message';
import { LastMessagesController } from '../last-messages-controller';
import { HubsControllerService } from './hubs-controller.service';

@Injectable({
  providedIn: 'root'
})
export class ControlService {

  isUserLogged: Subject<boolean> = new Subject<boolean>();
  user: BehaviorSubject<User> = new BehaviorSubject<User>({email: '', id: 1});
  friends: Subject<Array<User>> = new Subject<Array<User>>();
  currentChat: BehaviorSubject<Chat> = new BehaviorSubject<Chat>({id: 0,  ownerId: 0, name: ''});
  chats: Subject<Array<Chat>> = new Subject();
  notifications: BehaviorSubject<Array<UserNotification>> = new BehaviorSubject<Array<UserNotification>>(null);
  isRefreshRequired: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  isChatRefreshNeeded: Subject<boolean> = new Subject<boolean>();
  messages: BehaviorSubject<Array<Message>> = new BehaviorSubject<Array<Message>>(null);
  chatsMap: Map<number, Array<Message>> = new Map<number, Array<Message>>();

  constructor(private httpService: HttpService, private hubsController: HubsControllerService) {
    this.getIsChatRefreshNeeded().subscribe(x => {
      if (x) {
          this.httpService.getChats(this.user.value.id).subscribe(chats => {
          this.chats.next(chats);
          this.setIsChatRefreshNeeded(false);
        });
      }
    });

    this.hubsController.getMessage().subscribe(message => {
      if (message.chatId === this.currentChat.value.id) {
        this.addMessage(message);
      }
    });

    this.hubsController.getNotification().subscribe(notification => {
      this.addNotification(notification);
    });

    this.hubsController.getIsSuccessLoggedStatus().subscribe(x => {

      if (this.user.value.id !== 0 && x) {
        this.isUserLogged.next(x);
        this.httpService.getChats(this.user.value.id).subscribe(chats => {
          this.setChats(chats);
        });
        this.httpService.getAllFriend(this.user.value.id).subscribe(friends => {
          this.setFriends(friends);
        });
        this.httpService.getUserNotifications(this.user.value.id).subscribe(notifications => {
          this.setNotifications(notifications);
        });
      }
    });
  }

 delay(ms: number) {
    return new Promise( resolve => setTimeout(resolve, ms) );
  }

  public getNotifications(): Observable<Array<UserNotification>> {
    return this.notifications.asObservable();
  }

  public setNotifications(notifications: Array<UserNotification>) {
    this.notifications.next(notifications);
  }

  public notificationProcess(type: NotificationType, notificationProcess: ActionNotification) {
    this.httpService.notificationProcess(notificationProcess).subscribe(x => {
      this.httpService.getUserNotifications(this.user.value.id).subscribe(notifications => {
          this.setNotifications(notifications);
          this.setIsChatRefreshNeeded(true);
      });
    });
  }

  public deleteChat(chatId: number, reason: string) {
    const relationDeletion: RelationDeletion = {senderId: this.user.value.id, chatId, reason};
    this.httpService.deleteRelation(relationDeletion).subscribe(x => {
      this.setIsChatRefreshNeeded(x);
    });
  }
  addRelation(relationAdding: RelationAdding) {
    this.httpService.addingRelation(relationAdding).subscribe(x => {
    });
  }

  addNotification(notification: UserNotification) {
    if (this.user.value.id === notification.receiverId) {
      const tempNotificaitons = this.notifications.value;
      tempNotificaitons.push(notification);
      this.setNotifications(tempNotificaitons);
    }
  }

  public getIsChatRefreshNeeded(): Observable<boolean> {
    return this.isChatRefreshNeeded;
  }

  public setIsChatRefreshNeeded(isChatRefreshNeeded: boolean) {
    this.isChatRefreshNeeded.next(isChatRefreshNeeded);
  }

  public getChats(): Observable<Array<Chat>> {
    return this.chats;
  }

  public setChats(chats: Array<Chat>) {
    this.chats.next(chats);
  }

  public getChat(): Observable<Chat> {
    return this.currentChat;
  }

  public setChat(chat: Chat) {
    this.setMessages(null);
    this.currentChat.next(chat);
  }

  setFriends(friends: Array<User>) {
    this.friends.next(friends);
  }

  getFriends(): Observable<Array<User>> {
    return this.friends.asObservable();
  }

  setUser(user: User) {
    this.user.next(user);
    this.setLogged(user);
  }

  getUser(): Observable<User> {
    return this.user.asObservable();
  }

  getUserId(): number {
    return this.user.value.id;
  }

  setLogged(user: User) {
    this.hubsController.startListen(user.id);
  }

  getLogged(): Observable<boolean> {
    return this.isUserLogged.asObservable();
  }

  setRefreshStatus(messageChartModels: Array<MessageChartModel>) {
    messageChartModels.forEach(messageChartModel => {
      if (this.currentChat.value.id === messageChartModel.chatId) {
        this.isRefreshRequired.next(this.isRefreshRequired.value + 1);
      }
    });
  }

  getMessages(): Observable<Array<Message>> {
    return this.messages;
  }

  setMessages(messages: Array<Message>) {
    this.messages.next(messages);
  }

  getLastMessages() {
    let idLastMessage: number;
    if (this.messages.value == null || this.messages.value.length === 0) {
      idLastMessage = 0;
    } else {
      idLastMessage = this.messages.value[0].id;
    }
    const lastMessageController: LastMessagesController = {idLastMessage,
      chatId: this.currentChat.value.id, requesterId: this.user.value.id};
    this.httpService.getLastMessages(lastMessageController).subscribe(messages => {
      if (this.messages.value != null) {
        this.messages.value.forEach(message => {
          messages.push(message);
        });
      }
      this.messages.next(messages);
    });
  }

  addMessage(message: Message) {
    const messages: Array<Message> = this.messages.value;
    messages.push(message);
    this.messages.next(messages);
  }

  sendMessage(message: MessageDTO) {
    this.httpService.sendMessage(message).subscribe(value => console.log(value));
  }

  getRefreshStatus(): Observable<number> {
    return this.isRefreshRequired;
  }

  getUsers(): Observable<Array<User>> {
    return this.httpService.getUsers();
  }
}
