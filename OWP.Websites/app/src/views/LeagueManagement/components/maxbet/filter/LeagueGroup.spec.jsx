import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import LeagueGroup from './LeagueGroup';
import '@testing-library/jest-dom';
import MaxbetConst from '../../../constants/MaxbetConst';
import { queryData, updateExpressionFilter } from '../../../actions/action';
import {
  QUERY_LEAGUEGROUP,
  ACTION_LEAGUEGROUP_FILTER
} from '../../../constants/action-type';

const middleWares = [];
const mockStore = configureStore(middleWares);

const initialState = {
  leagueGroupReducer: {
    leagueGroupPending: true,
    leagueGroupOption: [],
    leagueGroupBackup: {},
    leagueGroupError: {}
  }
};
const store = mockStore(initialState);
jest.spyOn(store, 'dispatch');

const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

describe('test LeagueGroup component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });

  it('should render loading component', () => {
    const { container } = render(
      <Wrapper>
        <LeagueGroup />
      </Wrapper>
    );
    expect(container.querySelector('.loading-process')).toBeInTheDocument();
  });
});
