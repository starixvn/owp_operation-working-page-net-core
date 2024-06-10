type UserOptions = {
  userName: string;
  userId: number;
};

interface Option {
  value: any;
  label: string;
  key?: string;
  disabled?: boolean;
}

export { UserOptions, Option };
