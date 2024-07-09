import { useDispatch, useSelector } from 'react-redux';
import { render } from '@testing-library/react';
import LeagueCellFactory from './LeagueCellFactory';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
const setup = () => {
  const state = {
    policy: {
      permission: {
        HasWrite: true
      }
    },
    dayShift: {
      isExpandAll: true,
      leagueWithTimeCondition:{
        leagueInfos:[]
      }
    }
  };
  useDispatch.mockReturnValue(mockDispatch);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...state }));
};

const createProps = InfoSiteMatchId => {
  const dataSource = [
    {
      InfoSiteMatchId,
      LeagueName: 'LeagueNameTest',
      PreMatchLeagueId: 1,
      isExpand: true,
      MatchCount: 1
    }
  ];
  const rowIndex = 0;
  return { dataSource, rowIndex };
};

describe('LeagueCellFactory under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('when InfoSiteMatchId null, should render LeagueCell', () => {
    setup();
    const props = createProps(null);
    const { container } = render(<LeagueCellFactory {...props} />);
    const element = container.querySelector('.league-row');
    expect(element).toBeInTheDocument();
  });

  it('when InfoSiteMatchId not null, should render MatchCell', () => {
    setup();
    const props = createProps(1);
    const { container } = render(<LeagueCellFactory {...props} />);
    const element = container.querySelector('.match-row');
    expect(element).toBeInTheDocument();
  });
});
