import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { HttpService } from './http.service';
import { User } from '../user';

@Injectable({
  providedIn: 'root'
})
export class ControlService {

  isUserLogged: Subject<boolean> = new Subject<boolean>();
  user: User;
  token: string;

  setToken(token: string) {
    if (!this.isNullOrWhitespace(token)) {
      this.token = token;
      this.setLogged();
    } else {
      console.log('error with token');
    }
  }

  setUser(user: User) {
    this.user = user;
  }

  setLogged() {
    this.isUserLogged.next(true);
  }

  getLogged(): Observable<boolean> {
    return this.isUserLogged.asObservable();
  }

  private isNullOrWhitespace(str: string): boolean {
    return str != null || str.length == 0;
  }
}
