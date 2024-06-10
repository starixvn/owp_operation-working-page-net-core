enum RateIconEnum {
  DEFAULT = 'default',
  ANGRY = 'Angry',
  SAD = 'Sad',
  NORMAL = 'Normal',
  GOOD = 'Happy',
  AMAZING = 'Love'
}

enum StatusEnum {
  FEED_BACK = 'FEED_BACK',
  THANK_YOU = 'THANK_YOU'
}

type TFeedback = {
  feedBackInfo: {
    functionName: Required<string>;
    rate: RateIconEnum;
    inputMessage: string;
    selectedImage: File | null;
    selectedFile: File | null;
  };
  statusForm: StatusEnum;
  setRateStatus: (status: RateIconEnum) => void;
  setStatusForm: (status: StatusEnum) => void;
  setFunctionName: (name: string) => void;
  postFeedback: () => Promise<void>;
  validateBeforeCreate: () => ({
    valid: boolean,
    message: string
  });
  updateFeedbackInfo: (key: string, value: string | File | null) => void;
};

interface SelectOption {
  value: number;
  label: string;
}

export { TFeedback, RateIconEnum, StatusEnum, SelectOption };
