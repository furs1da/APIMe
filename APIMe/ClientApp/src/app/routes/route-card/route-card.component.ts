import { Component, Input, Output, EventEmitter } from '@angular/core';
import { RouteDto } from '../../../interfaces/response/routeDTO';

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

  constructor() { }

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
