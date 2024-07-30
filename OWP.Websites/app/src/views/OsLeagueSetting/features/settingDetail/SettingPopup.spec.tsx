import SettingPopup from './SettingPopup';
import useStore from '../../app/store';
import { fireEvent, render, screen } from '@testing-library/react';
import { TSettingPopup } from '../../typing/settingPopup';

const mockOnSave = jest.fn();
const mockSetPopupType = jest.fn();
useStore.setState({
  policy: {
    Username: 'sa',
    HasRead: true,
    HasWrite: true,
    ActorId: 8,
    SiteId: 99
  },
  setPopupType: mockSetPopupType
});

const props: TSettingPopup = {
  title: 'abc',
  isAdd: true,
  onSave: mockOnSave
};
describe('SettingPopup', () => {
  it('handle onclick Add', () => {
    render(<SettingPopup {...props} />);
    const button = screen.queryByText(/Add/i);
    fireEvent.click(button!);
    expect(mockOnSave).toHaveBeenCalled();
  });
  it('handle onclick Cancel', () => {
    render(<SettingPopup {...props} />);
    const button = screen.queryByText(/Cancel/i);
    fireEvent.click(button!);
    expect(mockSetPopupType).toHaveBeenCalled();
  });
});
