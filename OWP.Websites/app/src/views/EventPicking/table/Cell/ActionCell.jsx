import { useState } from 'react';
import PropTypes from 'prop-types';
import LogHistoryIcon from '../../../../assets/svg-element/LogHistoryIcon';
import HistoryModal from '../../modal/HistoryModal.native';
import { getLocaleDateTime } from '../../../Shared/DateTime/dateTimeHelper';
import DeleteIcon from 'mdi-react/DeleteIcon';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { removeSpecialProduct } from '../../service/Service';
import Spinner from '../../../components/Spinner';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import store from '../../app/store';
import { getEventPickings } from '../eventPickingTableSlice/eventSliceThunkNative';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const ActionCell = props => {
  const { rowIndex, dataSource } = props;
  const {
    infoSiteMatchId,
    leagueName,
    homeName,
    awayName,
    eventDateTime,
    minorBettypeId,
    isChildMatch
  } = dataSource[rowIndex];
  const [openLog, setOpenLog] = useState(false);
  const dispatch = useAppDispatch();
  const { HasWrite } = useAppSelector(state => state.pagePolicy.permission);
  const handleRemoveSpecialProduct = async () => {
    const confirmResult = await ToastrWrapper.confirmYesNo(
      'Are you sure that you want to delete this event ?'
    );

    if (confirmResult.value) {
      Spinner.show();
      const model = {
        ParentId: infoSiteMatchId,
        MinorBettypeId: minorBettypeId
      };
      const response = await removeSpecialProduct(model);
      if (response.status === 200) {
        ToastrWrapper.showInfo('Remove event successfully!');
        const { param } = store.getState().filter;
        dispatch(getEventPickings(param));
      }
      Spinner.hide();
    }
  };

  const header = (
    <>
      <strong>
        Log History - {leagueName} | {homeName} vs {awayName}
      </strong>{' '}
      <i>{getLocaleDateTime(eventDateTime, 'lll')}</i>
    </>
  );

  return (
    <>
      <div className='action-cell'>
        {isChildMatch ? (
          HasWrite && (
            <span onClick={handleRemoveSpecialProduct}>
              <DeleteIcon className='icon-delete' size={20} />
            </span>
          )
        ) : (
          <span className='icon-history-event' onClick={() => setOpenLog(true)}>
            <LogHistoryIcon width={20} height={20} />
          </span>
        )}
      </div>
      {openLog && (
        <HistoryModal
          infoSiteMatchId={infoSiteMatchId}
          onToggle={() => setOpenLog(false)}
          header={header}
        />
      )}
    </>
  );
};

ActionCell.propTypes = propTypes;
export default ActionCell;
