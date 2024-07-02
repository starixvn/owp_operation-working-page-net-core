import { Provider } from 'react-redux';
import { render, fireEvent } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import OldLeague from './OldLeague';

const middleWares = [];
const mockStore = configureStore(middleWares);

const initialState = {
  displayOldLeagueReducer: {
    displayOldLeague: ''
  }
};
const store = mockStore(initialState);
jest.spyOn(store, 'dispatch');

const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

describe('test OldLeague component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });

  it('should render component', () => {
    const { getByText } = render(
      <Wrapper>
        <OldLeague />
      </Wrapper>
    );
    expect(getByText('Include Old Leagues')).toBeInTheDocument();
  });

  it('should render change event component', () => {
    const { container } = render(
      <Wrapper>
        <OldLeague />
      </Wrapper>
    );
    const input = container.querySelector('.form-check-input');

    fireEvent.click(input);

    expect(store.dispatch).toBeCalled();
  });
});
