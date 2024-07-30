import { useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';
import Dialog from '../../common/Dialog';
import RoundList from './RoundList';
import { useDispatch, useSelector } from 'react-redux';
import { queryDataWithParams, postData } from '../../../actions/action';
import MaxbetConst from '../../../constants/MaxbetConst';
import {
  QUERY_CONDITION,
  ACTION_TV,
  QUERY_LEAGUE_DETAIL
} from '../../../constants/action-type';
import { selectCondition } from '../../../selector/SelectorLeagueDetail';
import Loading from '../../../../components/Loading';
import DivisionList from './DivisionList';
import Tv from './Tv';
import { buildModelCondition } from '../../../helper/commonHelper';
import Service from '../../../Service';
import isEqual from 'lodash.isequal';
import ToastrWrapper from '../../../../components/ToastrWrapper';
import { useParams } from 'react-router-dom';
import { buildQuerySimpleApi } from '../../../../Shared/Constants';

const Condition = ({ leagueSettingId, onClose }) => {
  const { id } = useParams();
  const dispatch = useDispatch();
  const model = useSelector(selectCondition);
  const handleSave = () => {
    const postModel = buildModelCondition(model);
    (async () => {
      const response = await Service.updateCondition(postModel);
      if (isEqual(response.status, 2e2)) {
        await ToastrWrapper.showInfo(MaxbetConst.messageUpdateConditionSuccess);
        dispatch(
          postData(
            buildQuerySimpleApi(),
            {
              OWPLeagueId: parseInt(id, 10),
              apiKey: MaxbetConst.leagueManagementGetLeagueSettingAPIKey
            },
            QUERY_LEAGUE_DETAIL
          )
        );
      }
    })();
  };
  const renderBody = () => {
    return (
      <>
        <Tv hasTV={model.tv.hasTV} tv={model.tv.tv} actionType={ACTION_TV} />
        <RoundList rounds={model.round} />
        <DivisionList divisions={model.division} />
      </>
    );
  };
  const stableDispatch = useCallback(dispatch, [dispatch]);
  useEffect(() => {
    stableDispatch(
      queryDataWithParams(
        MaxbetConst.queryConditionUrl,
        {
          leagueSettingId
        },
        QUERY_CONDITION
      )
    );
  }, [stableDispatch, leagueSettingId]);

  if (model.conditionPending) {
    return <Loading />;
  }
  const props = {
    titleHeader: 'Condition',
    onClose,
    onSave: handleSave,
    renderBody,
    hasWrite: true
  };
  return <Dialog {...props} />;
};

Condition.propTypes = {
  leagueSettingId: PropTypes.number.isRequired,
  onClose: PropTypes.func.isRequired
};

export default Condition;
