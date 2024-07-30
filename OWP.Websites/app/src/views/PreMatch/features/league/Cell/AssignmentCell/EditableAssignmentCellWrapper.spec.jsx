import { useSelector, useDispatch, useStore } from 'react-redux';
import { fireEvent, render } from '@testing-library/react';
import EditableAssignmentCellWrapper from './EditableAssignmentCellWrapper';
import axios from 'axios';
import Spinner from '../../../../../components/Spinner';
import { noop } from 'lodash';
import ToastrWrapper from '../../../../../components/ToastrWrapper';

jest.mock('axios');
jest.mock('./AssignmentHelper.jsx');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn()
}));

const mockedDispatch = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);
const store = {
  dayShift: {
    leagues: [
      {
        LeagueId: 1
      }
    ]
  }
};

const mockStore = {
  getState: () => store
};
useStore.mockReturnValue(mockStore);
const setUp = props => {
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
};

jest.mock('../../../../../components/Spinner');
Spinner.show.mockImplementation(() => noop());
Spinner.hide.mockImplementation(() => noop());

jest.mock('../../../../../components/ToastrWrapper');
ToastrWrapper.showInfo.mockImplementation(() => noop());

describe('EditableAssignmentCellWrapper under test', () => {
  afterEach(() => {
    axios.get.mockReset();
    axios.post.mockReset();
    jest.clearAllMocks();
  });
  it('should call dispatch pasteCopiedValue when paste', () => {
    setUp({
      copy: {
        highlightedCell: {
          PreMatchLeagueId: [4],
          isTakeOver: true,
          isCopying: true
        },
        copiedValue: {
          OU: 'sa',
          HDP: 'sa'
        },
        isCopying: true
      },
      editor: {
        isEdit: true
      }
    });
    const props = {
      columnId: 'league',
      rowIndex: 0,
      dataSource: [
        {
          PreMatchLeagueId: 4,
          InfoSiteMatchId: undefined,
          LeagueId: 1
        }
      ],
      isEditing: true
    };

    const { container } = render(<EditableAssignmentCellWrapper {...props} />);
    const leagueCell = container.querySelector('.league-group');
    fireEvent.click(leagueCell);
    expect(mockedDispatch).toHaveBeenCalledTimes(1);
    expect(mockedDispatch).toBeCalledWith({
      payload: {
        copiedValue: {
          HDP: 'sa',
          OU: 'sa'
        },
        highlightedCell: {
          PreMatchLeagueId: [4],
          isTakeOver: true,
          isCopying: true
        },
        isTakeOver: false,
        leagueIndex: 0,
        rowIndex: 0
      },
      type: 'dayShift/pasteCopiedValue'
    });
  });

  it('should call 2 dispatch when ctrlKey + click', () => {
    setUp({
      copy: {
        highlightedCell: {
          PreMatchLeagueId: [4],
          isTakeOver: true,
          isCopying: true
        },
        copiedValue: {
          OU: 'sa',
          HDP: 'sa'
        },
        isCopying: false
      },
      editor: {
        isEdit: true
      }
    });
    const props = {
      columnId: 'league',
      rowIndex: 0,
      dataSource: [
        {
          PreMatchLeagueId: 4,
          InfoSiteMatchId: undefined
        }
      ],
      isEditing: true
    };

    const { container } = render(<EditableAssignmentCellWrapper {...props} />);
    const leagueCell = container.querySelector('.league-group');
    fireEvent.click(leagueCell, { ctrlKey: true });
    expect(mockedDispatch).toHaveBeenCalledTimes(2);
  });

  it('should render cell empty when InfoSiteMatchId has value', () => {
    setUp({
      copy: {
        highlightedCell: {
          PreMatchLeagueId: [4],
          isTakeOver: true,
          isCopying: true
        },
        copiedValue: {
          OU: 'sa',
          HDP: 'sa'
        },
        isCopying: false
      },
      editor: {
        isEdit: true
      }
    });
    const props = {
      columnId: 'league',
      rowIndex: 0,
      dataSource: [
        {
          PreMatchLeagueId: 4,
          InfoSiteMatchId: 4
        }
      ],
      isEditing: true
    };
    const { container } = render(<EditableAssignmentCellWrapper {...props} />);
    expect(container.querySelector('.cell-empty')).toBeInTheDocument();
  });
});
