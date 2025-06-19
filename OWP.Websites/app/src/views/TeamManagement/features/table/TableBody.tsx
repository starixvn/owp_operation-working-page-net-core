import ReadonlyCommonCell from './cell/ReadonlyCommonCell';
import { TLeague } from '../../type/table';
import tableSchema from './TableSchema.json';
import { Virtualizer } from '@tanstack/react-virtual';
import { useAppDispatch, useAppSelector } from '../../app/hookNative';
import { toggleLeagueExpansion } from './tableSlice';

interface TableBodyProps {
  dataSource: TLeague[];
  virtualizer: Virtualizer<HTMLDivElement, Element>;
}

const TableBody = ({ dataSource, virtualizer }: TableBodyProps) => {
  const dispatch = useAppDispatch();

  const handleToggleLeague = (leagueId: number) => {
    dispatch(toggleLeagueExpansion(leagueId));
  };

  return (
    <tbody>
      <tr style={{ height: `${virtualizer.getTotalSize()}px` }}>
        <td colSpan={11} style={{ padding: 0 }}>
          <div className='table-content-wrapper'>
            <div
              className='table-virtual-content'
              style={{
                transform: `translateY(${virtualizer.getVirtualItems()[0]?.start ?? 0}px)`
              }}>
              {dataSource.map((league: TLeague, leagueIndex: number) => (
                <div key={leagueIndex} className="league-container">
                  <div
                    className='table-row league-header-row'
                    onClick={() => handleToggleLeague(league.LeagueId)}
                    style={{ cursor: 'pointer' }}
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
                          {league.LeagueName} ({league.LeagueGroupName})
                        </span>
                      </div>
                    </div>
                  </div>

                  {league.IsExpanded && league.Teams.map((team, teamIndex: number) => (
                    <div
                      key={`${league.LeagueId}-${team.TeamId}`}
                      className='table-row team-row'>
                      {tableSchema.TABLE_HEADER.map((header, idx) => (
                        <div
                          key={header.HEADER_KEY}
                          className='table-cell'
                          style={{
                            width: header.WIDTH
                          }}>
                          {header.HEADER_KEY === 'No' && (
                            <ReadonlyCommonCell
                              className='col_no'
                              value={(teamIndex + 1).toString()}
                              index={teamIndex}
                              isCanToolTip={false}
                            />
                          )}
                          {header.HEADER_KEY === 'Team' && (
                            <ReadonlyCommonCell
                              className='col_team'
                              value={team.TeamName}
                              index={teamIndex}
                              isCanToolTip={false}
                            />
                          )}
                        </div>
                      ))}
                    </div>
                  ))}
                </div>
              ))}
            </div>
          </div>
        </td>
      </tr>
    </tbody>
  );
};

export default TableBody;
