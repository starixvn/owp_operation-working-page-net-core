import { fireEvent, render, screen } from '@testing-library/react';
import SubTypeTable from './SubTypeTable';
import SubTypeRow from './SubTypeRow';

jest.mock('./subtypeRow', () => {
  return jest.fn(() => null);
});

describe('SubTypeTable under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should render 2 rows', () => {
    const subtypes = [
      { typeId: 1, subtypeId: 0, subtypeName: 'sa1' },
      { typeId: 1, subtypeId: 61, subtypeName: 'sa2' }
    ];
    const onUpdateSubtype = jest.fn();
    const handleSelectAnotherSubtype = jest.fn();
    const props = {
      subtypes,
      onUpdateSubtype,
      handleSelectAnotherSubtype
    };

    render(<SubTypeTable {...props} />);

    expect(SubTypeRow).toBeCalledTimes(2);
  });
  it('should render 0 rows', () => {
    const subtypes = [];
    const onUpdateSubtype = jest.fn();
    const handleSelectAnotherSubtype = jest.fn();
    const props = {
      subtypes,
      onUpdateSubtype,
      handleSelectAnotherSubtype
    };

    render(<SubTypeTable {...props} />);

    expect(SubTypeRow).toBeCalledTimes(0);
  });
});
