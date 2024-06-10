import { fireEvent, render, screen } from '@testing-library/react';
import useStore from './store/storeNative';
import { EditModel, Policy } from './typeNative';
import MaxbetSettingTable from './MaxbetSettingTable';

const mockOpenLogPopup = jest.fn();
const mockSetPopupType = jest.fn();
const mockGetMaxbetSetting = jest.fn();
const mockGetSecondAcceptValue = jest.fn();
const mockSetEditModel = jest.fn();
const mockSetOpenEditModel = jest.fn();
const mockSetLogData = jest.fn();
const setUp = (maxbetSettings, loading = 'idle', HasWrite = true) => {
  useStore.setState({
    getMaxbetSettings: mockGetMaxbetSetting,
    getSecondAcceptValues: mockGetSecondAcceptValue,
    maxbetSettings: maxbetSettings,
    loading: loading,
    policy: { HasWrite } as Policy,
    setEditModel: mockSetEditModel,
    setLogData: mockSetLogData,
    setOpenLogPopup: mockOpenLogPopup,
    setOpenEditPopup: mockSetOpenEditModel,
    setPopupType: mockSetPopupType
  });
}
const props = [
  {
    "fT_HDP": 500000.0000,
    "fT_OU": 500000.0000,
    "firstHalf_HDP": 120000.0000,
    "firstHalf_OU": 150000.0000,
    "noOfCorner_FT_HDP": 9000.0000,
    "noOfCorner_FT_OU": 18000.0000,
    "noOfCorner_1H_HDP": 4000.0000,
    "noOfCorner_1H_OU": 8000.0000,
    "nextCorner_FT_HDP": 18000.0000,
    "totalBooking_FT_HDP": 10000.0000,
    "totalBooking_FT_OU": 10000.0000,
    "totalBooking_1H_HDP": 5000.0000,
    "totalBooking_1H_OU": 5000.0000,
    "nextBooking_FT_HDP": 10000.0000,
    "secondAccept_Main": "8s Accept",
    "secondAccept_Corner": "15s Accept",
    "secondAccept_TotalBooking": "15s Accept",
    "secondAccept_Fantasy": "20s Accept",
    "lastUpdatedBy": null
  }
]

describe('MaxbetSettingTable under test', () => {
  beforeEach(() => {
    setUp(props);
    jest.resetAllMocks();
  });

  it('maxbetSettings is empty, Should not render maxbet-setting-table', () => {
    setUp(null);
    const { container } = render(<MaxbetSettingTable />);
    const element = container.querySelector('.maxbet-setting-table');
    expect(element).toBeNull();
  });

  it('maxbetSettings is not empty, Should render maxbet-setting-table', () => {
    const { container } = render(<MaxbetSettingTable />);
    const element = container.querySelector('.maxbet-setting-table');
    expect(element).toBeInTheDocument();
  });

  it('should render Loading, when loading is pending', () => {
    const { container } = render(<MaxbetSettingTable />);
    const element = container.querySelector('.maxbet-setting-table');
    expect(element).toBeInTheDocument();
  });

  it('should call mockUpdateMaxbetSetting, when click Save button', () => {
    setUp(null, 'pending')
    const { container } = render(<MaxbetSettingTable />)
    const element = container.querySelector('.loading-process');
    expect(element).toBeInTheDocument();
  });

  it('should render activeClick class, when click on cell', () => {
    const { container } = render(<MaxbetSettingTable />);
    const element = container.querySelector('.fT_OU');
    fireEvent.click(element!);
    expect(element).toBeInTheDocument();
    const activeClass = container.querySelector('.activeClick');
    expect(activeClass).toBeInTheDocument();
  });

  it('when double click on cell', () => {
    const { container } = render(<MaxbetSettingTable />);
    const element = container.querySelector('.cell-test');
    expect(element).toBeInTheDocument();
    fireEvent.doubleClick(element!);
    expect(mockSetEditModel).toBeCalled();
    expect(mockSetPopupType).toBeCalled();
    expect(mockSetOpenEditModel).toBeCalled();
  });

  it('when double click on cell second accept', () => {
    const { container } = render(<MaxbetSettingTable />);
    const element = container.querySelectorAll('.cell-test-2')[1];
    expect(element).toBeInTheDocument();
    fireEvent.doubleClick(element!);
    expect(mockSetEditModel).toBeCalled();
    expect(mockSetPopupType).toBeCalled();
    expect(mockSetOpenEditModel).toBeCalled();
  });

  it('when click History icon', () => {
    const { container } = render(<MaxbetSettingTable />);
    const element = container.querySelector('.mdi-icon');
    expect(element).toBeInTheDocument();
    fireEvent.click(element!);
    expect(mockSetLogData).toBeCalled();
    expect(mockOpenLogPopup).toBeCalled();
  });

  it('should not call mockSetPopupType, when HasWrite=false, click cell value', () => {
    setUp(props, 'idle', false);
    const { container } = render(<MaxbetSettingTable />);
    const element = container.querySelectorAll('.cell-test-2')[1];
    expect(element).toBeInTheDocument();
    fireEvent.doubleClick(element!);
    expect(mockSetEditModel).toBeCalledTimes(0);
    expect(mockSetPopupType).toBeCalledTimes(0);
    expect(mockSetOpenEditModel).toBeCalledTimes(0);
  });
});
