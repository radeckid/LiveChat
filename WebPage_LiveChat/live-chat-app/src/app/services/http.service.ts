import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from '../user';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  constructor(private httpClient: HttpClient) { }

  userUrl = 'http://localhost:53064/Users/';

  getUser(user: User): Observable<string> {
    return this.httpClient.post<string>(this.userUrl + 'login', user);
  }

  addUser(user: User): Observable<string> {
    return this.httpClient.post<string>(this.userUrl + 'register', user);
  }
}
