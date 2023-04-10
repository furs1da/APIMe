import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { RepositoryService } from '../shared/services/repository.service';
import { RouteLogDto } from '../../interfaces/routelog/routeLogDTO';
import { RouteLogInfoDialogComponent } from './route-log-info-dialog/route-log-info-dialog.component';


@Component({
  selector: 'app-route-log',
  templateUrl: './route-log.component.html',
  styleUrls: ['./route-log.component.css']
})
export class RouteLogComponent implements OnInit {
  routeLogs: RouteLogDto[] = [];
  filteredRouteLogs: RouteLogDto[] = [];
  displayedColumns = ['fullName', 'httpMethod', 'tableName', 'requestTimestamp', 'actions'];
  filterForm: FormGroup;

  constructor(private repository: RepositoryService, public dialog: MatDialog, private fb: FormBuilder) {
    this.filterForm = this.fb.group({
      fullName: new FormControl(''),
      httpMethod: new FormControl(null),
      tableName: new FormControl(null),
      requestTimestamp: new FormControl(null),
    });
  }

  ngOnInit(): void {
    this.getRouteLogs();
    this.filterForm.valueChanges.subscribe(() => {
      this.filterRouteLogs();
    });
  }

  getRouteLogs(): void {
    this.repository.getAllRouteLogs().subscribe((routeLogs) => {
      this.routeLogs = routeLogs;
      this.filteredRouteLogs = routeLogs;
    });
  }

  filterRouteLogs(): void {
    const filter = {
      fullName: this.filterForm.get('fullName')?.value.toLowerCase() || '',
      httpMethod: this.filterForm.get('httpMethod')?.value,
      tableName: this.filterForm.get('tableName')?.value,
      requestTimestamp: this.filterForm.get('requestTimestamp')?.value,
    };

    this.filteredRouteLogs = this.routeLogs.filter((routeLog) => {
      const isInFullName = routeLog.fullName.toLowerCase().includes(filter.fullName);
      const isInHttpMethod = filter.httpMethod ? routeLog.httpMethod === filter.httpMethod : true;
      const isInTableName = routeLog.tableName.toLowerCase().includes(filter.tableName);


      const isInRequestTimestamp = filter.requestTimestamp
        ? this.isSameDate(new Date(routeLog.requestTimestamp), new Date(filter.requestTimestamp))
        : true;

      return isInFullName && isInHttpMethod && isInTableName && isInRequestTimestamp;
    });
  }

  isSameDate(date1: Date, date2: Date): boolean {
    return (
      date1.getFullYear() === date2.getFullYear() &&
      date1.getMonth() === date2.getMonth() &&
      date1.getDate() === date2.getDate()
    );
  }

  openInfoDialog(routeLog: RouteLogDto): void {
    this.dialog.open(RouteLogInfoDialogComponent, {
      width: '500px',
      data: { routeLog },
    });
  }


}
