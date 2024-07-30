import { fireEvent, render, screen } from '@testing-library/react';
import { LogHistoryProps } from '../../../../../typing/tableType';
import useStore from '../../../../../app/store';
import LogHistory from './LogHistory';

const mockClosePopup = jest.fn();
const props: LogHistoryProps = {
  leagueName: 'ABC',
  handleClosePopup: mockClosePopup
};
useStore.setState({
  logData: [
    {
      action: 123,
      modifyBy: 'sa',
      modifyDate: '12-08-2022',
      newValue: {
        maxBetHDP: '2000',
        maxBetOU: '2000'
      },
      oldValue: {
        maxBetHDP: '1000',
        maxBetOU: '1000'
      }
    },
    {
      action: 124,
      modifyBy: 'sa',
      modifyDate: '12-08-2022',
      newValue: {
        maxBetHDP: '3000',
        maxBetOU: '3000'
      },
      oldValue: {
        maxBetHDP: '1000',
        maxBetOU: '1000'
      }
    }
  ]
});
describe('Modal Log History', () => {
  it('Should render modal', () => {
    render(<LogHistory {...props} />);
    const leagueName = screen.getByText('Log History - ABC');
    expect(leagueName).toBeInTheDocument();
  });
  it('Should call close button', () => {
    render(<LogHistory {...props} />);
    const closeButton = screen.getByLabelText('Close');
    closeButton && fireEvent.click(closeButton);
    expect(closeButton).toBeInTheDocument();
    expect(mockClosePopup).toBeCalled();
  });
});
