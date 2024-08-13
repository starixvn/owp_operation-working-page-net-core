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
  const [activeTab, setActiveTab] = useState<TTab>(TAB_MY_TICKET);
  const pagePermissionModel = usePermission(
    FunctionKeys.TicketCenter,
    'pagePolicy'
  );

  const { permission } = pagePermissionModel;
  const { HasWrite } = permission;

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

  return (
    <>
      <div className='header'>
        <ul className='tab-my-ticket'>
          <li
            className={`${isEqual(TAB_MY_TICKET, activeTab) && 'active'}`}
            onClick={() => setActiveTab(TAB_MY_TICKET)}>
            My Tickets
          </li>
          {HasWrite && <li
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
