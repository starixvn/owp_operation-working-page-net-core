import LogSettingPopup from './LogSettingPopup';
import { fireEvent, render, screen } from '@testing-library/react';
import useStore from '../store/storeNative';

const mockClosePopup = jest.fn();
useStore.setState({
  setCloseLogPopup: mockClosePopup,
  isOpenLogPopup: true,
  maxBetValue: 500000,
  secondAcceptValues: [
    { value: 1, label: '10s Accept' },
    { value: 2, label: 'Any' },
    { value: 3, label: 'KO+15' }
  ],
  logData: [
    {
      action: 128,
      oldValue: {
        fT_HDP: 0,
        fT_OU: 0,
        firstHalf_HDP: 20000,
        firstHalf_OU: 0,
        noOfCorner_FT_HDP: 0,
        noOfCorner_FT_OU: 0,
        noOfCorner_1H_HDP: 0,
        noOfCorner_1H_OU: 0,
        nextCorner_FT_HDP: 0,
        totalBooking_FT_HDP: 0,
        totalBooking_FT_OU: 0,
        totalBooking_1H_HDP: 0,
        totalBooking_1H_OU: 0,
        nextBooking_FT_HDP: 0,
        secondAccept_Main: '3',
        secondAccept_Corner: '',
        secondAccept_TotalBooking: '',
        secondAccept_Fantasy: '',
        lastUpdatedBy: 'sa'
      },
      newValue: {
        fT_HDP: 0,
        fT_OU: 0,
        firstHalf_HDP: 12000,
        firstHalf_OU: 0,
        noOfCorner_FT_HDP: 0,
        noOfCorner_FT_OU: 0,
        noOfCorner_1H_HDP: 0,
        noOfCorner_1H_OU: 0,
        nextCorner_FT_HDP: 0,
        totalBooking_FT_HDP: 0,
        totalBooking_FT_OU: 0,
        totalBooking_1H_HDP: 0,
        totalBooking_1H_OU: 0,
        nextBooking_FT_HDP: 0,
        secondAccept_Main: '1',
        secondAccept_Corner: '',
        secondAccept_TotalBooking: '',
        secondAccept_Fantasy: '',
        lastUpdatedBy: 'sa'
      },
      modifyDate: '08/30/2022 03:56 AM',
      modifyBy: 'sa'
    }
  ]
});
describe('Modal Log History', () => {
  it('Should render modal', () => {
    const component = render(<LogSettingPopup />);
    const popupName = screen.getByText(
      'Log History | Main Match - MaxBet FT HDP of 500K'
    );
    expect(popupName).toBeInTheDocument();
  });
  it('Should call close button', () => {
    render(<LogSettingPopup />);
    const closeButton = screen.getByLabelText('Close');
    closeButton && fireEvent.click(closeButton);
    expect(closeButton).toBeInTheDocument();
    expect(mockClosePopup).toBeCalled();
  });
});
