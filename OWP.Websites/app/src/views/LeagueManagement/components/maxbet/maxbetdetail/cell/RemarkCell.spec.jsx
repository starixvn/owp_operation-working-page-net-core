import { Provider } from 'react-redux';
import { render, fireEvent } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import RemarkCell from './RemarkCell';
import '@testing-library/jest-dom';

const middleWares = [];
const mockStore = configureStore(middleWares);

const initialState = {
  leagueCategoryReducer: {
    leagueCategory: ''
  },
  leagueReducer: {
    league: ''
  },
  leagueGroupReducer: {
    leagueGroupPending: true,
    leagueGroupOption: [],
    leagueGroupBackup: {},
    leagueGroupError: {}
  },
  statusReducer: {
    status: ''
  },
  displayOldLeagueReducer: {
    displayOldLeague: ''
  },
  maxbetGeneralReducer: {
    displayMaxbetGeneralTable: {}
  },
  policyReducer: {
    policyData: {
      tvListPolicy: {
        HasRead: true
      },
      generalPolicy: {
        Username: 'sa',
        HasRead: true
      },
      conditionPolicy: {
        HasRead: true
      },
      lstNotePolicy: {
        HasRead: true,
        HasWrite: true
      }
    }
  }
};
const store = mockStore(initialState);
jest.spyOn(store, 'dispatch');

jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useParams: jest.fn()
}));

const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

describe('test NoOfHdpCell component', () => {
  const props = {
    data: {
      Remark: 'abc'
    },
    rowIndex: 0
  };
  afterAll(() => {
    jest.clearAllMocks();
  });
  it('render correct', () => {
    const { container, getByText } = render(
      <Wrapper>
        <RemarkCell {...props} />
      </Wrapper>
    );
    expect(getByText('abc')).toBeInTheDocument();
  });

  it('should render input when click cell', () => {
    const { container } = render(
      <Wrapper>
        <RemarkCell {...props} />
      </Wrapper>
    );
    fireEvent.click(container.querySelector('.col-remark'));

    const input = container.querySelector('.input-field');
    expect(input).toBeInTheDocument();

    fireEvent.change(input, { target: { value: 'av' } });
    fireEvent.blur(input);

    expect(store.dispatch).toBeCalled();
  });
});
