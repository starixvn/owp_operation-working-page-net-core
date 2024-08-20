type TSubTypeRow={
  typeId: number,
  subTypeId: number|null,
  subTypeName: string,
  isRead: boolean
}

type TReactIcon={
  messageId: number,
  iconKey: string,
  reactUserId: number,
  reactUserName: string,
  reactAvatarName: string,
}

type TMessageDetail={
  ticketId: number,
  messageId: number,
  message: string,
  userId: number,
  username: string,
  avatarFileName: string,
  extension: string,
  fileId: number|null,
  isSaved: boolean,
  isFile: boolean,
  imageFileName: string,
  createdDate: Date,
  quoteMessage: string,
  quoteUsername: string,
  quoteMessageId: number,
  quoteCreatedDate: Date,
  messageReactIcons:Array<TReactIcon>
}
export {
  TSubTypeRow,
  TMessageDetail,
  TReactIcon
};
