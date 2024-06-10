import { Provider } from 'react-redux';
import { render, fireEvent } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import League from './League';

const middleWares = [];
const mockStore = configureStore(middleWares);

const initialState = {
  leagueReducer: {
    league: ''
  }
};
const store = mockStore(initialState);
jest.spyOn(store, 'dispatch');

const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

describe('test League component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });

  it('should render component', () => {
    const { getByText } = render(
      <Wrapper>
        <League />
      </Wrapper>
    );
    expect(getByText('League')).toBeInTheDocument();
  });

  it('should render change event component', () => {
    const { container } = render(
      <Wrapper>
        <League />
      </Wrapper>
    );

    const input = container.querySelector('.input-field');

    fireEvent.change(input, {
      target: { value: 'a' }
    });

    expect(store.dispatch).toBeCalled();
  });
});
