import { renderHeaderCell } from './createColumn';
import { ColumnModels } from './constants/preMatchConst';
import OrderHeader from './OrderHeader';

const { LeagueColumn } = ColumnModels;
jest.mock('../../app/store');

describe('createColumn under test', () => {
  it('when isCanSort = false, renderHeaderCell should render normal header ', () => {
    const nomalHeaderModel = {
      ...LeagueColumn,
      isCanSort: false
    };

    const result = renderHeaderCell(nomalHeaderModel);

    expect(result).toEqual(LeagueColumn.headerName);
  });
  it('when isCanSort = true, renderHeaderCell should render sortable header ', () => {
    const sortableHeaderModel = {
      ...LeagueColumn,
      sortState: {
        columnKey: 'LeagueName',
        orderType: 'ORDER_ASC'
      },
      isCanSort: true
    };

    const result = renderHeaderCell(sortableHeaderModel);
    const mockSortableHeader = <OrderHeader {...sortableHeaderModel} />;

    expect(result).toEqual(mockSortableHeader);
  });
});
