import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import DropdownActionHeader from './DropdownActionHeader';
import { setOpenPopupState } from '../eventPickingTableSlice/reducer';

const mockStore = configureStore([]);

// Mock the hooks
jest.mock('../../app/hooks', () => ({
  useAppDispatch: jest.fn(),
  useAppSelector: jest.fn()
}));

describe('DropdownActionHeader', () => {
  let store: any;
  let refValue: React.RefObject<HTMLDivElement>;
  const mockDispatch = jest.fn();

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

    // Mock the hooks
    const { useAppDispatch, useAppSelector } = require('../../app/hooks');
    useAppDispatch.mockReturnValue(mockDispatch);
    useAppSelector.mockImplementation((selector) => {
      const mockState = {
        eventPickingTable: {
          matchSelected: []
        }
      };
      return selector(mockState);
    });
  });

  test('renders DropdownActionHeader and dispatches actions on click', () => {
    render(
      <Provider store={store}>
        <DropdownActionHeader refValue={refValue} />
      </Provider>
    );

    const addSpecialProduct = screen.getByText('Add Special Product');
    const removeSpecialProduct = screen.getByText('Remove Special Product');

    fireEvent.click(addSpecialProduct);
    expect(mockDispatch).toHaveBeenCalledWith(
      setOpenPopupState({ key: 'isOpenAddSpecialProduct', value: true })
    );

    fireEvent.click(removeSpecialProduct);
    expect(mockDispatch).toHaveBeenCalledWith(
      setOpenPopupState({ key: 'isOpenRemoveSpecialProduct', value: true })
    );
  });
});
