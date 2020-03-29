import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { UserDTO } from '../userDTO';
import { Observable } from 'rxjs';
import { Message } from '../message';
import { User } from '../user';
import { MessageDTO } from '../messageDTO';
import { LoginResponse } from '../login-response';

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

  addUser(user: UserDTO): Observable<string> {
    return this.httpClient.post<string>(this.url + 'Users/register', user);
  }

  getAllFriend(user: User, token: string): Array<User> {
    return [{id: 1, email: 'email'}];
  }

  sendMessage(message: MessageDTO): Observable<string> {
    console.log(this.token);
    const headersLivechat = new HttpHeaders({
      'Authorization': 'Bearer ' + this.token,
    });
    return this.httpClient.post<string>(this.url + 'Messages/post', message, {headers: headersLivechat});
  }

  loadPreviousMessage(email: string, dateLastMessage: Date, token: string): Observable<Array<Message>> {
    const date: string = dateLastMessage.getFullYear + '-' +
    dateLastMessage.getMonth + '-' +
    dateLastMessage.getDate() + ' ' +
    dateLastMessage.getHours + ':' +
    dateLastMessage.getMinutes + ':' +
    dateLastMessage.getSeconds;

    const tokenHeaders: HttpHeaders = new HttpHeaders().set('token', token);
    const dateParam: HttpParams = new HttpParams().set('email', email).set('date', date);
    return this.httpClient.get<Array<Message>>(this.url + 'Messages/getAll', {headers: tokenHeaders, params: dateParam});
  }

  private isNullOrWhitespace(str: string): boolean {
    return str == null || str.length == 0;
  }
}
