export interface Contribution {
  id: number;
  content: string;
  createdBy: number;
  accountId: number;
  periodTypeId: number;
  period: number;
  modifiedBy: number | null;
  createdTime: string;
  modifiedTime: string | null;
}
