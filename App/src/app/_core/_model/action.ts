export interface Action {
  id: number;
  target: string;
  content: string;
  deadline: string | null;
  accountId: number;
  kPIId: number;
  statusId: number;
  createdTime: string;
  modifiedTime: string | null;
}
