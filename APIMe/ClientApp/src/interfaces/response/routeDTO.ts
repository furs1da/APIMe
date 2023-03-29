export interface RouteDto {
  id: number;
  name: string;
  description: string;
  routeTypeId: number;
  dataTableName: string;
  isVisible: boolean;

  // RouteType properties
  routeTypeName: string;
  routeTypeResponseCode: string;

  routeTypeCrudActionName: string | null;
  routeTypeCrudActionId: number | null;

  // Records from uncertain tables
  records: Array<object>;
}
