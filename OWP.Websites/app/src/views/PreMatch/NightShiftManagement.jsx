import PreMatchTable from './features/league/PreMatchTable';
import EditComponent from './features/edit/EditComponent';
import WorkingShiftComponent from './features/workingShift/WorkingShiftComponent';
import { useSelector } from 'react-redux';
import { FunctionKeys } from '../Shared/Constants';
import HeaderNightShift from './features/header/HeaderNightShift';
import usePermission from '../Shared/customHook/usePermission';
import EditFilterNightShift from './features/filter/EditFilterNightShift';
import ViewFilterNighShift from './features/filter/ViewFilterNighShift';
import NightShiftSaver from './features/save/NightShiftSaver';
import Loading from '../components/Loading';
import SortDefault from './features/edit/SortDefault';
import TicketDetailBoard from '../Shared/chatBoardNew/TicketDetailBoard';
import { RECIPIENT_TYPE } from '../TicketCenter/const/const.json';
import CreateTicketButton from '../Shared/createTicketNew/CreateTicketButton';
import CreateTicketOther from '../Shared/createTicketNew/CreateTicketBoardOther';
import CreateTicketEventChecking from '../Shared/createTicketNew/CreateTicketEventChecking';
import CreateTicketPlusCircle from './features/traderChatBoard/CreateTicketPlusCircle';
import { BUTTON_TYPE } from '../Shared/createTicketNew/const/const';

const { PRE_MATCH } = BUTTON_TYPE;
const { INDIVIDUAL } = RECIPIENT_TYPE;
const NightShiftManagement = () => {
  const modelEditor = useSelector(state => state.editor);
  const modelPermission = usePermission(FunctionKeys.NightShiftAssignment);
  const { permission } = modelPermission;
  const creatorInfo = {
    groupName: null,
    nickName: permission.Username,
    recipientId: permission.ActorId,
    recipientType: INDIVIDUAL,
    userName: permission.Username
  };
  const props = {
    sortState: {
      orderType: 'ORDER_ASC',
      columnKey: 'LeagueName'
    },
    isEditing: modelEditor.isEdit,
    isNightShift: true
  };

  const renderFilterEditor = () => (
    <>
      <EditFilterNightShift />
      <div className='block-button '>
        <WorkingShiftComponent />
        <SortDefault />
        <NightShiftSaver />
      </div>
    </>
  );

  const renderFilterViewer = () => (
    <>
      <ViewFilterNighShift isNightShift={props.isNightShift} />
      {permission.HasWrite && <CreateTicketButton buttonType={PRE_MATCH} />}
      {permission.HasWrite && <EditComponent />}
    </>
  );

  const renderFilterArea = () =>
    modelEditor.isEdit ? renderFilterEditor() : renderFilterViewer();

  if (!permission.Username) {
    return <Loading />;
  }

  return (
    <>
      <HeaderNightShift isEdit={modelEditor.isEdit} isNightShift />
      <div className='block-filter'>{renderFilterArea()}</div>
      <div className='table-PreMatch'>
        <PreMatchTable {...props} />
      </div>
      <CreateTicketPlusCircle creatorInfo={creatorInfo} />
      <TicketDetailBoard />
      <CreateTicketOther creatorInfo={creatorInfo} />
      <CreateTicketEventChecking creatorInfo={creatorInfo} />
    </>
  );
};

export default NightShiftManagement;
