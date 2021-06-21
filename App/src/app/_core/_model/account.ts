import { AccountGroup } from "./account.group";
import { AccountType } from "./account.type";

export interface Account {
  id: number;
  username: string;
  fullName: string;
  password: string;
  email: string;
  isLock: boolean;
  accountTypeId: number | null;
  accountGroupId: number | null;
  createdBy: number;
  modifiedBy: number | null;
  createdTime: string;
  modifiedTime: string | null;
  accountType: AccountType;
  accountGroup: AccountGroup;
}
