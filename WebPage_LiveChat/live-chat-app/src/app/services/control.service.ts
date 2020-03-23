import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root'
})
export class ControlService {

  isUserLogged: Subject<boolean> = new Subject<boolean>();
  token: string;

  constructor() {
    this.token
   }

  setToken(token: string) {
    if (!this.isNullOrWhitespace(token)) {
      this.token = token;
      this.setLogged();
    } else {
      this.token = '';
    }
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
