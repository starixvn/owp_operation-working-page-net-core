import { useSelector, useDispatch } from 'react-redux';
import { render } from '@testing-library/react';
import ReadOnlyAssignmentCell from './ReadOnlyAssignmentCell';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockedDispatch = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);

const setUp = props => {
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
};

describe('ReadOnlyAssignmentCell under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render cell assignment readonly', () => {
    setUp();
    const props = {
      columnId: 'HDP',
      rowIndex: 0,
      dataSource: [
        {
          HDP: 'esther'
        }
      ]
    };

    const { container } = render(<ReadOnlyAssignmentCell {...props} />);
    expect(container.querySelector('.assignment-readonly')).toBeInTheDocument();
  });

  it('should render cell other assignment readonly', () => {
    setUp();
    const props = {
      columnId: 'HDP',
      rowIndex: 0,
      dataSource: [
        {
          OtherHDP: 'esther test note'
        }
      ]
    };

    const { container } = render(<ReadOnlyAssignmentCell {...props} />);
    expect(
      container.querySelector('.show-other-assignment')
    ).toBeInTheDocument();
  });
});
