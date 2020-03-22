import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ControlService {

  isUserLogged: boolean;
  token: string;

  constructor() { }
}
