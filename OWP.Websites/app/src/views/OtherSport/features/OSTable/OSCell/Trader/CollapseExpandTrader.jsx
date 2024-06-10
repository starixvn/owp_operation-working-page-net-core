import { useCallback } from 'react';
import PropTypes from 'prop-types';
import CollapseExpandCell from '../../../../../Shared/common/CustomTable/commonCell/collapseExpandCell/CollapseExpandCell';
import {
  collapse,
  expand
} from '../../../../../Shared/common/CustomTable/commonCell/collapseExpandCell/collapseExpandConst.json';
import { useDispatch } from 'react-redux';
import { collapseRow, expandRow } from '../../OSTableSlice/reducer';

const propTypes = {
  collapseExpandRowState: PropTypes.object.isRequired,
  mode: PropTypes.string,
  rowIndex: PropTypes.number.isRequired,
  dataSource: PropTypes.array.isRequired
};

const CollapseExpandTrader = props => {
  const { collapseExpandRowState, dataSource, rowIndex } = props;
  const { osMatchId } = dataSource[rowIndex];
  const { columnKey, rowHasModeCollapse } = collapseExpandRowState;
  const dispatch = useDispatch();
  const stableDispatch = useCallback(dispatch, [dispatch]);

  const onCollapse = () => {
    const postModel = {
      columnKey,
      mode: collapse,
      osMatchId
    };
    stableDispatch(collapseRow(postModel));
  };

  const onExpand = () => {
    const postModel = {
      columnKey,
      mode: expand,
      osMatchId
    };
    stableDispatch(expandRow(postModel));
  };

  const mode = rowHasModeCollapse.some(matchId => matchId === osMatchId)
    ? collapse
    : expand;
  const collapseExpandProp = {
    headerName: '',
    mode,
    onCollapse,
    onExpand
  };
  return <CollapseExpandCell {...collapseExpandProp} />;
};

CollapseExpandTrader.propTypes = propTypes;

export default CollapseExpandTrader;
