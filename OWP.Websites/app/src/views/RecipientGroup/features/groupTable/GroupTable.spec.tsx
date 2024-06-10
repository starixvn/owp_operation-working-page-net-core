import { fireEvent, render, screen } from '@testing-library/react';
import { useDispatch, useSelector } from 'react-redux';
import GroupTable from './GroupTable';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useEffect: jest.fn()
}));

const mockDispatch = jest.fn();

const setUp = (loading: string) => {
  (useDispatch as jest.Mock).mockReturnValue(mockDispatch);
  (useSelector as jest.Mock).mockImplementation(selectorFn =>
    selectorFn({
      recipientGroup: {
        groups: [
          {
            groupId: 1,
            groupName: 'LST',
            recipients: [1, 2, 3]
          }
        ],
        loading
      },
      policy: { permission: { Username: 'sa', UserId: 8, HasWrite: true } }
    })
  );
};

describe('test GroupTable', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('loading=pending, should render Loading and call dispatch', () => {
    setUp('pending');
    const { container } = render(<GroupTable />);

    expect(
      container.querySelector(`[class="icon-loading"]`)
    ).toBeInTheDocument();
  });

  it('loading=idle, should render Loading and call dispatch 1 time', () => {
    setUp('idle');
    const { container } = render(<GroupTable />);
    expect(screen.getByText('Group Name')).toBeInTheDocument();
  });
});
