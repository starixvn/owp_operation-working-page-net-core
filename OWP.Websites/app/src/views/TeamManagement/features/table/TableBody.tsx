import ReadonlyCommonCell from './cell/ReadonlyCommonCell';
import { TLeague } from '../../type/table';
import tableSchema from './TableSchema.json';
import { Virtualizer } from '@tanstack/react-virtual';
import { useAppDispatch } from '../../app/hookNative';
import { toggleLeagueExpansion } from './tableSlice';

interface TableBodyProps {
  dataSource: TLeague[];
  virtualizer: Virtualizer<HTMLDivElement, Element>;
  flattenedRows: Array<{ type: 'league' | 'team', leagueIndex: number, teamIndex?: number, data: any; }>;
}

const TableBody = ({ dataSource, virtualizer, flattenedRows }: TableBodyProps) => {
  const dispatch = useAppDispatch();

  const handleToggleLeague = (leagueId: number) => {
    dispatch(toggleLeagueExpansion(leagueId));
  };

  return (
    <tbody>
      <tr style={{ height: `${virtualizer.getTotalSize()}px` }}>
        <td colSpan={2} style={{ padding: 0 }}>
          <div className='table-content-wrapper'>
            <div
              className='table-virtual-content'
              style={{
                position: 'absolute',
                top: 0,
                left: 0,
                right: 0,
                transform: `translateY(${virtualizer.getVirtualItems()[0]?.start ?? 0}px)`
              }}>
              {virtualizer.getVirtualItems().map(virtualRow => {
                const rowData = flattenedRows[virtualRow.index];

                if (!rowData) return null;

                if (rowData.type === 'league') {
                  const league = rowData.data as TLeague;
                  return (
                    <div
                      key={`league-${league.LeagueId}`}
                      className='table-row league-header-row'
                      onClick={() => handleToggleLeague(league.LeagueId)}
                      style={{
                        cursor: 'pointer',
                        height: `${virtualRow.size}px`
                      }}
                    >
                      <div className='table-cell'>
                        <div className="league-header-content">
                          <button
                            onClick={(e) => {
                              e.stopPropagation();
                              handleToggleLeague(league.LeagueId);
                            }}
                            className='expand-button'
                          >
                            {league.IsExpanded ? '▼' : '▶'}
                          </button>
                          <span>
                            {league.LeagueName}
                          </span>
                        </div>
                      </div>
                    </div>
                  );
                } else if (rowData.type === 'team') {
                  const team = rowData.data;
                  const teamIndex = rowData.teamIndex!;

                  return (
                    <div
                      key={`team-${team.TeamId}`}
                      className='table-row team-row'
                      style={{
                        height: `${virtualRow.size}px`
                      }}>
                      {tableSchema.TABLE_HEADER.map((header, idx) => (
                        <div
                          key={header.HEADER_KEY}
                          className='table-cell'
                          style={{
                            width: header.WIDTH
                          }}>
                          {header.HEADER_KEY === 'No' && (
                            <ReadonlyCommonCell
                              className='col-no'
                              value={(teamIndex + 1).toString()}
                              index={teamIndex}
                              isCanToolTip={false}
                            />
                          )}
                          {header.HEADER_KEY === 'Team' && (
                            <ReadonlyCommonCell
                              className='col-team'
                              value={team.TeamName}
                              index={teamIndex}
                              isCanToolTip={false}
                            />
                          )}
                        </div>
                      ))}
                    </div>
                  );
                }

                return null;
              })}
            </div>
          </div>
        </td>
      </tr>
    </tbody>
  );
};

export default TableBody;
