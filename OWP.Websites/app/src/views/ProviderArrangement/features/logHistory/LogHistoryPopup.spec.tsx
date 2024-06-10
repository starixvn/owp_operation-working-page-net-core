import { fireEvent, render, screen } from '@testing-library/react';
import { useSelector } from 'react-redux';
import LogHistoryPopup from './LogHistoryPopup';

type TPopupProps = {
  handleClosePopup(): void;
};
jest.mock('react-redux', () => ({
  useSelector: jest.fn()
}));
const mockClosePopup = jest.fn();
const props: TPopupProps = {
  handleClosePopup: mockClosePopup
};

(useSelector as jest.Mock).mockImplementation(selectorFn =>
  selectorFn({
    providerReducer: {
      logData: [
        {
          action: 123,
          modifyBy: 'sa',
          modifyDate: '12-08-2022',
          newValue: ['abc', 'xyz'],
          oldValue: ['abc', 'xyz']
        }
      ]
    }
  })
);

describe('Modal Log History', () => {
  it('Should render modal', () => {
    render(<LogHistoryPopup {...props} />);
    const leagueName = screen.getByText('Log History - Provider Arrangement');
    expect(leagueName).toBeInTheDocument();
  });
  it('Should call close button', () => {
    render(<LogHistoryPopup {...props} />);
    const closeButton = screen.getByLabelText('Close');
    closeButton && fireEvent.click(closeButton);
    expect(closeButton).toBeInTheDocument();
    expect(mockClosePopup).toBeCalled();
  });
});
