export interface Comment {
  id: number;
  content: string;
  createdBy: number;
  objectiveId: number;
  modifiedBy: number | null;
  createdTime: string;
  modifiedTime: string | null;
}
