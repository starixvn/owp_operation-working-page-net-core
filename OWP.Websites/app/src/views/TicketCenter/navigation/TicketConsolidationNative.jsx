import Subtype from '../features/consolidation/subtype/Subtype';
import ConsolidationView from '../features/consolidation/consolidationView/ConsolidationView';
import TicketDetailBoard from '../../Shared/chatBoardNew/TicketDetailBoard';

const TicketConsolidationNative = props => {
  return (
    <div className='ticket__consolidation'>
      <div className='subtype-tab'>
        <Subtype />
      </div>
      <div className='merge__ticket'>
        <ConsolidationView />
      </div>
      <div className='ticket__detail'>
        <div className='popup-ChatBoard'>
          <TicketDetailBoard />
        </div>
      </div>
    </div>
  );
};

TicketConsolidationNative.propTypes = {};

export default TicketConsolidationNative;
