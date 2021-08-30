import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/model/user.model';
import { SubSink } from 'subsink';
import { UserService } from '../shared/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit, OnDestroy {
  eMail: string;
  private subs = new SubSink();

  constructor(private router: Router,
    private userService: UserService) { }
  
  ngOnInit() {
  }

  onSignIn() {
    this.subs.add(
      this.userService
      .login(this.eMail)
      .subscribe((user: User) => {
        this.userService.user = user;
        this.router.navigate(['home'], {queryParams: { id: user.id }});
      }));
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }
}
