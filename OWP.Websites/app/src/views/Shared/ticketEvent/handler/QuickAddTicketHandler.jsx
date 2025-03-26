import PropTypes from 'prop-types';
import { FlashAutoIcon } from 'mdi-react';
import { useSelector } from 'react-redux';
import { urlRequestLiveIncident } from '../../createTicketNew/const/const';
import Service from '../../createTicketNew/Service';
import { TicketTypeRecipientEnum } from '../../enum/TicketRecipientEnum';
import {
  formatRecipientModel,
  informResult
} from '../../createTicketNew/Helper/HelperCreateTicket';
import usePermission from '../../customHook/usePermission';
import { FunctionKeys } from '../../Constants';
import { TICKET_TYPE } from '../../TicketCenterPrematch/const/const.json';
import { post } from '../../Service/Service';
const { LIVE_INCIDENT } = TICKET_TYPE;

const propTypes = {
  closeAnotherPopup: PropTypes.func,
  data: PropTypes.array
};

const QuickAddTicketHandler = props => {
  const { closeAnotherPopup, data } = props;
  const { defaultRecipientsLGA } = useSelector(
    state => state.createTicketBoard
  );
  const hasWrite = useSelector(state => state.pagePolicy).permission.HasWrite;
  const pagePermissionModel = usePermission(
    FunctionKeys.TicketCenter,
    'pagePolicy'
  );
  const { permission } = pagePermissionModel;
  const { Username, ActorId } = permission;
  const creatorInfo = {
    groupName: null,
    nickName: Username,
    recipientId: ActorId,
    recipientType: 1,
    userName: Username
  };
  const handleQuickAddTicketInternal = async () => {
    closeAnotherPopup();
    const { OWPMatchId, League, Home, Away, EventDateTime } = data;
    const model = { owpMatchId: OWPMatchId };
    let recipientLGA = [];
    if (OWPMatchId) {
      recipientLGA = await Service.queryAssigneesByOwpMatchId(model);
    }

    const selected = defaultRecipientsLGA
      .filter(x => x.recipientType !== TicketTypeRecipientEnum.Assignee)
      .concat(recipientLGA);

    const reformattedArray = formatRecipientModel(selected, creatorInfo);
    const newAddModel = {
      OwpMatchId: OWPMatchId,
      OwpLeagueName: League,
      OwpHomeName: Home,
      OwpAwayName: Away,
      EventDateTimeGmt8: EventDateTime,
      CreatorId: creatorInfo.recipientId,
      Recipients: reformattedArray,
      Message: 'frank',
      TicketType: LIVE_INCIDENT,
      TicketTypeName: 'Live Incident',
      SubType: 0,
      SubTypeName: '',
      Files: [],
      url: urlRequestLiveIncident
    };

    const result = await post(newAddModel);
    informResult(result);
  };

  if (!hasWrite) {
    return null;
  }

  return (
    <span style={{ margin: '10px' }}>
      <FlashAutoIcon
        onClick={handleQuickAddTicketInternal}
        className='icon-plus'
      />
    </span>
  );
};

QuickAddTicketHandler.propTypes = propTypes;
export default QuickAddTicketHandler;
