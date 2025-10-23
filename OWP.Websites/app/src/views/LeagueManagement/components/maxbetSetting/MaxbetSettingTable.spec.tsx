import React from 'react';
import { fireEvent, render, screen } from '@testing-library/react';
import useStore from './store/storeNative';
import { MaxBetSettingModel, Policy } from './typeNative';
import MaxbetSettingTable from './MaxbetSettingTable';

const mockOpenLogPopup = jest.fn();
const mockSetPopupType = jest.fn();
const mockGetMaxbetSetting = jest.fn();
const mockGetSecondAcceptValue = jest.fn();
const mockSetEditModel = jest.fn();
const mockSetOpenEditModel = jest.fn();
const mockSetLogData = jest.fn();

const mockMaxbetSettings: MaxBetSettingModel[] = [
  {
    fT_HDP: 500000.0,
    fT_OU: 500000.0,
    firstHalf_HDP: 120000.0,
    firstHalf_OU: 150000.0,
    noOfCorner_FT_HDP: 9000.0,
    noOfCorner_FT_OU: 18000.0,
    noOfCorner_1H_HDP: 4000.0,
    noOfCorner_1H_OU: 8000.0,
    nextCorner_FT_HDP: 18000.0,
    totalBooking_FT_HDP: 10000.0,
    totalBooking_FT_OU: 10000.0,
    totalBooking_1H_HDP: 5000.0,
    totalBooking_1H_OU: 5000.0,
    nextBooking_FT_HDP: 10000.0,
    secondAccept_Main: '8s Accept',
    secondAccept_Corner: '15s Accept',
    secondAccept_TotalBooking: '15s Accept',
    secondAccept_Fantasy: '20s Accept',
    lastUpdatedBy: 'TestUser'
  },
  {
    fT_HDP: 300000.0,
    fT_OU: 300000.0,
    firstHalf_HDP: 100000.0,
    firstHalf_OU: 100000.0,
    noOfCorner_FT_HDP: 5000.0,
    noOfCorner_FT_OU: 10000.0,
    noOfCorner_1H_HDP: 2000.0,
    noOfCorner_1H_OU: 4000.0,
    nextCorner_FT_HDP: 10000.0,
    totalBooking_FT_HDP: 8000.0,
    totalBooking_FT_OU: 8000.0,
    totalBooking_1H_HDP: 3000.0,
    totalBooking_1H_OU: 3000.0,
    nextBooking_FT_HDP: 8000.0,
    secondAccept_Main: '10s Accept',
    secondAccept_Corner: '20s Accept',
    secondAccept_TotalBooking: '20s Accept',
    secondAccept_Fantasy: '25s Accept',
    lastUpdatedBy: 'AnotherUser'
  }
];

const setUp = (
  maxbetSettings: MaxBetSettingModel[] | undefined = mockMaxbetSettings,
  loading = 'idle',
  HasWrite = true
) => {
  // Reset store completely first
  useStore.setState({
    getMaxbetSettings: mockGetMaxbetSetting,
    getSecondAcceptValues: mockGetSecondAcceptValue,
    maxbetSettings: [] as MaxBetSettingModel[],
    loading: 'idle',
    policy: { HasWrite: true, HasRead: true } as Policy,
    setEditModel: jest.fn(),
    setLogData: jest.fn(),
    setOpenLogPopup: jest.fn(),
    setOpenEditPopup: jest.fn(),
    setPopupType: jest.fn(),
    orderInfo: {
      columnId: 'fT_HDP',
      orderType: 'NONE'
    },
    editModel: {
      fTHDP: 0,
      column: '',
      oldValue: 0
    },
    isOpenEditPopup: false,
    isOpenLogPopup: false,
    popupType: null,
    logData: [],
    maxBetValue: 0,
    secondAcceptValues: [],
    queryPolicy: jest.fn(),
    setCloseEditPopup: jest.fn(),
    setCloseLogPopup: jest.fn(),
    updateMaxbetSetting: jest.fn(),
    sortMaxbetSettings: jest.fn()
  });

  // Now set the specific test values
  useStore.setState({
    getMaxbetSettings: mockGetMaxbetSetting,
    getSecondAcceptValues: mockGetSecondAcceptValue,
    maxbetSettings: maxbetSettings as any,
    loading: loading,
    policy: { HasWrite, HasRead: true } as Policy,
    setEditModel: mockSetEditModel,
    setLogData: mockSetLogData,
    setOpenLogPopup: mockOpenLogPopup,
    setOpenEditPopup: mockSetOpenEditModel,
    setPopupType: mockSetPopupType
  });
};

