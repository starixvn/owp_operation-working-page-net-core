type UserOptions = {
  userName: string;
  userId: number;
};

interface Option {
  value: string | number;
  label: string;
  key?: string;
  disabled?: boolean;
}

interface UserInfo {
  UserId: number;
  UserName: string;
  NickName?: string;
}

export { UserOptions, Option, UserInfo };
