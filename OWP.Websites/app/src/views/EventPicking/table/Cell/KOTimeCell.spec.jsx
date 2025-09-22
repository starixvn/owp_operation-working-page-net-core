import KOTimeCell from './KOTimeCell';
import { render, screen } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';

const middlewares = [];
const mockStore = configureStore(middlewares);

const props = {
  dataSource: [
    {
      eventDateTime: '2022-11-07T20:00:00',
      rowColor: 'red',
      time: '08:00 PM',
      isNoOffer: false,
      infoSiteMatchId: 1,
      isChildMatch: false,
      childMatchId: null
    }
  ],
  rowIndex: 0,
  permission: {
    HasWrite: true
  }
};

const Wrapper = ({ children }) => {
  const store = mockStore({});
  return <Provider store={store}>{children}</Provider>;
};

// Mock the custom hook
jest.mock('../../../Shared/customHook/useStableDispatch', () => {
  return jest.fn(() => jest.fn());
});

describe('KOTimeCell under test', () => {
  it('render cell', () => {
    render(<KOTimeCell {...props} />, { wrapper: Wrapper });
    const text = screen.getAllByText('08:00 PM');
    expect(text).toBeTruthy();
  });
});
