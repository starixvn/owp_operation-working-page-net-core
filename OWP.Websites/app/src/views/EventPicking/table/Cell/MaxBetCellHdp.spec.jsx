import MaxBetCellHdp from './MaxBetCellHdp';
import { render, screen } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import '@testing-library/jest-dom';
const middleWares = [];
const mockStore = configureStore(middleWares);


const store = mockStore({
  
});
const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;


const props = {
  dataSource: [
    {
      maxbetHdp: '10000',
      rowColor: 'red'
    }
  ],
  rowIndex: 0
};

describe('MaxBetCell under test', () => {
  it('render cell', () => {
    render(<Wrapper><MaxBetCellHdp {...props} /></Wrapper>);
    const text = screen.getAllByText('10K');
    expect(text).toBeTruthy();
  });
});
