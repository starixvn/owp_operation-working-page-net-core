const SIZE_FILE_MESSAGE =
  'The file size can not exceed 5 MB. Please try another one';
const FORMAT_FILE_MESSAGE = 'Uploaded file is not an excel file';
const NO_FILE_MESSAGE = 'No file was selected for uploading';

const isAcceptedFileFormat = (file: File) => {
  const validExts = ['.xlsx', '.xls', '.xlsm'];

  return validExts.some(x => file.name.endsWith(x));
};

const isFileExceed5Mb = (file: File) => {
  const BYTES = 1024;
  const MB_MAX = 5;

  return file.size / (BYTES * BYTES) >= MB_MAX;
};

const validateFile = (file?: File) => {
  if (!file) {
    return NO_FILE_MESSAGE;
  }

  if (!isAcceptedFileFormat(file)) {
    return FORMAT_FILE_MESSAGE;
  }

  if (isFileExceed5Mb(file)) {
    return SIZE_FILE_MESSAGE;
  }

  return '';
};

export { validateFile };
