import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import TableHeader from './TableHeader';
import {
  TABLE_HEADER,
  TABLE_HEADER_TURN_OVER_MARGIN
} from '../TableSchema.json';
import { VIEW } from '../../mode/mode.json';
import { FilterAllColumn, TFilterState } from '../../types/dataTableType';
import { REQUEST_STATUS } from '../../../Shared/Constants';

const mockStore = configureStore([]);

describe('TableHeader', () => {
  let store: any;

  beforeEach(() => {
    store = mockStore({
      eventPickingTable: {
        openPopupState: {
          isOpenAddSpecialProduct: false,
          isOpenRemoveSpecialProduct: false
        },
        entitiesByDate: [],
        entities: [],
        entitiesOriginal: [],
        isNoOfferAll: false,
        isConfirmAll: false,
        valueFilterAllColumn: {} as FilterAllColumn,
        columnKeyFilterCurrent: '',
        filterState: [] as TFilterState[],
        loading: REQUEST_STATUS.idle,
        previousNoOfferIndex: null,
        multipleFilter: {
          KO_Time: {
            isFilter: false,
            selected: [],
            options: []
          },
          MB: {
            isFilter: false,
            selected: [],
            options: []
          }
        }
      },
      turnOverMarginPolicy: {
        permission: { HasRead: true }
      },
      pagePolicy: {
        permission: { HasRead: true, HasWrite: true }
      },
      mode: { mode: VIEW }
    });

    store.dispatch = jest.fn();
  });

  test('renders dropdown icon and handles dropdown action', () => {
    render(
      <Provider store={store}>
        <table>
          <thead>
            <TableHeader />
          </thead>
        </table>
      </Provider>
    );

    const dropdownIcon = screen.getByTestId('dropdown');
    fireEvent.click(dropdownIcon);

    expect(screen.getByText('Add Special Product')).toBeInTheDocument();
    expect(screen.getByText('Remove Special Product')).toBeInTheDocument();
  });
});
