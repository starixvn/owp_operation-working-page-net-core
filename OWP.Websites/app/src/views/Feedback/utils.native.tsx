import { RateIconEnum, TFeedback } from "./type";

const validateInfoCreateFeedback = (info: TFeedback) => {
  const { feedBackInfo } = info;
  const { functionName, inputMessage, selectedFile, selectedImage, rate } = feedBackInfo;
  return (!functionName || (!inputMessage && !selectedFile && !selectedImage) || rate === RateIconEnum.DEFAULT)
    ? {
      valid: false,
      message: 'Please input required field'
    }
    : {
      valid: true,
      message: ''
    };
};

export {
  validateInfoCreateFeedback
};
