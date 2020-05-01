import { Injectable } from '@angular/core';
import { Subject, Observable, BehaviorSubject } from 'rxjs';
import { User } from '../user';
import { Chat } from '../chat';
import { HttpService } from './http.service';
import { MessageChartModel } from '../message-chart-model';
import { ConnectionSignalRService } from '../connection-signal-r.service';
import { UserNotification } from '../user-notification';
import { ActionNotification } from '../action-notification';
import { NotificationType } from '../notification-type.enum';
import { RelationDeletion } from '../relation-deletion';
import { RelationAdding } from '../relation-adding';
import { concatMap, mergeMap } from 'rxjs/operators';

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

  constructor(private httpService: HttpService, private connectionSignalRSerivce: ConnectionSignalRService) { }

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
          this.httpService.getChats(this.user.value.id).subscribe(chats => {
            console.log('i am here');
            this.setChats(chats);
          });
      });
    });
  }

  public deleteChat(chatId: number, reason: string) {
    let isEnd: boolean = false;
    console.log('deleted');
    const relationDeletion: RelationDeletion = {senderId: this.user.value.id, chatId, reason};
    console.log('before deletion');
    this.httpService.deleteRelation(relationDeletion).subscribe(x => {
      console.log('here');
      isEnd = true;
    }, error => {
      isEnd = true;
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
    this.isUserLogged.next(true);
    this.httpService.getChats(user.id).subscribe(chats => {
      this.setChats(chats);
    });
    this.httpService.getAllFriend(user.id).subscribe(friends => {
      this.setFriends(friends);
    });
    this.httpService.getUserNotifications(user.id).subscribe(notifications => {
      this.setNotifications(notifications);
    });
    //this.connectionSignalRSerivce.startConnection(user.id);
  }

  getLogged(): Observable<boolean> {
    return this.isUserLogged.asObservable();
  }

  setRefreshStatus(messageChartModels: Array<MessageChartModel>) {
    messageChartModels.forEach(messageChartModel => {
      if (this.currentChat.value.id === messageChartModel.chatId) {
        console.log('after');
        this.isRefreshRequired.next(this.isRefreshRequired.value + 1);
      }
    });
  }

  getRefreshStatus(): Observable<number> {
    return this.isRefreshRequired;
  }

  getUsers(): Observable<Array<User>> {
    return this.httpService.getUsers();
  }
}
