import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../app/model/user.model';
import { SubSink } from 'subsink';
import { UserService } from '../shared/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit, OnDestroy {
  eMail: string;
  invalidEmail = false;
  private subs = new SubSink();

  constructor(private router: Router,
    private userService: UserService) { }
  
  ngOnInit() {
  }

  onSignIn() {
    if (this.eMailAddressIsValid(this.eMail)) 
    {
      this.invalidEmail = false;
      this.subs.add(
        this.userService
        .login(this.eMail)
        .subscribe((user: User) => {
          this.userService.user = user;
          this.router.navigate(['home'], {queryParams: { id: user.id }});
        }));
    }
    else {
      this.invalidEmail = true;
    }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  eMailAddressIsValid(eMail: string): boolean {
    const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(eMail.toLowerCase());
  }
}
