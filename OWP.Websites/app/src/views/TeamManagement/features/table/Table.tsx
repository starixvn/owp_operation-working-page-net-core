import { useAppSelector } from '../../app/hookNative';
import { isEquals } from 'immutability-helper';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import Loading from '../../../components/Loading';
import TableHeader from './TableHeader';
import useLayoutTable from '../../../Shared/customHook/useLayoutTable';
import { useRef, useMemo } from 'react';
import TableBody from './TableBody';
import { useVirtualizer } from '@tanstack/react-virtual';

const ROW_HEIGHT = 36;

const Table = () => {
  const { leagues, loadingDataTable } = useAppSelector(
    state => state.leagues
  );
  const { maxHeight, maxWidth } = useLayoutTable();
  const parentRef = useRef<HTMLDivElement>(null);

  const totalRows = useMemo(() => {
    return leagues.reduce((total, league) => {
      let leagueRows = 1;
      if (league.IsExpanded) {
        leagueRows += league.Teams.length;
      }
      return total + leagueRows;
    }, 0);
  }, [leagues]);

  const flattenedRows = useMemo(() => {
    const rows: Array<{ type: 'league' | 'team', leagueIndex: number, teamIndex?: number, data: any; }> = [];

    leagues.forEach((league, leagueIndex) => {
      rows.push({
        type: 'league',
        leagueIndex,
        data: league
      });

      if (league.IsExpanded) {
        league.Teams.forEach((team, teamIndex) => {
          rows.push({
            type: 'team',
            leagueIndex,
            teamIndex,
            data: team
          });
        });
      }
    });

    return rows;
  }, [leagues]);

  const rowVirtualizer = useVirtualizer({
    count: totalRows,
    getScrollElement: () => parentRef.current,
    estimateSize: () => ROW_HEIGHT,
    overscan: 5
  });

  if (isEquals(loadingDataTable, REQUEST_STATUS.pending)) {
    return <Loading />;
  }

  return (
    <>
      <div
        ref={parentRef}
        className='table-wrap'
        style={{
          maxHeight,
          maxWidth,
          overflow: 'auto'
        }}>
        {!isEquals(loadingDataTable, 'first-time') &&
          <table className='table-team-management'>
            <TableHeader />
            <TableBody
              dataSource={leagues}
              virtualizer={rowVirtualizer}
              flattenedRows={flattenedRows}
            />
          </table>}
      </div>
      <div className='total-record'>Total record: {totalRows}</div>
    </>
  );
};

export default Table;
