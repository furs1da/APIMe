export interface RouteTypeDto {
  id: number;
  name: string;
  responseCode: string;
  crudActionName: string | null;
  crudId: number | null;
}
