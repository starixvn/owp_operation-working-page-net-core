type TGetTemplate = {
  key: number;
};

type TUploadProviderFile = {
  key: number;
};

type TReview = {
  key: number;
};

type UploadRequest = {
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