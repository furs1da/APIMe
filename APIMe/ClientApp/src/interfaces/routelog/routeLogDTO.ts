export interface RouteLogDto {
  id: number;
  ipAddress: string;
  requestTimestamp: Date;
  fullName: string;
  httpMethod: string;
  tableName: string;
  recordId: number | null;
  routePath: string;
}
