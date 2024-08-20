import { isEqual } from 'lodash';
import { lazy, Suspense, useState } from 'react';
import Loading from '../../components/Loading';
import { FunctionKeys } from '../../Shared/Constants';
import usePermission from '../../Shared/customHook/usePermission';

const MyTicketTab = lazy(() => import('./MyTicketTabNative'));
const TicketConsolidationTab = lazy(() => import('./TicketConsolidationNative'));
type TTab = 'TAB_MY_TICKET' | 'TAB_CONSOLIDATE';
const TAB_MY_TICKET: TTab = 'TAB_MY_TICKET';
const TAB_CONSOLIDATE: TTab = 'TAB_CONSOLIDATE';

const TabSwitcher = () => {
  const [activeTab, setActiveTab] = useState<TTab>(TAB_CONSOLIDATE);
  const pagePolicy = usePermission(
    FunctionKeys.TicketCenter,
    'pagePolicy'
  );

  const consolidationPolicy = usePermission(
    FunctionKeys.TicketConsolidation,
    'consolidationPolicy'
  );

  const renderTabContent = () => {
    switch (activeTab) {
      case TAB_MY_TICKET:
        return <MyTicketTab />;
      case TAB_CONSOLIDATE:
        return <TicketConsolidationTab />;
      default:
        return null;
    }
  };

  if (!pagePolicy.permission.Username || !consolidationPolicy.permission.Username) {
    return <Loading />;
  }

  return (
    <>
      <div className='header'>
        <ul className='tab-my-ticket'>
          <li
            className={`${isEqual(TAB_MY_TICKET, activeTab) && 'active'}`}
            onClick={() => setActiveTab(TAB_MY_TICKET)}>
            My Tickets
          </li>
          {consolidationPolicy.permission.HasRead && <li
            className={`${isEqual(TAB_CONSOLIDATE, activeTab) && 'active'}`}
            onClick={() => setActiveTab(TAB_CONSOLIDATE)}>
            Ticket Consolidation
          </li>}
        </ul>
      </div>
      <div>
        <Suspense fallback={<Loading />}>{renderTabContent()}</Suspense>
      </div>
    </>
  );
};

export default TabSwitcher;
