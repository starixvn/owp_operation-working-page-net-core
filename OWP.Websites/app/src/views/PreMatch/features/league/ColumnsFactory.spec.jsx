import { REQUEST_STATUS } from '../../../Shared/Constants';
import ColumnsFactory from './ColumnsFactory';

jest.mock('../../app/store');

describe('ColumnsFactory under test', () => {
  it('when isEditing: false, should contain columns  "No#", dont contain "Action"', () => {
    const initialState = {
      entities: [
        {
          PreMatchLeagueId: 4,
          LeagueName: '0ACWIJ912J34ACGTK86R79UCHOS4XZE02Y6SXDPUTRW9PBCR6C',
          HDP: '5KR3OCF3N0',
          OU: 'STTP5JSIWI'
        }
      ],
      originDataSource: [],
      prematchIdUpdated: [],
      loading: REQUEST_STATUS.idle,
      sortState: {},
      isEditing: false
    };
    const dataSource = [
      {
        PreMatchLeagueId: 4,
        LeagueName: '0ACWIJ912J34ACGTK86R79UCHOS4XZE02Y6SXDPUTRW9PBCR6C',
        HDP: '5KR3OCF3N0',
        OU: 'STTP5JSIWI'
      }
    ];
    const columnWidth = {
      Action: 100,
      No: 100,
      League: 100,
      HDP: 100,
      Issues: 100,
      OU: 100,
      HDPTakeOver: 100,
      OUTakeOver: 100,
      MaxBetHDP: 100,
      MaxBetOU: 100,
      BlackList: 100,
      Note: 100,
      Remark: 100
    };

    const columns = ColumnsFactory(dataSource, initialState, columnWidth);
    const isRenderNoColumn = columns.filter(e => e.key === 'No').length;
    const isRenderActionColumn = columns.filter(e => e.key === 'Action').length;
    expect(isRenderNoColumn).toEqual(1);
    expect(isRenderActionColumn).toEqual(0);
  });

  it('when isEditing: true, should dont contain columns  "No#",  contain "Action"', () => {
    const initialState = {
      entities: [
        {
          PreMatchLeagueId: 4,
          LeagueName: '0ACWIJ912J34ACGTK86R79UCHOS4XZE02Y6SXDPUTRW9PBCR6C',
          HDP: '5KR3OCF3N0',
          OU: 'STTP5JSIWI'
        }
      ],
      originDataSource: [],
      prematchIdUpdated: [],
      loading: REQUEST_STATUS.idle,
      sortState: {},
      isEditing: true
    };
    const dataSource = [
      {
        PreMatchLeagueId: 4,
        LeagueName: '0ACWIJ912J34ACGTK86R79UCHOS4XZE02Y6SXDPUTRW9PBCR6C',
        HDP: '5KR3OCF3N0',
        OU: 'STTP5JSIWI'
      }
    ];
    const columnWidth = {
      Action: 100,
      No: 100,
      League: 100,
      HDP: 100,
      Issues: 100,
      OU: 100,
      HDPTakeOver: 100,
      OUTakeOver: 100,
      MaxBetHDP: 100,
      MaxBetOU: 100,
      BlackList: 100,
      Note: 100,
      Remark: 100
    };

    const columns = ColumnsFactory(dataSource, initialState, columnWidth);
    const isRenderNoColumn = columns.filter(e => e.key === 'No').length;
    const isRenderActionColumn = columns.filter(e => e.key === 'Action').length;
    expect(isRenderNoColumn).toEqual(0);
    expect(isRenderActionColumn).toEqual(1);
  });
});
