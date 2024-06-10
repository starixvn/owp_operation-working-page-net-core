import { useEffect, useCallback } from 'react';
import MaxbetConst from '../../../constants/MaxbetConst';
import { useDispatch, useSelector } from 'react-redux';
import { postData } from '../../../actions/action';
import { selectTeamInfo } from '../../../selector/SelectorMaxbetGeneral';
import Loading from '../../../../components/Loading';
import { QUERY_TEAM_INFO } from '../../../constants/action-type';
import Dialog from '../../common/Dialog';

import { buildQuerySimpleApi } from '../../../../Shared/Constants';
import { IPropsTeamInfo, ITeamInfo } from '../../../type';
import { isEqual } from 'lodash';

const TeamInfo = (props: IPropsTeamInfo): JSX.Element => {
  const { owpLeagueId, onClose, leagueName } = props;

  const dispatch = useDispatch();

  const model = useSelector<ITeamInfo, ITeamInfo>(selectTeamInfo);

  const stableDispatch = useCallback(dispatch, [dispatch]);
  useEffect(() => {
    const postModel = {
      apiKey: MaxbetConst.teamInfoAPIKey,
      owpLeagueId
    };
    stableDispatch(postData(buildQuerySimpleApi(), postModel, QUERY_TEAM_INFO));
  }, [stableDispatch, owpLeagueId]);

  const renderBody = () => {
    return (
      <table className='tb-team-info'>
        <tbody>
          <tr>
            <th>No.</th>
            <th>Team Name</th>
          </tr>
          {model.entity.map((item, index) => {
            return (
              <tr key={index}>
                <th>{index + 1}</th>
                <th>{item.OWPTeamName}</th>
              </tr>
            );
          })}
        </tbody>
      </table>
    );
  };
  if (isEqual(model.loading, 'pending')) {
    return <Loading />;
  }

  const propsDialog = {
    titleHeader: `Team Information - League: ${leagueName}`,
    onClose,
    renderBody,
    commandCancelTitle: 'Close',
    size: 'small'
  };
  return <Dialog {...propsDialog} />;
};

export default TeamInfo;
