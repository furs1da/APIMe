export interface TestRouteResponse {
  statusCode: number;
  message: string;
  records: Array<object> | null;
}
