import PropTypes from 'prop-types';
import { Input } from 'reactstrap';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { buildNoOfferMainMatchInfo } from '../../helper/noOfferSingleMatchPipeline';
import { useAppSelector } from '../../app/hooks';
import {
  setPreviousNoOfferRow,
  setMatchSelected
} from '../eventPickingTableSlice/reducer';

const propTypes = {
  rowIndex: PropTypes.number.isRequired,
  dataSource: PropTypes.array.isRequired
};

const NoCell = props => {
  const { rowIndex, dataSource } = props;
  const dispatch = useStableDispatch();
  const previousRowIndex = useAppSelector(
    state => state.eventPickingTable.previousNoOfferIndex
  );
  const matchSelected = useAppSelector(
    state => state.eventPickingTable.matchSelected
  );

  const selected =
    matchSelected.findIndex(
      x => x.infoSiteMatchId === dataSource[rowIndex].infoSiteMatchId
    ) >= 0;

  const handleMultipleSelected = async isAdded => {
    const startIndex = Math.min(previousRowIndex, rowIndex);
    const endIndex = Math.max(previousRowIndex, rowIndex);

    for (let i = startIndex; i <= endIndex; i++) {
      dispatch(
        setMatchSelected({
          match: dataSource[i],
          isAdded
        })
      );
    }
    dispatch(setPreviousNoOfferRow(rowIndex));
  };

  const handleSelectedChanged = async event => {
    if (event.shiftKey && previousRowIndex !== null) {
      handleMultipleSelected(event?.target?.checked);
      return;
    }
    dispatch(
      setMatchSelected({
        match: dataSource[rowIndex],
        isAdded: event?.target?.checked
      })
    );
    dispatch(setPreviousNoOfferRow(rowIndex));
  };

  return (
    <div className='no-cell' onClick={handleSelectedChanged}>
      <span>{rowIndex + 1}</span>
      <Input
        type='checkbox'
        className='check-by-lst'
        defaultChecked={selected}
      />
    </div>
  );
};
NoCell.propTypes = propTypes;
export default NoCell;
