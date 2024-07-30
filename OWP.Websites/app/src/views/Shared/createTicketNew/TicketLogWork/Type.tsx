type SubType = {
  subTypeId: number;
  subTypeName: string;
};

type SubTypeOption = {
  value: number | string;
  label: string;
  key?: string;
  disabled?: boolean;
};

type StatusOption = {
  value: string;
  label: string;
};

type TRecipient = {
  groupName: string | null;
  nickName: string;
  recipientId: number;
  recipientType: number;
  userName: string | null;
};

type LogWorkProps = {
  creatorInfo: TRecipient;
};

type MatchInfo = {
  leagueName: string;
  awayName: string;
  homeName: string;
  koTime: Date;
};
export {
  MatchInfo,
  LogWorkProps,
  TRecipient,
  StatusOption,
  SubTypeOption,
  SubType
};
