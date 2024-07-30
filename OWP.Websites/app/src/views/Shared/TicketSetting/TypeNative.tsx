type TicketSettingRecipient = {
  recipientId: number;
  recipientType: number;
  recipientName: string;
  recipientNickname: string | undefined;
};

type Permission = {
  SiteId: number
};

export {
  TicketSettingRecipient,
  Permission
};
