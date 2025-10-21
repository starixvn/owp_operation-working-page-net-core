import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import '@testing-library/jest-dom';
import DropdownActionHeader from './DropdownActionHeader';
import { setOpenPopupState } from '../eventPickingTableSlice/reducer';

const mockStore = configureStore([]);

// Mock the hooks
jest.mock('../../app/hooks', () => ({
  useAppDispatch: jest.fn(),
  useAppSelector: jest.fn()
}));

// Mock ToastrWrapper
jest.mock('../../../components/ToastrWrapper', () => ({
  showError: jest.fn(),
  confirmYesNo: jest.fn()
}));

// Mock Spinner
jest.mock('../../../components/Spinner', () => ({
  show: jest.fn(),
  hide: jest.fn()
}));

// Mock updateNoOfferMatchThunk
jest.mock('../updateNoOfferMatchThunkNative', () => jest.fn());

import ToastrWrapper from '../../../components/ToastrWrapper';
import Spinner from '../../../components/Spinner';
import updateNoOfferMatchThunk from '../updateNoOfferMatchThunkNative';

describe('DropdownActionHeader', () => {
  let store: any;
  let refValue: React.RefObject<HTMLDivElement>;
  const mockDispatch = jest.fn();
  const mockUseAppDispatch = jest.fn();
  const mockUseAppSelector = jest.fn();

  beforeEach(() => {
    store = mockStore({
      openPopupStore: {
        isOpenAddSpecialProduct: false,
        isOpenRemoveSpecialProduct: false
      },
      eventPickingTable: {
        matchSelected: []
      }
    });

    refValue = React.createRef<HTMLDivElement>();

    store.dispatch = jest.fn();
    mockDispatch.mockClear();
    mockUseAppDispatch.mockReturnValue(mockDispatch);

    // Mock the hooks
    const { useAppDispatch, useAppSelector } = require('../../app/hooks');
    useAppDispatch.mockImplementation(() => mockUseAppDispatch());
    useAppSelector.mockImplementation(mockUseAppSelector);

    jest.clearAllMocks();
  });

  describe('Add Special Product', () => {
    test('should dispatch setOpenPopupState when matches are selected', () => {
      const matchSelected = [{ infoSiteMatchId: 1, childMatchId: null }];
      mockUseAppSelector.mockImplementation(selector => {
        return selector({ eventPickingTable: { matchSelected } });
      });

      render(
        <Provider store={store}>
          <DropdownActionHeader refValue={refValue} />
        </Provider>
      );

      const addSpecialProduct = screen.getByText('Add Special Product');
      fireEvent.click(addSpecialProduct);

      expect(mockDispatch).toHaveBeenCalledWith(
        setOpenPopupState({ key: 'isOpenAddSpecialProduct', value: true })
      );
    });

    test('should show error when no matches are selected', () => {
      mockUseAppSelector.mockImplementation(selector => {
        return selector({ eventPickingTable: { matchSelected: [] } });
      });

      render(
        <Provider store={store}>
          <DropdownActionHeader refValue={refValue} />
        </Provider>
      );

      const addSpecialProduct = screen.getByText('Add Special Product');
      fireEvent.click(addSpecialProduct);

      expect(ToastrWrapper.showError).toHaveBeenCalledWith(
        'You must select at least one match!'
      );
      expect(mockDispatch).not.toHaveBeenCalled();
    });
  });

  describe('Remove Special Product', () => {
    test('should dispatch setOpenPopupState when matches are selected', () => {
      const matchSelected = [{ infoSiteMatchId: 1, childMatchId: null }];
      mockUseAppSelector.mockImplementation(selector => {
        return selector({ eventPickingTable: { matchSelected } });
      });

      render(
        <Provider store={store}>
          <DropdownActionHeader refValue={refValue} />
        </Provider>
      );

      const removeSpecialProduct = screen.getByText('Remove Special Product');
      fireEvent.click(removeSpecialProduct);

      expect(mockDispatch).toHaveBeenCalledWith(
        setOpenPopupState({ key: 'isOpenRemoveSpecialProduct', value: true })
      );
    });

    test('should show error when no matches are selected', () => {
      mockUseAppSelector.mockImplementation(selector => {
        return selector({ eventPickingTable: { matchSelected: [] } });
      });

      render(
        <Provider store={store}>
          <DropdownActionHeader refValue={refValue} />
        </Provider>
      );

      const removeSpecialProduct = screen.getByText('Remove Special Product');
      fireEvent.click(removeSpecialProduct);

      expect(ToastrWrapper.showError).toHaveBeenCalledWith(
        'You must select at least one match!'
      );
      expect(mockDispatch).not.toHaveBeenCalled();
    });
  });

  describe('Offer', () => {
    test('should show error when no matches are selected', async () => {
      mockUseAppSelector.mockImplementation(selector => {
        return selector({ eventPickingTable: { matchSelected: [] } });
      });

      render(
        <Provider store={store}>
          <DropdownActionHeader refValue={refValue} />
        </Provider>
      );

      const offerButton = screen.getByText('Offer');
      fireEvent.click(offerButton);

      expect(ToastrWrapper.showError).toHaveBeenCalledWith(
        'You must select at least one match!'
      );
      expect(ToastrWrapper.confirmYesNo).not.toHaveBeenCalled();
    });

    test('should show confirmation and dispatch thunk when user confirms', async () => {
      const matchSelected = [
        { infoSiteMatchId: 1, childMatchId: null },
        { infoSiteMatchId: 2, childMatchId: null }
      ];
      mockUseAppSelector.mockImplementation(selector => {
        return selector({ eventPickingTable: { matchSelected } });
      });

      (ToastrWrapper.confirmYesNo as jest.Mock).mockResolvedValue({
        value: true
      });

      (updateNoOfferMatchThunk as unknown as jest.Mock).mockReturnValue({
        type: 'updateNoOfferMatch'
      });

      mockDispatch.mockResolvedValue({});

      render(
        <Provider store={store}>
          <DropdownActionHeader refValue={refValue} />
        </Provider>
      );

      const offerButton = screen.getByText('Offer');
      fireEvent.click(offerButton);

      await waitFor(() => {
        expect(ToastrWrapper.confirmYesNo).toHaveBeenCalledWith(
          'Are you sure that you want to select the game for those events?'
        );
      });

      await waitFor(() => {
        expect(Spinner.show).toHaveBeenCalled();
      });

      await waitFor(() => {
        expect(updateNoOfferMatchThunk).toHaveBeenCalledWith([
          { infoSiteMatchId: 1, childMatchId: null, isNoOffer: false },
          { infoSiteMatchId: 2, childMatchId: null, isNoOffer: false }
        ]);
      });

      await waitFor(() => {
        expect(mockDispatch).toHaveBeenCalled();
      });

      await waitFor(() => {
        expect(Spinner.hide).toHaveBeenCalled();
      });
    });

    test('should not dispatch thunk when user cancels confirmation', async () => {
      const matchSelected = [{ infoSiteMatchId: 1, childMatchId: null }];
      mockUseAppSelector.mockImplementation(selector => {
        return selector({ eventPickingTable: { matchSelected } });
      });

      (ToastrWrapper.confirmYesNo as jest.Mock).mockResolvedValue({
        value: false
      });

      render(
        <Provider store={store}>
          <DropdownActionHeader refValue={refValue} />
        </Provider>
      );

      const offerButton = screen.getByText('Offer');
      fireEvent.click(offerButton);

      await waitFor(() => {
        expect(ToastrWrapper.confirmYesNo).toHaveBeenCalledWith(
          'Are you sure that you want to select the game for those events?'
        );
      });

      expect(Spinner.show).not.toHaveBeenCalled();
      expect(updateNoOfferMatchThunk).not.toHaveBeenCalled();
      expect(mockDispatch).not.toHaveBeenCalled();
    });
  });

  describe('No Offer', () => {
    test('should show error when no matches are selected', async () => {
      mockUseAppSelector.mockImplementation(selector => {
        return selector({ eventPickingTable: { matchSelected: [] } });
      });

      render(
        <Provider store={store}>
          <DropdownActionHeader refValue={refValue} />
        </Provider>
      );

      const noOfferButton = screen.getByText('No Offer');
      fireEvent.click(noOfferButton);

      expect(ToastrWrapper.showError).toHaveBeenCalledWith(
        'You must select at least one match!'
      );
      expect(ToastrWrapper.confirmYesNo).not.toHaveBeenCalled();
    });

    test('should show confirmation and dispatch thunk when user confirms', async () => {
      const matchSelected = [
        { infoSiteMatchId: 1, childMatchId: null },
        { infoSiteMatchId: 2, childMatchId: null }
      ];
      mockUseAppSelector.mockImplementation(selector => {
        return selector({ eventPickingTable: { matchSelected } });
      });

      (ToastrWrapper.confirmYesNo as jest.Mock).mockResolvedValue({
        value: true
      });

      (updateNoOfferMatchThunk as unknown as jest.Mock).mockReturnValue({
        type: 'updateNoOfferMatch'
      });

      mockDispatch.mockResolvedValue({});

      render(
        <Provider store={store}>
          <DropdownActionHeader refValue={refValue} />
        </Provider>
      );

      const noOfferButton = screen.getByText('No Offer');
      fireEvent.click(noOfferButton);

      await waitFor(() => {
        expect(ToastrWrapper.confirmYesNo).toHaveBeenCalledWith(
          'Are you sure that you want to unselect the game for those events?'
        );
      });

      await waitFor(() => {
        expect(Spinner.show).toHaveBeenCalled();
      });

      await waitFor(() => {
        expect(updateNoOfferMatchThunk).toHaveBeenCalledWith([
          { infoSiteMatchId: 1, childMatchId: null, isNoOffer: true },
          { infoSiteMatchId: 2, childMatchId: null, isNoOffer: true }
        ]);
      });

      await waitFor(() => {
        expect(mockDispatch).toHaveBeenCalled();
      });

      await waitFor(() => {
        expect(Spinner.hide).toHaveBeenCalled();
      });
    });

    test('should not dispatch thunk when user cancels confirmation', async () => {
      const matchSelected = [{ infoSiteMatchId: 1, childMatchId: null }];
      mockUseAppSelector.mockImplementation(selector => {
        return selector({ eventPickingTable: { matchSelected } });
      });

      (ToastrWrapper.confirmYesNo as jest.Mock).mockResolvedValue({
        value: false
      });

      render(
        <Provider store={store}>
          <DropdownActionHeader refValue={refValue} />
        </Provider>
      );

      const noOfferButton = screen.getByText('No Offer');
      fireEvent.click(noOfferButton);

      await waitFor(() => {
        expect(ToastrWrapper.confirmYesNo).toHaveBeenCalledWith(
          'Are you sure that you want to unselect the game for those events?'
        );
      });

      expect(Spinner.show).not.toHaveBeenCalled();
      expect(updateNoOfferMatchThunk).not.toHaveBeenCalled();
      expect(mockDispatch).not.toHaveBeenCalled();
    });
  });

  describe('Rendering', () => {
    test('should render all menu items correctly', () => {
      mockUseAppSelector.mockImplementation(selector => {
        return selector({ eventPickingTable: { matchSelected: [] } });
      });

      const { container } = render(
        <Provider store={store}>
          <DropdownActionHeader refValue={refValue} />
        </Provider>
      );

      expect(screen.getByText('Add Special Product')).toBeInTheDocument();
      expect(screen.getByText('Remove Special Product')).toBeInTheDocument();
      expect(screen.getByText('Offer')).toBeInTheDocument();
      expect(screen.getByText('No Offer')).toBeInTheDocument();

      const dropdown = container.querySelector('.dropdown-action.is-show');
      expect(dropdown).toBeInTheDocument();
    });

    test('should apply correct ref to the dropdown', () => {
      mockUseAppSelector.mockImplementation(selector => {
        return selector({ eventPickingTable: { matchSelected: [] } });
      });

      render(
        <Provider store={store}>
          <DropdownActionHeader refValue={refValue} />
        </Provider>
      );

      expect(refValue.current).toBeInstanceOf(HTMLDivElement);
      expect(refValue.current?.className).toContain('dropdown-action');
    });
  });
});
