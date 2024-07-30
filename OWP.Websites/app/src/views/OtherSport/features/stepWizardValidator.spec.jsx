import {
  noFileIsSelected,
  acceptedFileFormat,
  fileExceed5Mb
} from './stepWizardValidator';

describe('stepWizardValidator under test', () => {
  it('should retrun error when no file is selected', () => {
    const actual = noFileIsSelected(undefined);
    expect(actual).toEqual({
      hasError: true,
      message: 'No file was selected for uploading'
    });
  });

  it('should retrun no error when a file is selected', () => {
    const file = new File([new ArrayBuffer(6000000)], 'file.xlsx');
    const actual = noFileIsSelected(file);
    expect(actual).toEqual({
      hasError: false,
      message: ''
    });
  });

  it('should retrun error when format file incorrect', () => {
    const file = new File([new ArrayBuffer(6000000)], 'file.png');
    const actual = acceptedFileFormat(file);
    expect(actual).toEqual({
      hasError: true,
      message: 'File extension not allowed!'
    });
  });

  it('should retrun no error when format file is accepted', () => {
    const file = new File([new ArrayBuffer(6000000)], 'file.xlsx');
    const actual = acceptedFileFormat(file);
    expect(actual).toEqual({
      hasError: false,
      message: ''
    });
  });

  it('should retrun error when file size exceeds maximum 5mb', () => {
    const file = new File([new ArrayBuffer(6000000)], 'file.png');
    const actual = fileExceed5Mb(file);
    expect(actual).toEqual({
      hasError: true,
      message: 'Sorry, your file exceeds 5MB'
    });
  });

  it('should retrun no error when file size smaller 5mb', () => {
    const file = new File([new ArrayBuffer(600)], 'file.xlsx');
    const actual = fileExceed5Mb(file);
    expect(actual).toEqual({
      hasError: false,
      message: ''
    });
  });
});
