import { validateFile } from './validationHelper';

describe('ValidationHelper under test', () => {
  it('file is empty should return no file message', () => {
    const result = validateFile();

    expect(result).toEqual('No file was selected for uploading');
  });

  it('wrong format file, should return wrong format message', () => {
    const file = new File([], 'name');
    const result = validateFile(file);

    expect(result).toEqual('Uploaded file is not an excel file');
  });

  it('correct format file, should return empty', () => {
    const file = new File([], 'name.xlsx');
    const result = validateFile(file);

    expect(result).toEqual('');
  });

  it('size > 5MB, should return error message', () => {
    const file = new File([new ArrayBuffer(6000000)], 'name.xlsx');
    const result = validateFile(file);

    expect(result).toEqual(
      'The file size can not exceed 5 MB. Please try another one'
    );
  });
});
