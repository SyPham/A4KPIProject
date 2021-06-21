export interface AttitudeScore {
  id: number;
  periodType: string;
  period: number;
  point: number;
  objectiveId: number;
  scoreBy: number;
  createdTime: string;
  modifiedTime: string | null;
}
