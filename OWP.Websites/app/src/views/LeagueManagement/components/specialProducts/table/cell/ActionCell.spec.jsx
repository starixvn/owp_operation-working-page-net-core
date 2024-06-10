import { useDispatch } from 'react-redux';
import { render, fireEvent } from '@testing-library/react';
import ToastrWrapper from '../../../../../components/ToastrWrapper';
import ActionCell from './ActionCell';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('../../../../../components/ToastrWrapper');

const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);
ToastrWrapper.confirm.mockResolvedValue({
  value: true
});

describe('ActionCell component under unit test', () => {
  it('render component correct', () => {
    const props = { productId: 10, maxbetFTHDP: 100 };
    const { container } = render(<ActionCell {...props} />);

    const icon = container.querySelector('.icon-delete');

    expect(icon).toBeInTheDocument();

    fireEvent.click(icon);
    expect(ToastrWrapper.confirm).toBeCalled();
  });

  it('should not render icon delete', () => {
    const props = { productId: -1, maxbetFTHDP: 100 };
    const { container } = render(<ActionCell {...props} />);

    const icon = container.querySelector('.icon-delete');

    expect(icon).not.toBeInTheDocument();
  });
});
