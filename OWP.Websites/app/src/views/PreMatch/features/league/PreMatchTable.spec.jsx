import { useState, createRef } from 'react';
import { render } from '@testing-library/react';
import PreMatchTable from './PreMatchTable';
import { useDispatch, useSelector } from 'react-redux';
import ColumnsFactory from './ColumnsFactory';
import { calculateRowHight, calculateTableWidth } from './PrematchTableHelper';

jest.mock('../../app/store');
jest.mock('fixed-data-table-2');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  createRef: jest.fn()
}));
jest.mock('./ColumnsFactory');
jest.mock('./PrematchTableHelper.jsx');
const mockClickRef = jest.fn();
const mockDispatch = jest.fn();
const setState = jest.fn();
const ref = { current: { value: 'Hello', click: mockClickRef } };
const mockCallBack = jest.fn();
const calculateRowHightMock = jest.fn();
const calculateTableWidthMock = jest.fn();
const setup = status => {
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
          OtherHDP: null,
          OtherOUN: 'test note',
          OtherHDPTakeOver: null,
          OtherOUTakeOver: null,
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
          OtherHDP: '#starix14 note1 note2, #starix1 <testing>',
          OtherOU: '#starix14 note1 note2, #starix1 <testing>',
          OtherHDPTakeOver: null,
          OtherOUTakeOver: null,
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
          OtherHDP: '#sa',
          OtherOU: null,
          OtherHDPTakeOver: null,
          OtherOUTakeOver: null,
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
          OtherHDP: '#starix14 bajd',
          OtherOU: null,
          OtherHDPTakeOver: null,
          OtherOUTakeOver: null,
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
          OtherHDP: null,
          OtherOU:
            'aaa bbb ccc dddd eeee fffff qqqqqq teeeeee tesssss tsssssss',
          OtherHDPTakeOver: null,
          OtherOUTakeOver: null,
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
          OtherHDP: null,
          OtherOUN: 'test note',
          OtherHDPTakeOver: null,
          OtherOUTakeOver: null,
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
          OtherHDP: '#starix14 note1 note2, #starix1 <testing>',
          OtherOU: '#starix14 note1 note2, #starix1 <testing>',
          OtherHDPTakeOver: null,
          OtherOUTakeOver: null,
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
          OtherHDP: '#sa',
          OtherOU: null,
          OtherHDPTakeOver: null,
          OtherOUTakeOver: null,
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
          OtherHDP: '#starix14 bajd',
          OtherOU: null,
          OtherHDPTakeOver: null,
          OtherOUTakeOver: null,
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
          OtherHDP: null,
          OtherOU:
            'aaa bbb ccc dddd eeee fffff qqqqqq teeeeee tesssss tsssssss',
          OtherHDPTakeOver: null,
          OtherOUTakeOver: null,
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
      loading: status
    },
    filter: {
      filterDate: new Date()
    }
  };

  useSelector.mockImplementation(selectorFn => selectorFn({ ...state }));
};
const createProps = () => {
  return {
    columnKey: 'No',
    headerName: 'LeagueName',
    color: 'none',
    onOrder: { mockCallBack },
    sortState: {
      columnKey: 'No'
    },
    height: 750,
    isEditing: false,
    containerWidth: 1700
  };
};
const { ResizeObserver } = window;
createRef.mockReturnValue(ref);
describe('PreMatchTable under test', () => {
  beforeEach(() => {
    useState.mockImplementation(init => [init, setState]);
    useDispatch.mockReturnValue(mockDispatch);
    ColumnsFactory.mockReturnValue([]);
    calculateTableWidth.mockReturnValue(calculateTableWidthMock);
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

  it('when waiting query data, should show loading', () => {
    setup('pending');
    const props = createProps();
    const { container } = render(<PreMatchTable {...props} />);
    const loading = container.querySelector('.loading-process');
    expect(loading).toBeInTheDocument();
  });
  // it('should render 1 table, 1 Total League', () => {
  //   setup();
  //   const props = createProps();
  //   const { container } = render(<PreMatchTable {...props} />);
  //   const table = container.querySelector('.table');
  //   expect(table).toBeInTheDocument(2);
  // });

  // it('table should have call ColumnsFactory ', () => {
  //   setup();
  //   const props = createProps();
  //   render(<PreMatchTable {...props} />);
  //   expect(ColumnsFactory).toBeCalledTimes(1);
  // });

  // it('press ESC, should call dispatch ', () => {
  //   setup();
  //   const props = createProps();
  //   const eventMap = {};
  //   document.addEventListener = jest.fn((event, cb) => {
  //     eventMap[event] = cb;
  //   });
  //   document.removeEventListener = jest.fn((event, cb) => {
  //     eventMap[event] = cb;
  //   });
  //   render(<PreMatchTable {...props} />);
  //   eventMap.keydown({ keyCode: 27 });
  //   expect(mockDispatch).toHaveBeenCalledTimes(1);
  // });
});
