type TGetTemplate = {
  key: number;
};

type TUploadProviderFile = {
  key: number;
};

type TReview = {
  key: number;
};

type ValidationResponse = {
  isSuccess: boolean;
  value: {
    originalRecordCount: number;
    sessionId: string;
  };
  errors
  : {
    description: string;
    row: string;
  }[];
};

export { TGetTemplate, TUploadProviderFile, TReview, ValidationResponse };