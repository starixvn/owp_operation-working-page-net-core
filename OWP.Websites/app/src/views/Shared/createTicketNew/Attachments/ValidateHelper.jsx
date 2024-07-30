import {
  validateFileExtension,
  validateFileSize
} from '../../chatBoardNew/helper/chatBoardHelper';

const ValidateHelper = () => {
  const errorList = [];
  let isValid = true;

  const validateNumberOfFile = listFile => {
    if (listFile.length > 5) {
      errorList.push('You have reached a maximum of 5 attachments');
      isValid = false;
    }
  };
  const validateExtension = listFile => {
    listFile.forEach(file => {
      const result = validateFileExtension(file.name);
      if (!result) {
        errorList.push(`File extension not allowed!`);
        isValid = result;
      }
    });
  };

  const validateSize = listFile => {
    listFile.forEach(file => {
      const result = validateFileSize(file.size);
      if (!result) {
        errorList.push(
          `The file's size should not larger than 5MB. Try another one`
        );
        isValid = result;
      }
    });
  };

  const validatePipe = (listFile, selectedFilesLength) => {
    if (!listFile.length) {
      return {
        isValid: false,
        errorList: ['File not null']
      };
    }
    if (selectedFilesLength > 5) {
      return {
        isValid: false,
        errorList: ['You have reached a maximum of 5 attachments']
      };
    }
    validateNumberOfFile(listFile);
    validateExtension(listFile);
    validateSize(listFile);
    return {
      isValid,
      errorList
    };
  };
  return { validatePipe };
};

export default ValidateHelper;
