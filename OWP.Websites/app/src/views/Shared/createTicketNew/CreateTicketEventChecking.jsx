import { useEffect } from 'react';
import CreateTicketBoardEvent from './CreateTicketBoardEvent';
import { urlRequestEvent, urlRequestLeague } from './const/const';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import { TICKET_TYPE } from '../TicketCenterPrematch/const/const.json';
import useStableDispatch from '../../Shared/customHook/useStableDispatch';
import { setDefaultRecipients } from '../../Shared/createTicketNew/createTicketBoardSliceNew';
import Loading from '../../components/Loading';

const { EVENT_CHECKING } = TICKET_TYPE;
const propTypes = {
  creatorInfo: PropTypes.object.isRequired
};

const CreateTicketEventChecking = props => {
  const stableDispatch = useStableDispatch();
  const { creatorInfo } = props;
  const { isEventChecking, valueEvent, subTypeSelected } = useSelector(
    state => state.createTicketBoard
  );

  useEffect(() => {
    creatorInfo.userName && stableDispatch(setDefaultRecipients([creatorInfo]));
  }, [creatorInfo, stableDispatch]);

  if (!creatorInfo.userName) {
    return <Loading />;
  }
  const handleBuildModel = () => {
    return {
      InfoSiteMatchId: valueEvent.infoSiteMatchId,
      OwpAwayId: valueEvent.awayId,
      OwpHomeId: valueEvent.homeId,
      OwpLeagueId: valueEvent.owpLeagueId,
      OwpHomeName: valueEvent.homeName,
      OwpAwayName: valueEvent.awayName,
      OwpLeagueName: valueEvent.leagueName,
      EventDateTimeGmt8: valueEvent.eventDateTimeGmt8,
      SubType: subTypeSelected.value,
      SubTypeName: subTypeSelected.label,
      TicketType: EVENT_CHECKING,
      TicketTypeName: 'Event Checking',
      url: valueEvent.isLeague ? urlRequestLeague : urlRequestEvent
    };
  };

  return (
    isEventChecking && (
      <CreateTicketBoardEvent
        creatorInfo={creatorInfo}
        handleBuildModel={handleBuildModel}
        ticketType={EVENT_CHECKING}
      />
    )
  );
};

CreateTicketEventChecking.propTypes = propTypes;
export default CreateTicketEventChecking;
