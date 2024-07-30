import ValidateHelper from './ValidateHelper';

describe('ValidateHelper under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('case check file type, should handle validate file, return error list', () => {
    const state = {
      files: [{ name: 'abc.txt', size: 5000 }]
    };
    const { validatePipe } = ValidateHelper();
    const { isValid, errorList } = validatePipe(
      state.files,
      state.files.length + 1
    );

    expect(isValid).toEqual(false);
    expect(errorList).toEqual(['File extension not allowed!']);
  });

  it('case check file size, should handle validate file, return error list', () => {
    const state = {
      files: [{ name: 'abc.doc', size: 70000000000 }]
    };
    const { validatePipe } = ValidateHelper();
    const { isValid, errorList } = validatePipe(
      state.files,
      state.files.length + 1
    );

    expect(isValid).toEqual(false);
    expect(errorList).toEqual([`The file's size should not larger than 5MB. Try another one`]);
  });

  it('case check selectedFile > 5, should handle validate file, return error list', () => {
    const state = {
      files: [{ name: 'abc.doc', size: 70000000000 }]
    };
    const { validatePipe } = ValidateHelper();
    const { isValid, errorList } = validatePipe(
      state.files,
      state.files.length + 5
    );

    expect(isValid).toEqual(false);
    expect(errorList).toEqual(['You have reached a maximum of 5 attachments']);
  });
  it('case check chose > 5 file, should handle validate file, return error list', () => {
    const state = {
      files: [
        { name: 'abc.doc', size: 5 },
        { name: 'abc.doc', size: 6 },
        { name: 'abc.doc', size: 6 },
        { name: 'abc.doc', size: 7 },
        { name: 'abc.doc', size: 123 },
        { name: 'abc.doc', size: 123 },
        { name: 'abc.doc', size: 455 }
      ]
    };
    const { validatePipe } = ValidateHelper();
    const { isValid, errorList } = validatePipe(state.files, 1);

    expect(isValid).toEqual(false);
    expect(errorList).toEqual(['You have reached a maximum of 5 attachments']);
  });

  it('case check file null, should handle validate file, return error list', () => {
    const state = {
      files: []
    };
    const { validatePipe } = ValidateHelper();
    const { isValid, errorList } = validatePipe(
      state.files,
      state.files.length + 5
    );

    expect(isValid).toEqual(false);
    expect(errorList).toEqual(['File not null']);
  });
});
