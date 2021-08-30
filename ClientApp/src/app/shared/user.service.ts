import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../model/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = 'https://localhost:44371/api/User/';
  user: User;

  constructor(private http: HttpClient) { }

  login(eMail: string): Observable<User> {
    return this.http.get(`${this.baseUrl}${encodeURIComponent(eMail)}`)
      .pipe(map((response: any) => {
        const user: User = response;
        return user;
      }));
  }

  getUser(id: string): Observable<User> {
    return this.http.get(`${this.baseUrl}GetUser/${id}`)
      .pipe(map((response: any) => {
        const user: User = response;
        return user;
      }));
  }
}
