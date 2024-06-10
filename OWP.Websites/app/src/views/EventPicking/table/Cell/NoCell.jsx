import PropTypes from 'prop-types';
import store from '../../app/store';
import { Input } from 'reactstrap';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import updateNoOfferMatchThunk from '../updateNoOfferMatchThunkNative';
import {
  buildNoOfferChildMatchInfo,
  buildNoOfferMainMatchInfo
} from '../../helper/noOfferSingleMatchPipeline';
import { useAppSelector } from '../../app/hooks';
import { setPreviousNoOfferRow } from '../eventPickingTableSlice/reducer';

const propTypes = {
  rowIndex: PropTypes.number.isRequired,
  dataSource: PropTypes.array.isRequired
};

const NoCell = props => {
  const { permission } = store.getState().pagePolicy;

  const previousRowIndex = useAppSelector(
    state => state.eventPickingTable.previousNoOfferIndex
  );
  const { rowIndex, dataSource } = props;
  const { isNoOffer, infoSiteMatchId, isChildMatch, childMatchId } =
    dataSource[rowIndex];

  const dispatch = useStableDispatch();

  const handleMultipleSelected = async (matchNoOffer, value) => {
    const startIndex = Math.min(previousRowIndex, rowIndex);
    const endIndex = Math.max(previousRowIndex, rowIndex);
    for (let i = startIndex; i <= endIndex; i++) {
      const { infoSiteMatchId, childMatchId } = dataSource[i];
      const source = {
        infoSiteMatchId,
        isNoOffer: value,
        childMatchId
      };
      buildNoOfferMainMatchInfo({
        dataSource,
        source,
        matchNoOffer
      });
    }
    dispatch(setPreviousNoOfferRow(rowIndex));
    await dispatch(updateNoOfferMatchThunk(matchNoOffer));
  };

  const handleSelectedChanged = async event => {
    const value = event.target.checked;
    const matchNoOffer = [];
    if (event.shiftKey && previousRowIndex !== null) {
      handleMultipleSelected(matchNoOffer, value);
      return;
    }
    const source = {
      infoSiteMatchId,
      isNoOffer: value,
      childMatchId
    };

    isChildMatch &&
      buildNoOfferChildMatchInfo({
        matchNoOffer,
        source
      });

    !isChildMatch &&
      buildNoOfferMainMatchInfo({
        dataSource,
        source,
        matchNoOffer
      });

    dispatch(setPreviousNoOfferRow(rowIndex));
    await dispatch(updateNoOfferMatchThunk(matchNoOffer));
  };

  return (
    <div className='no-cell'>
      <span>{rowIndex + 1}</span>
      <Input
        type='checkbox'
        defaultChecked={isNoOffer}
        disabled={!permission.HasWrite}
        onClick={handleSelectedChanged}
      />
    </div>
  );
};
NoCell.propTypes = propTypes;
export default NoCell;
