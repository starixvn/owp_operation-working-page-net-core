import { fireEvent, render, screen } from '@testing-library/react';
import useStore from '../store/storeNative';
import { EditModel } from '../typeNative';
import EditMaxbetSettingPopup from './EditMaxbetSettingPopup';

const mockClosePopup = jest.fn();
const mockGetMaxbetSetting = jest.fn();
const mockUpdateMaxbetSetting = jest.fn();
useStore.setState({
  editModel: { 'fTHDP': 500000, 'oldValue': 1, column: 'fT_OU' } as EditModel,
  setCloseEditPopup: mockClosePopup,
  updateMaxbetSetting: mockUpdateMaxbetSetting,
  isOpenEditPopup: true,
  getMaxbetSettings: mockGetMaxbetSetting,
});

describe('EditMaxbetSettingPopup under test', () => {
  beforeEach(() => {
    jest.resetAllMocks();
  });

  it('Should render Modal', () => {
    render(<EditMaxbetSettingPopup />);
    const popupName = screen.getByText(
      'Edit Max Bet'
    );
    expect(popupName).toBeInTheDocument();
  });

  it('should call mockClosePopup, when click close button', () => {
    const { container } = render(<EditMaxbetSettingPopup />);
    const closeButton = screen.getByText('×');
    expect(closeButton).toBeInTheDocument();
    fireEvent.click(closeButton!);
    expect(mockClosePopup).toBeCalled();
  });

  it('should call mockUpdateMaxbetSetting, when click Save button', () => {
    render(<EditMaxbetSettingPopup />)
    const saveButton = screen.getByText('Save');
    expect(saveButton).toBeInTheDocument();
    fireEvent.click(saveButton);
    expect(mockUpdateMaxbetSetting).toBeCalled();
    expect(mockClosePopup).toBeCalled();
  });

  it('should call mockUpdateMaxbetSetting, when press Enter', () => {
    render(<EditMaxbetSettingPopup />);
    const input = screen.getByPlaceholderText('New value');
    fireEvent.focus(input!);
    fireEvent.keyDown(input!, { key: 'Enter', code: 13, charCode: 13 });
    expect(mockUpdateMaxbetSetting).toBeCalled();
    expect(mockClosePopup).toBeCalled();
  });
});
