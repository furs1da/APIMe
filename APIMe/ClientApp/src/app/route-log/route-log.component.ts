import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { RepositoryService } from '../shared/services/repository.service';
import { RouteLogDto } from '../../interfaces/routelog/routeLogDTO';
import { RouteLogInfoDialogComponent } from './route-log-info-dialog/route-log-info-dialog.component';

import * as ExcelJS from 'exceljs';

@Component({
  selector: 'app-route-log',
  templateUrl: './route-log.component.html',
  styleUrls: ['./route-log.component.css']
})
export class RouteLogComponent implements OnInit {
  routeLogs: RouteLogDto[] = [];
  dataSource: MatTableDataSource<RouteLogDto>;
  displayedColumns = ['fullName', 'httpMethod', 'tableName', 'requestTimestamp', 'actions'];
  filterForm: FormGroup;

  @ViewChild(MatSort, { static: true })
  sort!: MatSort;

  constructor(private repository: RepositoryService, public dialog: MatDialog, private fb: FormBuilder) {
    this.dataSource = new MatTableDataSource<RouteLogDto>();
    this.filterForm = this.fb.group({
      fullName: new FormControl(''),
      httpMethod: new FormControl(null),
      tableName: new FormControl(null),
      requestTimestamp: new FormControl(null),
    });
  }

  ngOnInit(): void {
    this.getRouteLogs();
    this.dataSource.sort = this.sort;
    this.filterForm.valueChanges.subscribe(() => {
      this.applyFilters();
    });
  }

  applyFilters(): void {
    const filterObj = {
      fullName: this.filterForm.get('fullName')?.value?.toLowerCase() || '',
      httpMethod: this.filterForm.get('httpMethod')?.value,
      tableName: this.filterForm.get('tableName')?.value?.toLowerCase() || '',
      requestTimestamp: this.filterForm.get('requestTimestamp')?.value,
    };

    this.dataSource.data = this.routeLogs.filter((routeLog) => {
      const isInFullName = routeLog.fullName.toLowerCase().includes(filterObj.fullName);
      const isInHttpMethod = filterObj.httpMethod ? routeLog.httpMethod === filterObj.httpMethod : true;
      const isInTableName = routeLog.tableName.toLowerCase().includes(filterObj.tableName);

      const isInRequestTimestamp = filterObj.requestTimestamp
        ? this.isSameDate(new Date(routeLog.requestTimestamp), new Date(filterObj.requestTimestamp))
        : true;

      return isInFullName && isInHttpMethod && isInTableName && isInRequestTimestamp;
    });
  }


  isSameDate(date1: Date, date2: Date): boolean {
    const sameYear = date1.getFullYear() === date2.getFullYear();
    const sameMonth = date1.getMonth() === date2.getMonth();
    const sameDate = date1.getDate() === date2.getDate();

    return sameYear && sameMonth && sameDate;
  }

  getRouteLogs(): void {
    this.repository.getAllRouteLogs().subscribe((routeLogs) => {
      this.routeLogs = routeLogs;
      this.dataSource.data = routeLogs;
    });
  }

  openInfoDialog(routeLog: RouteLogDto): void {
    this.dialog.open(RouteLogInfoDialogComponent, {
      width: '500px',
      data: { routeLog },
    });
  }

  exportToExcel() {
    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet('RouteLogs');
    this.styleWorksheet(worksheet, 'FF2196F3', 'FFFFFFFF', 'FF000000');  // Apply styles to header row


  worksheet.columns = [
    { key: 'id', header: 'ID', width: 10 },
    { key: 'ipAddress', header: 'IP Address', width: 15 },
    { key: 'requestTimestamp', header: 'Request Timestamp', width: 20 },
    { key: 'fullName', header: 'Full Name', width: 20 },
    { key: 'httpMethod', header: 'HTTP Method', width: 12 },
    { key: 'tableName', header: 'Table Name', width: 20 },
    { key: 'recordId', header: 'Record ID', width: 10 },
    { key: 'routePath', header: 'Route Path', width: 30 },
  ];
    
    this.styleWorksheet(worksheet, 'FFFFFFFF', 'FF000000', 'FF000000', 0);

  this.routeLogs.forEach(routeLog => {
    const row = worksheet.addRow({
      id: routeLog.id,
      ipAddress: routeLog.ipAddress,
      requestTimestamp: new Date(routeLog.requestTimestamp).toLocaleString(),
      fullName: routeLog.fullName,
      httpMethod: routeLog.httpMethod,
      tableName: routeLog.tableName,
      recordId: routeLog.recordId,
      routePath: routeLog.routePath,
    });
    this.styleWorksheet(worksheet, 'FFFFFFFF', 'FF000000', 'FF000000', row.number);
  });

  workbook.xlsx.writeBuffer().then(buffer => {
    const blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'RouteLogs.xlsx';
    a.click();
    window.URL.revokeObjectURL(url);
  });
}

  styleWorksheet(
    worksheet: ExcelJS.Worksheet,
    fillColor: string,
    fontColor: string,
    borderColor: string,
    rowNum: number = 1
  ) {
    const row = worksheet.getRow(rowNum);
    row.eachCell(cell => {
      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: fillColor },
      };
      cell.font = {
        color: { argb: fontColor },
      };
      cell.border = {
        top: { style: 'thin', color: { argb: borderColor } },
        bottom: { style: 'thin', color: { argb: borderColor } },
        left: { style: 'thin', color: { argb: borderColor } },
        right: { style: 'thin', color: { argb: borderColor } },
      };
    });
  }


}
