import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { render } from '@testing-library/react';
import EditableAssignmentCell from './EditableAssignmentCell';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useEffect: jest.fn()
}));

const mockedEffect = jest.fn();
useEffect.mockImplementationOnce(mockedEffect);
const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);
const setUp = () => {
  useSelector.mockImplementation(selectorFn =>
    selectorFn({
      copy: {
        highlightedCell: {
          PreMatchLeagueId: 4,
          isTakeOver: true,
          isCopying: true,
          copiedValue: {
            OU: 'sa',
            HDP: 'sa'
          }
        }
      }
    })
  );
};

const createProps = (otherHDP, isInfoSiteId) => {
  const dataSource = [
    {
      PreMatchLeagueId: 123,
      InfoSiteMatchId: isInfoSiteId,
      OtherHDP: otherHDP
    }
  ];
  const rowIndex = 0;
  const columnId = 'HDP';
  return { dataSource, rowIndex, columnId };
};

describe('EditableAssignmentCell under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('when otherAssignment change, should execute useEffect', () => {
    setUp();
    const props = createProps('esther test');
    render(<EditableAssignmentCell {...props} />);
    expect(mockedEffect).toHaveBeenCalledTimes(1);
  });

  it('always render MainAssignment', () => {
    setUp();
    const props = createProps();
    const { container } = render(<EditableAssignmentCell {...props} />);
    expect(container.querySelector('.edit-assignment')).toBeInTheDocument();
    expect(container.querySelector('.other-assignment')).toBeNull();
  });

  it('when isShowInputOtherAssignment = true, render OtherAssignment', () => {
    setUp();
    const props = createProps('esther test');
    const { container } = render(<EditableAssignmentCell {...props} />);
    expect(container.querySelector('.other-assignment')).toBeInTheDocument();
  });
});
