import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RouteLogDto } from '../../../interfaces/routelog/routeLogDTO';


@Component({
  selector: 'app-route-log-info-dialog',
  templateUrl: './route-log-info-dialog.component.html',
  styleUrls: ['./route-log-info-dialog.component.css'],
})
export class RouteLogInfoDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<RouteLogInfoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { routeLog: RouteLogDto }
  ) { }
}
