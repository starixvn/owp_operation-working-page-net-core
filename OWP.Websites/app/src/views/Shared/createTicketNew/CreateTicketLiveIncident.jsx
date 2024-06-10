import { useEffect } from 'react';
import CreateTicketBoardEvent from './CreateTicketBoardEvent';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import { urlRequestLiveIncident } from './const/const';
import { TICKET_TYPE } from '../TicketCenterPrematch/const/const.json';
import useStableDispatch from '../../Shared/customHook/useStableDispatch';
import { setDefaultRecipients } from '../../Shared/createTicketNew/createTicketBoardSliceNew';
import Loading from '../../components/Loading';

const { LIVE_INCIDENT } = TICKET_TYPE;
const propTypes = {
  creatorInfo: PropTypes.object.isRequired
};

const CreateTicketLiveIncident = props => {
  const stableDispatch = useStableDispatch();
  const { creatorInfo } = props;
  const { isLiveIncident, valueEvent, subTypeSelected, typeTicketText } =
    useSelector(state => state.createTicketBoard);
  const handleBuildModel = () => {
    return {
      OwpMatchId: valueEvent.owpMatchId,
      OwpLeagueName: valueEvent.leagueName,
      OwpHomeName: valueEvent.homeName,
      OwpAwayName: valueEvent.awayName,
      EventDateTimeGmt8: valueEvent.eventDateTimeGmt8,
      SubType: subTypeSelected.value,
      SubTypeName: subTypeSelected.label,
      TicketType: LIVE_INCIDENT,
      TicketTypeName: typeTicketText,
      url: urlRequestLiveIncident
    };
  };

  useEffect(() => {
    creatorInfo.userName && stableDispatch(setDefaultRecipients([creatorInfo]));
  }, [creatorInfo, stableDispatch]);

  if (!creatorInfo.userName) {
    return <Loading />;
  }
  return (
    isLiveIncident && (
      <CreateTicketBoardEvent
        creatorInfo={creatorInfo}
        handleBuildModel={handleBuildModel}
        ticketType={LIVE_INCIDENT}
      />
    )
  );
};

CreateTicketLiveIncident.propTypes = propTypes;
export default CreateTicketLiveIncident;
