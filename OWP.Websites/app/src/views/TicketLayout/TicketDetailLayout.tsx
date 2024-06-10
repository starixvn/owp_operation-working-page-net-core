import usePermission from '../Shared/customHook/usePermission';
import { FunctionKeys } from '../Shared/Constants';
import TicketDetailBoard from '../Shared/chatBoardNew/TicketDetailBoard';

const TicketDetailLayout = () => {
  usePermission(FunctionKeys.TicketCenter, 'pagePolicy');
  return (
    <div className='popup-ChatBoard'>
      <TicketDetailBoard />
    </div>
  );
};
export default TicketDetailLayout;
