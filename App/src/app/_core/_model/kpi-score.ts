export interface KPIScore {
  id: number;
  period: number;
  point: number;
  periodTypeId: number;
  accountId: number;
  scoreBy: number;
  createdTime: string;
  modifiedTime: string | null;
  periodTypeCode: string;
}
