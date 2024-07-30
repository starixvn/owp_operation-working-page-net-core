type UploadRequest = {
  eventDate: string;
  isUploadFMAndAIO: boolean;
  file: File;
};

type ValidationResponse = {
  hasError: boolean;
  sessionId: string;
  recordCount: number;
  validationResults: Array<MessageResponse>;
  isSuccessful: boolean;
  exception: string;
};

type MessageResponse = {
  row: number;
  message: string;
};

type SubmitResponse = {
  isSuccessful: boolean;
  message: string;
};

export { UploadRequest, ValidationResponse, MessageResponse, SubmitResponse };
