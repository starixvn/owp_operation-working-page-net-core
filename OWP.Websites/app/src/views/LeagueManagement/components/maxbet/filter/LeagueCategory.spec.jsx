import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import LeagueCategory from './LeagueCategory';
import '@testing-library/jest-dom';

const middleWares = [];
const mockStore = configureStore(middleWares);

const initialState = {
  leagueCategoryReducer: {
    leagueCategory: ''
  }
};
const store = mockStore(initialState);
jest.spyOn(store, 'dispatch');

const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

describe('test LeagueCategory component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });

  it('should change select component', async () => {
    render(
      <Wrapper>
        <LeagueCategory />
      </Wrapper>
    );
    fireEvent.change(document.getElementById('react-select-2-input'), {
      target: { value: 'Today' }
    });

    const options = screen.getAllByText('Today');
    fireEvent.click(options[1]);
    expect(store.dispatch).toBeCalled();
  });

  it('should render Category component', () => {
    const { getByText } = render(
      <Wrapper>
        <LeagueCategory />
      </Wrapper>
    );
    expect(getByText('Category')).toBeInTheDocument();
  });
});
