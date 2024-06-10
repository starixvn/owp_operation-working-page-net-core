import CreateTicketBoardOther from './../Shared/createTicketNew/CreateTicketBoardOther';
import CreateTicketButton from './../Shared/createTicketNew/CreateTicketButton';
import CreateTicketLiveIncident from './../Shared/createTicketNew/CreateTicketLiveIncident';
import CreateTicketLGAView from './../Shared/createTicketNew/CreateTicketLGAView';
import usePermissionLGL from './hook/usePermissionLGL';
import { FunctionKeys } from '../Shared/Constants';
import Loading from '../components/Loading';
import { useSelector } from 'react-redux';
import TicketDetail from '../Shared/chatBoardNew/TicketDetail';
import { useStableDispatch } from '../TicketCenter/ImportTicketCenterHelperNative';
import {
  changeErrorRecipient,
  closeCreatePopup,
  openPopupCreateTicketInLGA
} from '../Shared/createTicketNew/createTicketBoardSliceNew';
import {
  getModelByRowIndex,
  getTicketDetail,
  getTicketSubTypes
} from '../Shared/chatBoardNew/chatBoardThunkNative';
import { RECIPIENT_TYPE, DETAIL_URl, INCIDENT_TYPE } from './const.json';
import ToastrWrapper from '../components/ToastrWrapper';
import { closePopup } from '../Shared/chatBoardNew/chatBoardSlice';
import { onClickIssueModel } from './signalR/signalRSlice';
import { buildPermissionDetailLink } from './helper/commonHelper';

const PopupManagement = () => {
  const {
    permission: { Username, ActorId, HasWrite }
  } = usePermissionLGL(
    FunctionKeys.TicketCenter,
    buildPermissionDetailLink('api/application/authorize'),
    'policy'
  );

  const { isOpenPopup } = useSelector(state => state.chatBoard);
  const stableDispatch = useStableDispatch();
  const creatorInfo = {
    nickName: Username,
    recipientId: ActorId,
    userName: Username,
    groupName: null,
    recipientType: RECIPIENT_TYPE.INDIVIDUAL
  };

  const handleOpenCreateTicketByMatchId = async data => {
    stableDispatch(getModelByRowIndex(data));
    stableDispatch(closePopup());
    stableDispatch(closeCreatePopup());
    stableDispatch(openPopupCreateTicketInLGA());
  };

  const handleOpenMessage = async data => {
    const { ticketId, ticketType } = data;
    const requestDetailModel = {
      ticketId,
      actorId: ActorId,
      detailSegmentUrl: DETAIL_URl
    };
    stableDispatch(changeErrorRecipient(false));
    stableDispatch(onClickIssueModel(requestDetailModel));
    stableDispatch(closeCreatePopup());
    stableDispatch(getTicketSubTypes(ticketType));
    const actionResult = await stableDispatch(
      getTicketDetail({
        ...requestDetailModel,
        isQueryMessageInLastTwoDay: true
      })
    );
    if (getTicketDetail.rejected.match(actionResult)) {
      ToastrWrapper.showError(actionResult.payload);
    }
  };

  window.handleOpenMessage = handleOpenMessage;
  HasWrite &&
    (window.handleOpenCreateTicketByMatchId = handleOpenCreateTicketByMatchId);

  if (!ActorId) {
    return <Loading />;
  }

  return (
    <>
      {HasWrite && (
        <>
          <CreateTicketButton buttonType={INCIDENT_TYPE.NAME} />
          <CreateTicketBoardOther creatorInfo={creatorInfo} />
          <CreateTicketLiveIncident creatorInfo={creatorInfo} />
          <CreateTicketLGAView creatorInfo={creatorInfo} />
        </>
      )}
      {isOpenPopup && <TicketDetail policy='policy' />}
    </>
  );
};
export default PopupManagement;
