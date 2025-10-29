import PropTypes from 'prop-types';
import { Input } from 'reactstrap';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { useAppSelector } from '../../app/hooks';
import {
  setPreviousNoOfferRow,
  setListMatchSelected
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
    const matches = [];
    for (let i = startIndex; i <= endIndex; i++) {
      matches.push(dataSource[i]);
    }

    dispatch(setListMatchSelected({ matches, isAdded }));
    dispatch(setPreviousNoOfferRow(rowIndex));
  };

  const handleSelectedChanged = async event => {
    if (event.shiftKey && previousRowIndex !== null) {
      handleMultipleSelected(event?.target?.checked);
      return;
    }
    dispatch(
      setListMatchSelected({
        matches: [dataSource[rowIndex]],
        isAdded: event?.target?.checked
      })
    );
    dispatch(setPreviousNoOfferRow(rowIndex));
  };

  return (
    <div className='no-cell'>
      <span>{dataSource[rowIndex]?.matchNo}</span>
      {!dataSource[rowIndex]?.isChildMatch && (
        <Input
          type='checkbox'
          className='check-by-lst'
          defaultChecked={selected}
          onClick={handleSelectedChanged}
        />
      )}
    </div>
  );
};
NoCell.propTypes = propTypes;
export default NoCell;
