import EditSecondAcceptSettingPopup from './EditSecondAcceptSettingPopup';
import { useState } from 'react';
import { fireEvent, render, screen } from '@testing-library/react';
import useStore from '../store/storeNative';
import { EditModel } from '../typeNative';

const mockClosePopup = jest.fn();
const mockGetMaxbetSetting = jest.fn();
const mockUpdateMaxbetSetting = jest.fn();

useStore.setState({
  editModel: { 'fT_HDP': 500000, 'oldValue': 1, column: 'fT_OU' } as EditModel,
  setCloseEditPopup: mockClosePopup,
  updateMaxbetSetting: mockUpdateMaxbetSetting,
  isOpenEditPopup: true,
  secondAcceptValues: [
    { label: '10s Accept', value: 1 },
    { label: 'DEF', value: 3 }
  ],
  getMaxbetSettings: mockGetMaxbetSetting,
});

describe('EditSecondAcceptSettingPopup under test', () => {
  beforeEach(() => {
    jest.resetAllMocks();
  });

  it('Should render Modal', () => {
    render(<EditSecondAcceptSettingPopup />);
    const popupName = screen.getByText(
      'Edit Second Accept Value'
    );
    expect(popupName).toBeInTheDocument();
  });

  it('should call mockClosePopup, when click close button', () => {
    render(<EditSecondAcceptSettingPopup />);
    const closeButton = screen.getByText('×');
    expect(closeButton).toBeInTheDocument();
    fireEvent.click(closeButton!);
    expect(mockClosePopup).toBeCalled();
  });

  it('should call mockUpdateMaxbetSetting, when click Save button', () => {
    render(<EditSecondAcceptSettingPopup />)
    const saveButton = screen.getByText('Save');
    expect(saveButton).toBeInTheDocument();
    fireEvent.click(saveButton);
  });

  it('should call mockUpdateMaxbetSetting, when change select', () => {
    const { container } = render(<EditSecondAcceptSettingPopup />);
    const selectComp = screen.getByRole('combobox');
    expect(selectComp).toBeInTheDocument();
    fireEvent.change(selectComp!, {
      target: {
        label: 'DEF',
        value: 3
      }
    });
  });
});
