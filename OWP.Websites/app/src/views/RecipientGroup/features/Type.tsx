import { TicketSettingRecipient } from "../../Shared/TicketSetting/TypeNative";

interface RecipientModel extends TicketSettingRecipient {
  recipientGroupId: number
}

class GroupDataSource {
  groupId: number;
  groupName: string;
  recipients: RecipientModel[];

  constructor() {
    this.groupId = 0;
    this.groupName = '';
    this.recipients = [];
  }
}

export {
  RecipientModel,
  GroupDataSource
};
