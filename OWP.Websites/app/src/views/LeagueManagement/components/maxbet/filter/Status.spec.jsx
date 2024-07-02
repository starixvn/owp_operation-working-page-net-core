import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import Status from './Status';
import '@testing-library/jest-dom';

const middleWares = [];
const mockStore = configureStore(middleWares);

const initialState = {
  statusReducer: {
    status: ''
  }
};
const store = mockStore(initialState);
jest.spyOn(store, 'dispatch');

const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

describe('test Status component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });

  it('should change select component', async () => {
    render(
      <Wrapper>
        <Status />
      </Wrapper>
    );
    fireEvent.change(document.getElementById('react-select-2-input'), {
      target: { value: 'Active' }
    });

    const option = screen.getByText('Inactive');
    fireEvent.click(option);
    expect(store.dispatch).toBeCalled();
  });

  it('should render component', () => {
    const { getByText } = render(
      <Wrapper>
        <Status />
      </Wrapper>
    );
    expect(getByText('Status')).toBeInTheDocument();
  });
});
