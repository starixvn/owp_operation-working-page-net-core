const createDefaultValidationResult = () => {
  return {
    hasError: false,
    message: ''
  };
};

const acceptedFileFormat = file => {
  let validationResult = createDefaultValidationResult();

  if (!file.name.endsWith('.xlsx')) {
    validationResult = {
      ...validationResult,
      hasError: true,
      message: 'File extension not allowed!'
    };
  }
  return validationResult;
};

const noFileIsSelected = file => {
  let validationResult = createDefaultValidationResult();

  if (!file) {
    validationResult = {
      ...validationResult,
      hasError: true,
      message: 'No file was selected for uploading'
    };
  }
  return validationResult;
};

const fileExceed5Mb = file => {
  let validationResult = createDefaultValidationResult();

  if ((file.size / (1024 * 1024)) >= 5) {
    validationResult = {
      hasError: true,
      message: 'Sorry, your file exceeds 5MB'
    };
  }
  return validationResult;
};

export {
  noFileIsSelected,
  acceptedFileFormat,
  fileExceed5Mb
};
