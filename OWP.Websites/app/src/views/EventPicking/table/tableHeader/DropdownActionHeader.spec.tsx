import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import DropdownActionHeader from './DropdownActionHeader';
import { setOpenPopupState } from '../eventPickingTableSlice/reducer';

const mockStore = configureStore([]);

describe('DropdownActionHeader', () => {
  let store: any;
  let refValue: React.RefObject<HTMLDivElement>;

  beforeEach(() => {
    store = mockStore({
      openPopupStore: {
        isOpenAddSpecialProduct: false,
        isOpenRemoveSpecialProduct: false
      }
    });

    refValue = React.createRef<HTMLDivElement>();

    store.dispatch = jest.fn();
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
    expect(store.dispatch).toHaveBeenCalledWith(
      setOpenPopupState({ key: 'isOpenAddSpecialProduct', value: true })
    );

    fireEvent.click(removeSpecialProduct);
    expect(store.dispatch).toHaveBeenCalledWith(
      setOpenPopupState({ key: 'isOpenRemoveSpecialProduct', value: true })
    );
  });
});
