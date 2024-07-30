import PreMatchTable from './features/league/PreMatchTable';
import ViewFilterDayShift from './features/filter/ViewFilterDayShift';
import EditComponent from './features/edit/EditComponent';
import WorkingShiftComponent from './features/workingShift/WorkingShiftComponent';
import { useSelector } from 'react-redux';
import { FunctionKeys } from '../Shared/Constants';
import HeaderDayShift from './features/header/HeaderDayShift';
import usePermission from '../Shared/customHook/usePermission';
import EditFilterDayShift from './features/filter/EditFilterDayShift';
import DayShiftSaver from './features/save/DayShiftSaver';
import Loading from '../components/Loading';
import TicketDetailBoard from '../Shared/chatBoardNew/TicketDetailBoard';
import SortDefault from './features/edit/SortDefault';
import { RECIPIENT_TYPE } from '../TicketCenter/const/const.json';
import CreateTicketButton from '../Shared/createTicketNew/CreateTicketButton';
import CreateTicketOther from '../Shared/createTicketNew/CreateTicketBoardOther';
import CreateTicketEventChecking from '../Shared/createTicketNew/CreateTicketEventChecking';
import CreateTicketPlusCircle from './features/traderChatBoard/CreateTicketPlusCircle';
import { BUTTON_TYPE } from '../Shared/createTicketNew/const/const';

const { PRE_MATCH } = BUTTON_TYPE;
const { INDIVIDUAL } = RECIPIENT_TYPE;
const DayShiftManagement = () => {
  const modelEditor = useSelector(state => state.editor);
  const modelPermission = usePermission(FunctionKeys.DayShiftAssignment);
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
    isNightShift: false
  };

  if (!permission.Username) {
    return <Loading />;
  }

  const renderFilterEditor = () => (
    <>
      <EditFilterDayShift />
      <div className='block-button '>
        <WorkingShiftComponent />
        <SortDefault />
        <DayShiftSaver />
      </div>
    </>
  );

  const renderFilterViewer = () => (
    <>
      <ViewFilterDayShift />
      {permission.HasWrite && <CreateTicketButton buttonType={PRE_MATCH} />}
      {permission.HasWrite && <EditComponent />}
    </>
  );

  const renderFilterArea = () =>
    modelEditor.isEdit ? renderFilterEditor() : renderFilterViewer();

  return (
    <>
      <HeaderDayShift isEdit={modelEditor.isEdit} />
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

export default DayShiftManagement;
