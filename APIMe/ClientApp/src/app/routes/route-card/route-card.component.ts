import { Component, Input, Output, EventEmitter } from '@angular/core';
import { RouteDto } from '../../../interfaces/response/routeDTO';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-route-card',
  templateUrl: './route-card.component.html',
  styleUrls: ['./route-card.component.css']
})
export class RouteCardComponent {
  @Input() route!: RouteDto;
  @Output() edit = new EventEmitter<RouteDto>();
  @Output() delete = new EventEmitter<RouteDto>();
  @Output() test = new EventEmitter<RouteDto>(); 
  isAdmin: boolean = false;
  public isUserAuthenticated: boolean = false;

  constructor(private authService: AuthenticationService, private router: Router) {
    this.authService.authChanged
      .subscribe(res => {
        this.isUserAuthenticated = res;
      })

    this.authService.authChanged.subscribe(() => {
      this.isAdmin = this.authService.isUserAdmin();
    });
  }

  editRoute(): void {
    this.edit.emit(this.route);
  }

  deleteRoute(): void {
    this.delete.emit(this.route);
  }

  onTest(): void {
    this.test.emit(this.route); 
  }
}
