import { useCallback } from 'react';
import PropTypes from 'prop-types';
import { useDispatch } from 'react-redux';
import CollapseExpandCell from '../commonCell/collapseExpandCell/CollapseExpandCell';
import {
  collapse,
  expand
} from '../commonCell/collapseExpandCell/collapseExpandConst.json';

const propTypes = {
  columnKey: PropTypes.string.isRequired,
  headerName: PropTypes.string.isRequired,
  collapseAllRowFunc: PropTypes.func.isRequired,
  expandAllRowFunc: PropTypes.func.isRequired,
  collapseExpandAllRowState: PropTypes.object.isRequired
};

const CollapseExpandHeader = headerModel => {
  const {
    columnKey,
    headerName,
    collapseAllRowFunc,
    expandAllRowFunc,
    collapseExpandAllRowState
  } = headerModel;
  const dispatch = useDispatch();
  const stableDispatch = useCallback(dispatch, [dispatch]);

  const onCollapse = () => {
    const postModel = {
      columnKey,
      mode: collapse
    };
    stableDispatch(collapseAllRowFunc(postModel));
  };

  const onExpand = () => {
    const postModel = {
      columnKey,
      mode: expand
    };
    stableDispatch(expandAllRowFunc(postModel));
  };

  const props = {
    headerName,
    mode: collapseExpandAllRowState.mode,
    onCollapse,
    onExpand
  };

  return <CollapseExpandCell {...props} />;
};

CollapseExpandHeader.propTypes = propTypes;
export default CollapseExpandHeader;
