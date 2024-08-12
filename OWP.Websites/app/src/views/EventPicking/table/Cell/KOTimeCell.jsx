import PropTypes from 'prop-types';
import { formatTime } from '../../../Shared/DateTime/dateTimeHelper';
import Switch from '@mui/joy/Switch';
import Typography from '@mui/joy/Typography';
import updateNoOfferMatchThunk from '../updateNoOfferMatchThunkNative';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import {
  buildNoOfferChildMatchInfo,
  buildNoOfferMainMatchInfo
} from '../../helper/noOfferSingleMatchPipeline';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired,
  permission: PropTypes.object.isRequired
};

const KoTimeCell = props => {
  const { rowIndex, dataSource, permission } = props;
  const {
    eventDateTime,
    isNoOffer,
    infoSiteMatchId,
    isChildMatch,
    childMatchId
  } = dataSource[rowIndex];
  const dispatch = useStableDispatch();

  const handleSelectedChanged = async event => {
    const value = !event.target.checked;
    const matchNoOffer = [];
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
    await dispatch(updateNoOfferMatchThunk(matchNoOffer));
  };

  return (
    <div className='ko-time-cell'>
      <span>{formatTime(eventDateTime, 'minute', 'hh:mm A')}</span>
      <br></br>
      {!isChildMatch && (
        <Switch
          className='switch-match-offer'
          slotProps={{
            track: {
              children: (
                <Typography
                  component='span'
                  level='inherit'
                  className={isNoOffer ? 'no-offer-text' : 'offer-text'}>
                  {isNoOffer ? 'No Offer' : 'Offer'}
                </Typography>
              )
            }
          }}
          disabled={!permission.HasWrite}
          checked={!isNoOffer}
          onChange={handleSelectedChanged}
          sx={{
            '--Switch-thumbSize': '20px',
            '--Switch-trackWidth': '58px',
            '--Switch-trackHeight': '23px'
          }}
        />
      )}
    </div>
  );
};

KoTimeCell.propTypes = propTypes;
export default KoTimeCell;
