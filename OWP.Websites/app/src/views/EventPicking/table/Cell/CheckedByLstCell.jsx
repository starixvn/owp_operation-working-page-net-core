import PropTypes from 'prop-types';
import store from '../../app/store';
import { Input } from 'reactstrap';
import updateConfirmedThunk from '../updateConfirmedThunkNative';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const CheckedByLstCell = props => {
  const { rowIndex, dataSource } = props;
  const { isCheck, infoSiteMatchId, isChildMatch, isMovedEventDate } = dataSource[rowIndex];

  const { permission } = store.getState().pagePolicy;
  const dispatch = useStableDispatch();

  const handleSelectedChanged = async event => {
    const value = event.target.checked;
    if (value) {
      await dispatch(
        updateConfirmedThunk({
          confirmed: value,
          infoSiteMatchIds: [infoSiteMatchId]
        })
      );
    }
  };

  return (
    <div className='checked-cell'>
      <Input
        type='checkbox'
        className='check-by-lst'
        checked={isCheck}
        disabled={!permission.HasWrite || isMovedEventDate}
        hidden={isChildMatch}
        onChange={handleSelectedChanged}
      />
    </div>
  );
};

CheckedByLstCell.propTypes = propTypes;
export default CheckedByLstCell;
