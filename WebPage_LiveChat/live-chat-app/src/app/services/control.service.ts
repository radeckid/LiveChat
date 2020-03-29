import { Injectable } from '@angular/core';
import { Subject, Observable, BehaviorSubject } from 'rxjs';
import { HttpService } from './http.service';
import { User } from '../user';

@Injectable({
  providedIn: 'root'
})
export class ControlService {

  isUserLogged: Subject<boolean> = new Subject<boolean>();
  user: BehaviorSubject<User> = new BehaviorSubject<User>({email: '', id: 1});
  receiver: User = {email: 'test2@wp.pl', id: 2};

  public getReceiver(): User {
    return this.receiver;
  }

  setUser(user: User) {
    this.user.next(user);
    this.setLogged();
  }

  getUser(): Observable<User> {
    return this.user.asObservable();
  }

  setLogged() {
    this.isUserLogged.next(true);
  }

  getLogged(): Observable<boolean> {
    return this.isUserLogged.asObservable();
  }
}
