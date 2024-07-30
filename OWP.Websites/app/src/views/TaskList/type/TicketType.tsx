type CreatorInfo = {
  groupName: null | string;
  nickName: null | string;
  recipientId: number;
  recipientType: number;
  userName: string;
};

type Permission = {
  HasWrite: boolean;
  HasRead: boolean;
  ActorId: number;
  Username: string;
};

export { CreatorInfo, Permission };
