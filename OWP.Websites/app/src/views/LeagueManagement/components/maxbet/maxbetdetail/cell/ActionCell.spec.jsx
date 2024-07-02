import { fireEvent, render } from '@testing-library/react';
import { useDispatch, useSelector } from 'react-redux';
import ToastrWrapper from '../../../../../components/ToastrWrapper';
import ActionCell from './ActionCell';

jest.mock('react-redux', () => ({
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);

jest.mock('../../../../../components/ToastrWrapper');

ToastrWrapper.confirm.mockResolvedValue({
  value: true
});

// const setUp = pending => {
//   useSelector.mockImplementation(selectorFn =>
//     selectorFn({
//       maxbetGeneralReducer: {
//         maxbetGeneralData: [],
//         maxbetGeneralPending: pending
//       },
//       showHidePopupFilterReducer: {
//         filterKey: '',
//         isDisplay: ''
//       }
//     })
//   );
// };

describe('test TableMaxbetGeneral component', () => {
  const props = {
    data: {},
    rowIndex: 0
  };
  it('render correct loading component', () => {
    const { container, getByText } = render(<ActionCell {...props} />);

    expect(container.querySelector('.action-cell')).toBeInTheDocument();
  });

  it('should handle delete when click icon delete', () => {
    const { container, getByText } = render(<ActionCell {...props} />);
    const icon = container.querySelector('.icon-delete');
    expect(icon).toBeInTheDocument();

    fireEvent.click(icon);

    expect(ToastrWrapper.confirm).toBeCalled();
  });
});
