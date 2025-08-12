import { useState, createRef } from 'react';
import { fireEvent, render, waitFor } from '@testing-library/react';
import DayShiftManagement from './DayShiftManagement';
import { useDispatch, useSelector } from 'react-redux';
import usePermission from '../Shared/customHook/usePermission';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  createRef: jest.fn()
}));

jest.mock('../Shared/customHook/usePermission');
jest.mock('./ImportPrematchHelperNative');
const mockDispatch = jest.fn();
const setState = jest.fn();
const ref = { current: { value: 'Hello' } };
const setup = (status, mockPermission, isEditMode = false) => {
  const state = {
    dayShift: {
      entities: [
        {
          No: 1,
          LeagueId: 23940,
          PreMatchLeagueId: 9816,
          LeagueName: 'AUSTRALIA GOLD COAST MIDWEEK METRO DIVISION 1',
          MatchCount: 1,
          HDP: 'sa',
          OU: 'sa',
          HDPTakeOver: null,
          OUTakeOver: null,
          OtherHDPNote: null,
          OtherOUNote: 'test note',
          OtherHDPTakeOverNote: null,
          OtherOUTakeOverNote: null,
          Spread: null,
          Maxbet: null,
          Blacklist: false,
          Remark: null,
          HasMessages: false,
          IsRead: false,
          PreMatchIssueId: null,
          HasNotes: true,
          PreMatchNoteId: 117,
          isExpand: false
        },
        {
          No: 2,
          LeagueId: 31969,
          PreMatchLeagueId: 9992,
          LeagueName: 'BAHRAIN KING CUP',
          MatchCount: 1,
          HDP: 'sa',
          OU: 'sa',
          HDPTakeOver: null,
          OUTakeOver: null,
          OtherHDPNote: '#starix14 note1 note2, #starix1 <testing>',
          OtherOUNote: '#starix14 note1 note2, #starix1 <testing>',
          OtherHDPTakeOverNote: null,
          OtherOUTakeOverNote: null,
          Spread: null,
          Maxbet: null,
          Blacklist: false,
          Remark: null,
          HasMessages: false,
          IsRead: false,
          PreMatchIssueId: null,
          HasNotes: false,
          PreMatchNoteId: null,
          isExpand: false
        },
        {
          No: 3,
          LeagueId: 31063,
          PreMatchLeagueId: 9892,
          LeagueName: 'CHILE PRIMERA B',
          MatchCount: 6,
          HDP: 'sa',
          OU: null,
          HDPTakeOver: null,
          OUTakeOver: null,
          OtherHDPNote: '#sa',
          OtherOUNote: null,
          OtherHDPTakeOverNote: null,
          OtherOUTakeOverNote: null,
          Spread: null,
          Maxbet: null,
          Blacklist: false,
          Remark: null,
          HasMessages: false,
          IsRead: false,
          PreMatchIssueId: null,
          HasNotes: false,
          PreMatchNoteId: null,
          isExpand: false
        },
        {
          No: 4,
          LeagueId: 18,
          PreMatchLeagueId: 9744,
          LeagueName: 'CLUB FRIENDLY',
          MatchCount: 2,
          HDP: 'sa',
          OU: null,
          HDPTakeOver: null,
          OUTakeOver: null,
          OtherHDPNote: '#starix14 bajd',
          OtherOUNote: null,
          OtherHDPTakeOverNote: null,
          OtherOUTakeOverNote: null,
          Spread: null,
          Maxbet: null,
          Blacklist: true,
          Remark: null,
          HasMessages: false,
          IsRead: false,
          PreMatchIssueId: null,
          HasNotes: false,
          PreMatchNoteId: null,
          isExpand: false
        },
        {
          No: 5,
          LeagueId: 31866,
          PreMatchLeagueId: 9966,
          LeagueName: 'BHUTAN PREMIER LEAGUE',
          MatchCount: 1,
          HDP: null,
          OU: 'sa',
          HDPTakeOver: null,
          OUTakeOver: null,
          OtherHDPNote: null,
          OtherOUNote:
            'aaa bbb ccc dddd eeee fffff qqqqqq teeeeee tesssss tsssssss',
          OtherHDPTakeOverNote: null,
          OtherOUTakeOverNote: null,
          Spread: null,
          Maxbet: null,
          Blacklist: false,
          Remark: null,
          HasMessages: false,
          IsRead: false,
          PreMatchIssueId: null,
          HasNotes: false,
          PreMatchNoteId: null,
          isExpand: false
        }
      ],
      leagues: [
        {
          No: 1,
          LeagueId: 23940,
          PreMatchLeagueId: 9816,
          LeagueName: 'AUSTRALIA GOLD COAST MIDWEEK METRO DIVISION 1',
          MatchCount: 1,
          HDP: 'sa',
          OU: 'sa',
          HDPTakeOver: null,
          OUTakeOver: null,
          OtherHDPNote: null,
          OtherOUNote: 'test note',
          OtherHDPTakeOverNote: null,
          OtherOUTakeOverNote: null,
          Spread: null,
          Maxbet: null,
          Blacklist: false,
          Remark: null,
          HasMessages: false,
          IsRead: false,
          PreMatchIssueId: null,
          HasNotes: true,
          PreMatchNoteId: 117,
          isExpand: false
        },
        {
          No: 2,
          LeagueId: 31969,
          PreMatchLeagueId: 9992,
          LeagueName: 'BAHRAIN KING CUP',
          MatchCount: 1,
          HDP: 'sa',
          OU: 'sa',
          HDPTakeOver: null,
          OUTakeOver: null,
          OtherHDPNote: '#starix14 note1 note2, #starix1 <testing>',
          OtherOUNote: '#starix14 note1 note2, #starix1 <testing>',
          OtherHDPTakeOverNote: null,
          OtherOUTakeOverNote: null,
          Spread: null,
          Maxbet: null,
          Blacklist: false,
          Remark: null,
          HasMessages: false,
          IsRead: false,
          PreMatchIssueId: null,
          HasNotes: false,
          PreMatchNoteId: null,
          isExpand: false
        },
        {
          No: 3,
          LeagueId: 31063,
          PreMatchLeagueId: 9892,
          LeagueName: 'CHILE PRIMERA B',
          MatchCount: 6,
          HDP: 'sa',
          OU: null,
          HDPTakeOver: null,
          OUTakeOver: null,
          OtherHDPNote: '#sa',
          OtherOUNote: null,
          OtherHDPTakeOverNote: null,
          OtherOUTakeOverNote: null,
          Spread: null,
          Maxbet: null,
          Blacklist: false,
          Remark: null,
          HasMessages: false,
          IsRead: false,
          PreMatchIssueId: null,
          HasNotes: false,
          PreMatchNoteId: null,
          isExpand: false
        },
        {
          No: 4,
          LeagueId: 18,
          PreMatchLeagueId: 9744,
          LeagueName: 'CLUB FRIENDLY',
          MatchCount: 2,
          HDP: 'sa',
          OU: null,
          HDPTakeOver: null,
          OUTakeOver: null,
          OtherHDPNote: '#starix14 bajd',
          OtherOUNote: null,
          OtherHDPTakeOverNote: null,
          OtherOUTakeOverNote: null,
          Spread: null,
          Maxbet: null,
          Blacklist: true,
          Remark: null,
          HasMessages: false,
          IsRead: false,
          PreMatchIssueId: null,
          HasNotes: false,
          PreMatchNoteId: null,
          isExpand: false
        },
        {
          No: 5,
          LeagueId: 31866,
          PreMatchLeagueId: 9966,
          LeagueName: 'BHUTAN PREMIER LEAGUE',
          MatchCount: 1,
          HDP: null,
          OU: 'sa',
          HDPTakeOver: null,
          OUTakeOver: null,
          OtherHDPNote: null,
          OtherOUNote:
            'aaa bbb ccc dddd eeee fffff qqqqqq teeeeee tesssss tsssssss',
          OtherHDPTakeOverNote: null,
          OtherOUTakeOverNote: null,
          Spread: null,
          Maxbet: null,
          Blacklist: false,
          Remark: null,
          HasMessages: false,
          IsRead: false,
          PreMatchIssueId: null,
          HasNotes: false,
          PreMatchNoteId: null,
          isExpand: false
        }
      ],
      sortState: {
        columnKey: ''
      }
    },
    policy: {
      permission: { Username: 'sa', HasRead: true, HasWrite: true }
    },
    editor: {
      isEdit: isEditMode
    },
    filter: {
      filterDate: ''
    },
    header: {
      info: {}
    },
    chatBoard: {
      isOpenTicket: false,
      modelByRowIndex: []
    },
    createTicketBoard: {
      selectedRecipients: [],
      subTypeSelected: {}
    },
    deletedLeague: {
      loading: false
    },
    copy: {
      highlightedCell: {
        PreMatchLeagueId: [9966]
      }
    },
    auto: {
      autoMode: false,
      assignments: []
    }
  };
  useState.mockImplementation(init => [init, setState]);
  useDispatch.mockReturnValue(mockDispatch);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...state }));
  createRef.mockReturnValue(ref);
  usePermission.mockReturnValue({
    permission: mockPermission,
    loading: status,
    error: null
  });
};
const { ResizeObserver } = window;

