import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { UserDTO } from '../userDTO';
import { Observable, EMPTY } from 'rxjs';
import { Message } from '../message';
import { User } from '../user';
import { MessageDTO } from '../messageDTO';
import { LoginResponse } from '../login-response';
import { Chat } from '../chat';
import { retry, catchError } from 'rxjs/operators';
import { UserNotification } from '../user-notification';
import { ActionNotification } from '../action-notification';
import { RelationDeletion } from '../relation-deletion';
import { RelationAdding } from '../relation-adding';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  token: string;

  constructor(private httpClient: HttpClient) { }

  setToken(token: string) {
    if (this.isNullOrWhitespace(token)) {
      console.log('error with token');
    } else {
      this.token = token;
    }
  }

  url = 'http://localhost:53064/';

  getUser(user: UserDTO): Observable<LoginResponse> {
    return this.httpClient.post<LoginResponse>(this.url + 'Users/login', user);
  }

  getUsers(): Observable<Array<User>> {
    return this.httpClient.get<Array<User>>(this.url + 'Users/getAll');
  }

  addUser(user: UserDTO): Observable<string> {
    return this.httpClient.post<string>(this.url + 'Users/register', user);
  }

  getAllFriend(userId: number): Observable<Array<User>> {
    const headersLivechat = new HttpHeaders({
      'Authorization': 'Bearer ' + this.token,
    });

    return this.httpClient.get<Array<User>>(this.url + 'Users/friends/' + userId, {headers: headersLivechat});
  }

  sendMessage(message: MessageDTO): Observable<string> {
    const headersLivechat = new HttpHeaders({
      'Authorization': 'Bearer ' + this.token,
    });
    return this.httpClient.post<string>(this.url + 'Messages/post', message, {headers: headersLivechat});
  }

  getAllMessages(userId: number, chatId: number): Observable<Array<Message>> {
    if (chatId === 0) {
      return EMPTY;
    }
    const headersLivechat = new HttpHeaders({
      'Authorization': 'Bearer ' + this.token,
    });

    return this.httpClient.get<Array<Message>>(this.url + 'Messages/getAll/' + userId + '/' + chatId, {headers: headersLivechat});
  }

  getChats(userId: number): Observable<Array<Chat>> {
    const headersLivechat = new HttpHeaders({
      'Authorization': 'Bearer ' + this.token,
    });

    return this.httpClient.get<Array<Chat>>(this.url + "Chat/getAll/" + userId, {headers: headersLivechat});
  }

  getUserNotifications(userId: number): Observable<Array<UserNotification>> {
    const headersLivechat = new HttpHeaders({
      'Authorization': 'Bearer ' + this.token,
    });

    return this.httpClient.get<Array<UserNotification>>(this.url + "Notification/getAllReceived/" + userId, {headers: headersLivechat});
  }

  notificationProcess(actionNotification: ActionNotification): Observable<any> {
    const headersLivechat = new HttpHeaders({
      'Authorization': 'Bearer ' + this.token,
    });

    return this.httpClient.post(this.url + 'Notification/proccess', actionNotification, {headers: headersLivechat,  responseType: 'text'});
  }

  deleteRelation(relationDeletion: RelationDeletion): Observable<any> {
    const headersLivechat = new HttpHeaders({
      'Authorization': 'Bearer ' + this.token,
    });

    // tslint:disable-next-line: max-line-length
    return this.httpClient.post(this.url + 'Notification/deleteRelation', relationDeletion, {headers: headersLivechat,  responseType: 'text'});
  }

  addingRelation(relationAdding: RelationAdding): Observable<any> {
    const headersLivechat = new HttpHeaders({
      'Authorization': 'Bearer ' + this.token,
    });

    // tslint:disable-next-line: max-line-length
    return this.httpClient.post(this.url + 'Notification/invitation', relationAdding, {headers: headersLivechat,  responseType: 'text'});
  }

  private isNullOrWhitespace(str: string): boolean {
    return str == null || str.length == 0;
  }
}
