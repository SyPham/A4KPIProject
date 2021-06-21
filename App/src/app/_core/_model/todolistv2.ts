export interface ToDoList {
  id: number;
  yourObjective: string;
  action: string;
  remark: string;
  progressId: number | null;
  objectiveId: number;
  createdBy: number;
  modifiedBy: number | null;
  createdTime: string;
  modifiedTime: string | null;
}
export interface ToDoListOfQuarter {
  yourObjective: string;
  resultOfMonth: string;
  resultOfMonthId: number;
  month: number;

}