describe('DayShiftManagement under test', () => {
  beforeEach(() => {
    delete window.ResizeObserver;
    window.ResizeObserver = jest.fn().mockImplementation(() => ({
      observe: jest.fn(),
      unobserve: jest.fn(),
      disconnect: jest.fn()
    }));
  });
  afterEach(() => {
    window.ResizeObserver = ResizeObserver;
    jest.restoreAllMocks();
  });
  it('when waiting query data, should show Loading', () => {
    const mockPermission = {
      Username: 'sa',
      HasRead: true,
      HasWrite: true
    };
    setup('pending', mockPermission);
    const { container } = render(<DayShiftManagement />);
    const loading = container.querySelector('.loading-process');
    expect(loading).toBeInTheDocument();
  });
  it('should render 1 table, 1 blockFilter, 1 tablePreMatch,1 preMatchViewChatBoard (not show on UI)', () => {
    const mockPermission = {
      Username: 'sa',
      HasRead: true,
      HasWrite: true
    };
    setup('idle', mockPermission);
    const { container } = render(<DayShiftManagement />);
    const blockFilter = container.querySelector('.block-filter');

    expect(blockFilter).toBeInTheDocument();
  });

  it('should render 1 table, 1 blockFilter, 1 tablePreMatch,1 SortDefault', async () => {
    const mockPermission = {
      Username: 'sa',
      HasRead: true,
      HasWrite: true
    };
    setup('idle', mockPermission, true);
    const { container } = render(<DayShiftManagement />);

    const blockFilter = container.querySelector('.block-button ');

    await waitFor(() => {
      expect(blockFilter).toBeInTheDocument();
    });
  });
});
