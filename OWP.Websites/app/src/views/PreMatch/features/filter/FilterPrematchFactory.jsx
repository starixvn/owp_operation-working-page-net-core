import { forwardRef } from 'react';
import PropTypes from 'prop-types';
import FilterPrematchViewer from './FilterPrematchViewer';
import Loading from '../../../components/Loading';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { isEqual } from 'lodash';

const propTypes = {
  model: PropTypes.object.isRequired,
  policy: PropTypes.object.isRequired
};

const FilterPrematchFactory = forwardRef((props, ref) => {
  const { model, policy } = props;

  if (
    isEqual(policy.loading, REQUEST_STATUS.pending) ||
    !policy.permission.ActorId
  ) {
    return <Loading />;
  }
  if (!model.filterDate || isEqual(model.loading, REQUEST_STATUS.pending)) {
    return <Loading />;
  }

  return <FilterPrematchViewer ref={{ ...ref }} {...props} />;
});

FilterPrematchFactory.propTypes = propTypes;
FilterPrematchFactory.displayName = 'FilterPrematchFactory';
export default FilterPrematchFactory;