describe('MaxbetSettingTable', () => {
  beforeEach(() => {
    setUp();
    jest.clearAllMocks();
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  describe('Component Rendering', () => {
    it('should not render maxbet-setting-table when maxbetSettings is undefined', () => {
      // Explicitly reset store with undefined
      useStore.setState({
        getMaxbetSettings: mockGetMaxbetSetting,
        getSecondAcceptValues: mockGetSecondAcceptValue,
        maxbetSettings: undefined as any,
        loading: 'idle',
        policy: { HasWrite: true, HasRead: true } as Policy,
        setEditModel: mockSetEditModel,
        setLogData: mockSetLogData,
        setOpenLogPopup: mockOpenLogPopup,
        setOpenEditPopup: mockSetOpenEditModel,
        setPopupType: mockSetPopupType,
        orderInfo: {
          columnId: 'fT_HDP',
          orderType: 'NONE'
        },
        editModel: {
          fTHDP: 0,
          column: '',
          oldValue: 0
        },
        isOpenEditPopup: false,
        isOpenLogPopup: false,
        popupType: null,
        logData: [],
        maxBetValue: 0,
        secondAcceptValues: [],
        queryPolicy: jest.fn(),
        setCloseEditPopup: jest.fn(),
        setCloseLogPopup: jest.fn(),
        updateMaxbetSetting: jest.fn(),
        sortMaxbetSettings: jest.fn()
      });

      const { container } = render(<MaxbetSettingTable />);
      const element = container.querySelector('.maxbet-setting-table');
      expect(element).toBe(null);
    });

    it('should render maxbet-setting-table with no data message when maxbetSettings is empty array', () => {
      setUp([]);
      const { container } = render(<MaxbetSettingTable />);
      const element = container.querySelector('.maxbet-setting-table');
      expect(element).not.toBe(null);
      const noDataElement = container.querySelector('.rt-noData');
      expect(noDataElement).not.toBe(null);
      expect(noDataElement?.textContent).toBe('There is no data.');
    });

    it('should render maxbet-setting-table when maxbetSettings has data', () => {
      const { container } = render(<MaxbetSettingTable />);
      const element = container.querySelector('.maxbet-setting-table');
      expect(element).not.toBe(null);
    });

    it('should render Loading component when loading is pending', () => {
      setUp(mockMaxbetSettings, 'pending');
      const { container } = render(<MaxbetSettingTable />);
      const loadingElement = container.querySelector('.loading-process');
      expect(loadingElement).not.toBe(null);
    });

    it('should display total records count correctly', () => {
      const { container } = render(<MaxbetSettingTable />);
      const totalRecords = container.querySelector('.total-records');
      expect(totalRecords).not.toBe(null);
      expect(totalRecords?.textContent).toBe(
        `Total records: ${mockMaxbetSettings.length}`
      );
    });

    it('should display correct total records for single item', () => {
      setUp([mockMaxbetSettings[0]]);
      const { container } = render(<MaxbetSettingTable />);
      const totalRecords = container.querySelector('.total-records');
      expect(totalRecords?.textContent).toBe('Total records: 1');
    });
  });

  describe('Component Lifecycle', () => {
    it('should call getMaxbetSettings and getSecondAcceptValues on mount', () => {
      render(<MaxbetSettingTable />);
      expect(mockGetMaxbetSetting).toHaveBeenCalledTimes(1);
      expect(mockGetSecondAcceptValue).toHaveBeenCalledTimes(1);
    });
  });

  describe('Cell Interactions', () => {
    it('should apply activeClick class when clicking on a cell', () => {
      const { container } = render(<MaxbetSettingTable />);
      const cell = container.querySelector('.fT_OU');
      expect(cell).not.toBe(null);

      fireEvent.click(cell!);
      const activeCell = container.querySelector('.fT_OU.activeClick');
      expect(activeCell).not.toBe(null);
    });

    it('should remove activeClick class from previous cell when clicking another cell', () => {
      const { container } = render(<MaxbetSettingTable />);
      const firstCell = container.querySelector('.fT_OU');
      const secondCell = container.querySelector('.firstHalf_HDP');

      fireEvent.click(firstCell!);
      expect(firstCell?.classList.contains('activeClick')).toBe(true);

      fireEvent.click(secondCell!);
      expect(firstCell?.classList.contains('activeClick')).toBe(false);
      expect(secondCell?.classList.contains('activeClick')).toBe(true);
    });

    it('should open edit popup when double clicking on a regular cell with write permission', () => {
      const { container } = render(<MaxbetSettingTable />);
      // Find the inner editable cell within the fT_OU column
      const outerCell = container.querySelector('.fT_OU.cell');
      expect(outerCell).not.toBe(null);

      const editableCell = outerCell?.querySelector('.cell-test');
      expect(editableCell).not.toBe(null);

      fireEvent.doubleClick(editableCell!);
      expect(mockSetEditModel).toHaveBeenCalled();
      expect(mockSetPopupType).toHaveBeenCalled();
      expect(mockSetOpenEditModel).toHaveBeenCalled();
    });

    it('should not open edit popup when double clicking without write permission', () => {
      setUp(mockMaxbetSettings, 'idle', false);
      const { container } = render(<MaxbetSettingTable />);
      const cell = container.querySelector('.cell-test');

      if (cell) {
        fireEvent.doubleClick(cell);
      }

      expect(mockSetEditModel).not.toHaveBeenCalled();
      expect(mockSetPopupType).not.toHaveBeenCalled();
      expect(mockSetOpenEditModel).not.toHaveBeenCalled();
    });
  });

  describe('History Icon Interaction', () => {
    it('should open log popup when clicking history icon', () => {
      const { container } = render(<MaxbetSettingTable />);
      const historyIcon = container.querySelector('.mdi-icon');
      expect(historyIcon).not.toBe(null);

      fireEvent.click(historyIcon!);
      expect(mockSetLogData).toHaveBeenCalled();
      expect(mockOpenLogPopup).toHaveBeenCalled();
    });
  });

  describe('Permissions', () => {
    it('should not allow editing when HasWrite is false', () => {
      setUp(mockMaxbetSettings, 'idle', false);
      const { container } = render(<MaxbetSettingTable />);
      const cells = container.querySelectorAll('.cell-test');

      if (cells.length > 0) {
        fireEvent.doubleClick(cells[0]);
        expect(mockSetEditModel).toHaveBeenCalledTimes(0);
        expect(mockSetPopupType).toHaveBeenCalledTimes(0);
        expect(mockSetOpenEditModel).toHaveBeenCalledTimes(0);
      }
    });

    it('should allow viewing history when HasWrite is false', () => {
      setUp(mockMaxbetSettings, 'idle', false);
      const { container } = render(<MaxbetSettingTable />);
      const historyIcon = container.querySelector('.mdi-icon');

      if (historyIcon) {
        fireEvent.click(historyIcon);
        expect(mockSetLogData).toHaveBeenCalled();
        expect(mockOpenLogPopup).toHaveBeenCalled();
      }
    });
  });

  describe('Loading States', () => {
    it('should not render table when loading and maxbetSettings is undefined', () => {
      setUp(undefined, 'pending');
      const { container } = render(<MaxbetSettingTable />);
      const table = container.querySelector('.maxbet-setting-table');
      const loading = container.querySelector('.loading-process');

      expect(table).toBe(null);
      expect(loading).not.toBe(null);
    });

    it('should render table after loading completes', () => {
      setUp(mockMaxbetSettings, 'idle');
      const { container } = render(<MaxbetSettingTable />);
      const table = container.querySelector('.maxbet-setting-table');
      const loading = container.querySelector('.loading-process');

      expect(table).not.toBe(null);
      expect(loading).toBe(null);
    });
  });
});
