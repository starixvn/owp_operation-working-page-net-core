import TableAssignment from './TableAssignment';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import { render } from '@testing-library/react';
const middleWares = [];
const mockStore = configureStore(middleWares);

const store = mockStore({
  tableReducer: {
    filterDate: 'abc'
  }
});
const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

describe('TableAssignment', () => {
  const policy = { HasRead: false, HasWrite: false };
  const dataSource = [
    {
      TotalRows: 1,
      DataSource: [
        {
          OWPMatchId: 1,
          Name: 'HDP',
          MaxBetFT: {
            MaxBet: '10K',
            Spread: '12%'
          },
          MaxBetHT: {
            MaxBet: '10K',
            Spread: '12%'
          },
          FT: {
            main: {},
            others: [],
            isAllowAdd: true
          },
          HT: {
            main: {},
            others: [],
            isAllowAdd: true
          },
          LA: {
            main: {},
            others: [],
            isAllowAdd: true
          }
        },
        {
          OWPMatchId: 1,
          Name: 'OU',
          MaxBetFT: {
            MaxBet: '10K',
            Spread: '12%'
          },
          MaxBetHT: {
            MaxBet: '10K',
            Spread: '12%'
          },
          FT: {
            main: {},
            others: [],
            isAllowAdd: true
          },
          HT: {
            main: {},
            others: [],
            isAllowAdd: true
          },
          LA: {
            main: {},
            others: [],
            isAllowAdd: true
          }
        }
      ]
    }
  ];

  beforeAll(() => {
    document.body.innerHTML = '<div id="root"></div>';
  });

  it('should render component', () => {
    const props = {
      policy,
      dataSource
    };
    const { container } = render(
      <Wrapper>
        <TableAssignment {...props} />
      </Wrapper>
    );
    expect(container.firstChild).toMatchSnapshot();
  });
});
