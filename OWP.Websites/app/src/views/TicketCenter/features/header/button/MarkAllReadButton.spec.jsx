import { render, fireEvent, screen } from '@testing-library/react';
import { useDispatch } from 'react-redux';
import MarkAllReadButton from './MarkAllReadButton';
import { markAllTicketsAsRead } from '../../../services/service';

jest.mock('../../../services/service');
jest.mock('../../../../components/ToastrWrapper');
jest.mock('react', () => ({
  ...jest.requireActual('react')
}));

jest.mock('react-redux', () => ({
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);
const props = { actorId: 8 };
describe('MarkAllReadButton under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should call handleOnClick', () => {
    markAllTicketsAsRead.mockResolvedValue({ status: 200 });
    render(<MarkAllReadButton {...props} />);

    fireEvent.click(screen.queryByText(/Mark all as read/));

    expect(useDispatch).toHaveBeenCalledTimes(1);
  });
});
