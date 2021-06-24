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
  id: number;

}

export interface ToDoListL1L2 {
  type: string;
  objective: string;
  id: number;

}
export interface ToDoListByLevelL1L2Dto {
  id: number;
  objective: string;
  l0TargetList: string[];
  l0ActionList: string[];
  result1OfMonth: string;
  result2OfMonth: string;
  result3OfMonth: string;
}

export interface SelfScore {
  objectiveList: string[];
  resultOfMonth: string;
  month: number;
}
