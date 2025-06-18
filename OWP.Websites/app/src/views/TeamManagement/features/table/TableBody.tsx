import ReadonlyCommonCell from './cell/ReadonlyCommonCell';
import { TDataSource } from '../../type/table';
import tableSchema from './TableSchema.json';
import { Virtualizer } from '@tanstack/react-virtual';
import { useAppDispatch, useAppSelector } from '../../app/hookNative';
import { toggleLeagueExpansion } from './tableSlice';

interface TableBodyProps {
  dataSource: TDataSource[];
  virtualizer: Virtualizer<HTMLDivElement, Element>;
}

type TLeagueGroup = {
  leagueId: number;
  league: string;
  leagueGroup: string;
  leagueGroupId: number;
  isExpanded: boolean;
  teams: TDataSource[];
};

const TableBody = ({ dataSource, virtualizer }: TableBodyProps) => {
  const dispatch = useAppDispatch();
  const leagueGroups = useAppSelector(state => state.teams.leagueGroups);

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
                position: 'absolute',
                top: 0,
                left: 0,
                right: 0,
                transform: `translateY(${virtualizer.getVirtualItems()[0]?.start ?? 0}px)`
              }}>
              {leagueGroups.map((leagueGroup: TLeagueGroup, leagueIndex: number) => (
                <div key={leagueGroup.leagueId} className="league-group-container">
                  <div
                    className='table-row league-header-row'
                    onClick={() => handleToggleLeague(leagueGroup.leagueId)}
                    style={{ cursor: 'pointer' }}
                  >
                    <div className='table-row table-row-content'>
                      <div className='table-cell' >
                        <div className="league-header-content">
                          <button
                            onClick={(e) => {
                              e.stopPropagation();
                              handleToggleLeague(leagueGroup.leagueId);
                            }}
                            className='expand-button'
                          >
                            {leagueGroup.isExpanded ? '▼' : '▶'}
                          </button>
                          <span>
                            {leagueGroup.league} - {leagueGroup.leagueGroup}
                          </span>
                        </div>
                      </div>
                    </div>
                  </div>

                  {leagueGroup.isExpanded && leagueGroup.teams.map((team: TDataSource, teamIndex: number) => (
                    <div
                      key={`${leagueGroup.leagueId}-${team.teamId}`}
                      className='table-row team-row'>
                      <div className='table-row table-row-content'>
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
                            {header.HEADER_KEY === 'League' && (
                              <ReadonlyCommonCell
                                className='col_league'
                                value={team.league}
                                index={teamIndex}
                                isCanToolTip={false}
                              />
                            )}
                            {header.HEADER_KEY === 'Team' && (
                              <ReadonlyCommonCell
                                className='col_team'
                                value={team.team}
                                index={teamIndex}
                                isCanToolTip={false}
                              />
                            )}
                            {header.HEADER_KEY === 'LeagueGroup' && (
                              <ReadonlyCommonCell
                                className='col_league_group'
                                value={team.leagueGroup}
                                index={teamIndex}
                                isCanToolTip={false}
                              />
                            )}
                          </div>
                        ))}
                      </div>
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
