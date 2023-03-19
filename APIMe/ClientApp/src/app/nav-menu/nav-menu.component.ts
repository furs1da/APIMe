import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../shared/services/authentication.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded: boolean = false;
  public isUserAuthenticated: boolean = false;
  isAdmin: boolean = false;

  constructor(private authService: AuthenticationService, private router: Router) {
    this.authService.authChanged
      .subscribe(res => {
        this.isUserAuthenticated = res;
      })

    this.authService.authChanged.subscribe(() => {
      this.isAdmin = this.authService.isUserAdmin();
    });
  }

  ngOnInit(): void {
    this.authService.authChanged
      .subscribe(res => {
        this.isUserAuthenticated = res;
      })

    this.isAdmin = this.authService.isUserAdmin();
    this.authService.authChanged.subscribe(() => {
      this.isAdmin = this.authService.isUserAdmin();
    });
  }

  public logout = () => {
    this.authService.logout();
    this.router.navigate(["/"]);
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
