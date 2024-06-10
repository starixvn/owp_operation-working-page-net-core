import { useParams } from 'react-router-dom';
import Header from './Header';
import NewCategoryButton from './NewCategoryButton';
import TableLeagueDetail from './TableLeagueDetail';
import { selectDisplayAddLeagueButton } from '../../../selector/SelectorDisplayAddnewLeagueButton';
import { useSelector, useStore } from 'react-redux';
import PropTypes from 'prop-types';
import { hasWriteOnPageMaxbetGeneral } from '../../../helper/commonHelper';

const MaxbetDetail = props => {
  const { id } = useParams();
  const store = useStore();
  const { policyData } = store.getState().policyReducer;
  const hasWrite = hasWriteOnPageMaxbetGeneral(policyData);
  const { canAddLeague } = useSelector(selectDisplayAddLeagueButton);

  return (
    <div className='maxbet-content-detail'>
      {canAddLeague && hasWrite && (
        <div className='filter'>
          <NewCategoryButton />
        </div>
      )}
      <div className='tbl-maxbet-wrapper'>
        <Header {...props} />
        <TableLeagueDetail leagueId={id} />
      </div>
    </div>
  );
};

MaxbetDetail.propTypes = {
  props: PropTypes.object
};

export default MaxbetDetail;
