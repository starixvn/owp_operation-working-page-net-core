import { useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';
import MaxbetConst from '../../../constants/MaxbetConst';
import { useDispatch, useSelector, useStore } from 'react-redux';
import { postData } from '../../../actions/action';
import { selectTVList } from '../../../selector/SelectorMaxbetGeneral';
import Loading from '../../../../components/Loading';
import ToastrWrapper from '../../../../components/ToastrWrapper';
import { buildModelTVList } from '../../../helper/commonHelper';
import Service from '../../../Service';
import { QUERY_TVLIST, ACTION_TVLIST } from '../../../constants/action-type';
import Dialog from '../../common/Dialog';
import RowsExpression from '../../common/RowsExpression';
import isEqual from 'lodash.isequal';
import { buildQuerySimpleApi } from '../../../../Shared/Constants';

const propTypes = {
  onClose: PropTypes.func.isRequired
};
const TVList = ({ onClose }) => {
  const dispatch = useDispatch();
  const store = useStore();
  const model = useSelector(selectTVList);
  const { policyData } = store.getState().policyReducer;
  const { HasWrite } = policyData.tvListPolicy;
  const handleSave = async () => {
    const state = store.getState();
    const postModel = buildModelTVList(state);
    const response = await Service.updateTVList(postModel);
    if (isEqual(response.status, 2e2)) {
      await ToastrWrapper.showInfo(MaxbetConst.messageUpdateTVListSuccess);
      onClose();
    }
  };

  const stableDispatch = useCallback(dispatch, [dispatch]);
  useEffect(() => {
    const postModel = {
      apiKey: MaxbetConst.maxbetGetTVListAPIKey
    };
    stableDispatch(postData(buildQuerySimpleApi(), postModel, QUERY_TVLIST));
  }, [stableDispatch]);
  const renderBody = () => {
    const expressionProps = {
      models: model.tvListExpression,
      titleName: ' TV List',
      hasWrite: HasWrite,
      modelsLength: model.tvListExpression.length,
      actionType: ACTION_TVLIST,
      modelExpressionDefault: {
        tv: {
          expression: MaxbetConst.expressionOption[0],
          value: ''
        }
      }
    };
    return <RowsExpression {...expressionProps} />;
  };
  if (model.tvListPending) {
    return <Loading />;
  }

  const props = {
    titleHeader: 'TV list',
    onClose,
    onSave: handleSave,
    renderBody,
    hasWrite: HasWrite
  };
  return <Dialog {...props} />;
};

TVList.propTypes = propTypes;
export default TVList;
