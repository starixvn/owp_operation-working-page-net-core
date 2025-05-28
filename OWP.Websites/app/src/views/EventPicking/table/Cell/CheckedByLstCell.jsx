import PropTypes from 'prop-types';
import store from '../../app/store';
import { Input } from 'reactstrap';
import updateConfirmedThunk from '../updateConfirmedThunkNative';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { convertToGMT8String } from '../../helper/confirmedHelper';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const CheckedByLstCell = props => {
  const { rowIndex, dataSource } = props;
  const { isCheck, infoSiteMatchId, isChildMatch, isMovedEventDate } =
    dataSource[rowIndex];

  const { permission } = store.getState().pagePolicy;
  const dispatch = useStableDispatch();

  const handleSelectedChanged = async event => {
    const value = event.target.checked;
    const { lastSearchTime } = store.getState().filter;
    const gmt8TimeStr = convertToGMT8String(lastSearchTime);

    if (value) {
      await dispatch(
        updateConfirmedThunk({
          confirmed: value,
          infoSiteMatchIds: [infoSiteMatchId],
          confirmedAt: gmt8TimeStr
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
