import { useSelector, useDispatch } from 'react-redux';
import { fireEvent, render } from '@testing-library/react';
import OtherAssignment from './OtherAssignment';
import Spinner from '../../../../../components/Spinner';
import { noop } from 'lodash';
import ToastrWrapper from '../../../../../components/ToastrWrapper';
import { validateAssignment } from './AssignmentHelper';
import { setImmediate } from 'timers';
import { act } from 'react-test-renderer';

jest.mock('axios');
jest.mock('./AssignmentHelper.jsx');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

jest.mock('../../../../../components/Spinner');
Spinner.show.mockImplementation(() => noop());
Spinner.hide.mockImplementation(() => noop());

jest.mock('../../../../../components/ToastrWrapper');
ToastrWrapper.showInfo.mockImplementation(() => noop());

const promise = Promise.resolve();

const mockedDispatch = jest.fn();
const handleShowHideIconPlusMock = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);

const setUp = props => {
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
};

describe('OtherAssignment under test', () => {
  beforeEach(() => {
    setUp();
  });
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should dispatch updatePrematch when blur on input, with validateResult = true', async () => {
    const responseData = {
      validateResult: true,
      valueAfterValidate: 'esther',
      valueBeforeValidate: 'esther'
    };
    validateAssignment.mockReturnValue(responseData);
    const props = {
      columnId: 'HDP',
      rowIndex: 0,
      dataSource: [
        {
          OtherHDP: 'sa'
        }
      ],
      handleShowHideIconPlus: handleShowHideIconPlusMock
    };

    const { container } = render(<OtherAssignment {...props} />);
    const input = container.querySelector('input');
    fireEvent.blur(input);
    await act(async () => {
      await promise;
    });

    expect(mockedDispatch).toBeCalledTimes(1);
    expect(mockedDispatch).toBeCalledWith({
      payload: {
        key: 'OtherHDP',
        newValue: 'esther',
        rowIndex: 0
      },
      type: 'dayShift/updatePrematch'
    });
    expect(handleShowHideIconPlusMock).toBeCalledTimes(1);
    expect(handleShowHideIconPlusMock).toBeCalledWith(true);
  });

  it('should not call dispatch when blur on input, with validateResult = false ', async () => {
    const responseData = {
      validateResult: false,
      valueAfterValidate: '',
      valueBeforeValidate: 'tsa'
    };
    validateAssignment.mockReturnValue(responseData);
    const props = {
      columnId: 'HDP',
      rowIndex: 0,
      dataSource: [
        {
          OtherHDP: 'sa'
        }
      ],
      handleShowHideIconPlus: handleShowHideIconPlusMock
    };

    const { container } = render(<OtherAssignment {...props} />);
    const input = container.querySelector('#input');
    fireEvent.blur(input);
    await act(async () => {
      await promise;
    });
    expect(mockedDispatch).not.toHaveBeenCalled();
  });

  it('should call back handleShowHidIconPlus(true) when valid value', () => {
    const props = {
      columnId: 'HDP',
      rowIndex: 0,
      dataSource: [
        {
          OtherHDP: 'abc'
        }
      ],
      handleShowHideIconPlus: handleShowHideIconPlusMock
    };

    const { container } = render(<OtherAssignment {...props} />);
    const input = container.querySelector('input');
    fireEvent.blur(input);
    expect(mockedDispatch).not.toHaveBeenCalled();
  });

  it('should  call dispatch when blur on input, with have not assignment ', async () => {
    setUp();
    const props = {
      columnId: 'HDP',
      rowIndex: 0,
      dataSource: [
        {
          OtherHDP: ''
        }
      ],
      handleShowHideIconPlus: handleShowHideIconPlusMock
    };

    const { container } = render(<OtherAssignment {...props} />);
    const input = container.querySelector('#input');
    fireEvent.blur(input);
    await act(async () => {
      await promise;
    });
    expect(mockedDispatch).toHaveBeenCalled();
  });

  it('alway render input tab', () => {
    setUp();
    const props = {
      columnId: 'HDP',
      rowIndex: 0,
      dataSource: [
        {
          OtherHDP: ''
        }
      ]
    };

    const { container } = render(<OtherAssignment {...props} />);
    expect(container.querySelector('#input')).toBeInTheDocument();
  });
});
