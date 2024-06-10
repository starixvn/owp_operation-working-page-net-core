import { useSelector } from 'react-redux';
import ModalCommon from '../share/ModalCommon';
import PropTypes from 'prop-types';
import { getLocaleDateTime } from '../../../Shared/DateTime/dateTimeHelper';
import useDeletedLeague from '../customHook/useDeletedLeague';
import { isEqual } from 'lodash';
import Loading from '../../../components/Loading';
import { REQUEST_STATUS } from '../../../Shared/Constants';

const propTypes = {
  children: PropTypes.node.isRequired,
  isDay: PropTypes.bool
};

const ViewDeletedLeagueCommon = props => {
  const filterDate = useSelector(state => state.filter.filterDate);
  const timeShiftFormatter = getLocaleDateTime(new Date(filterDate), 'LL');
  const model = useDeletedLeague(props.isDay);
  if (isEqual(model.loading, REQUEST_STATUS.pending)) {
    return <Loading />;
  }
  return (
    <ModalCommon
      className='deleted-league'
      title='View Deleted Leagues'
      size='xl'>
      <h2>Deleted Leagues - {timeShiftFormatter}</h2>
      {props.children}
    </ModalCommon>
  );
};

ViewDeletedLeagueCommon.propTypes = propTypes;
export default ViewDeletedLeagueCommon;
