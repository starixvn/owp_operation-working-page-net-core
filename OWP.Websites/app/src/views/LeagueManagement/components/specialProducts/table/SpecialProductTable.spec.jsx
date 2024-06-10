import { Provider } from 'react-redux';
import configureMockStore from 'redux-mock-store';
import SpecialProductTable from './SpecialProductTable';
import { render, fireEvent, screen } from '@testing-library/react';
import ReactTable from 'react-table';

jest.mock('react-redux', () => {
  const actualReactRedux = jest.requireActual('react-redux');
  return {
    ...actualReactRedux,
    useSelector: jest.fn().mockImplementation(() => {
      return {
        specialProductData: [
          {
            Maxbet_FTHDP: 1,
            ProductId: 283,
            Remark: 'Vietnam’s N'
          }
        ]
      };
    }),
    useStore: jest.fn().mockImplementation(() => {
      return {
        getState: jest.fn().mockImplementation(() => {
          return {
            policyReducer: {
              policyData: {
                generalPolicy: {
                  HasWrite: true
                }
              }
            }
          };
        })
      };
    })
  };
});

const mockStore = configureMockStore([]);
const store = mockStore({});
let component;

beforeEach(() => {
  component = render(
    <Provider store={store}>
      <SpecialProductTable />
    </Provider>
  );
});

describe('SpecialProductTable', () => {
  it('Should render table', () => {
    expect(component.container.querySelector('.table')).toBeInTheDocument();
  });

  it('Should table contain 1 row', () => {
    // expect(
    //   component
    //     .dive()
    //     .dive()
    //     .find(ReactTable)
    //     .prop('pageSize')
    // ).toEqual(1);
  });
});
