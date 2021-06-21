export interface ResultOfMonth {
  id: number;
  title: string;
  month: number;
  createdBy: number;
  modifiedBy: number | null;
  createdTime: string;
  modifiedTime: string | null;
}
export interface ResultOfMonthRequest {
  id: number;
  title: string;
}
