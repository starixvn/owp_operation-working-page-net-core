import dayjs from 'dayjs';
import { Suspense, useState, lazy } from 'react';
import Loading from '../../components/Loading';
import { FunctionKeys } from '../../Shared/Constants';
import usePermission from '../../Shared/customHook/usePermission';
import { getHeaderDateTime } from '../helper/tasklistHelper';
import CreateTicketBoardOther from '../../Shared/createTicketNew/CreateTicketBoardOther';
import CreateTicketLGAView from '../../Shared/createTicketNew/CreateTicketLGAView';
import CreateTicketLiveIncident from '../../Shared/createTicketNew/CreateTicketLiveIncident';
import TicketDetailBoard from '../../Shared/chatBoardNew/TicketDetailBoard';
import { CreatorInfo } from '../type/TicketType';
import { RECIPIENT_TYPE } from '../../Shared/createTicketNew/const/const';
import { buildGoToMyTicketLink } from '../../Shared/TicketCenterPrematch/helper/helper';
import PopupDetail from '../features/component/PopupDetail';
import { TTab } from '../type/TabType';
import { isEqual } from 'lodash';

const TabToday = lazy(() => import('../today/TabToday'));
const TabYesterDay = lazy(() => import('../yesterday/TabYesterday'));

const TAB_TODAY: TTab = 'TAB_TODAY';
const TAB_YESTERDAY: TTab = 'TAB_YESTERDAY';

const TabSwitcher = () => {
  const [activeTab, setActiveTab] = useState<TTab>(TAB_TODAY);
  const { today, yesterday } = getHeaderDateTime();
  const pagePermissionModel = usePermission(
    FunctionKeys.TicketCenter,
    'pagePolicy'
  );

  const { permission } = pagePermissionModel;
  const { Username, ActorId } = permission;

  const creatorInfo: CreatorInfo = {
    groupName: null,
    nickName: Username,
    recipientId: ActorId,
    recipientType: RECIPIENT_TYPE.INDIVIDUAL,
    userName: Username
  };

  const renderTabContent = () => {
    switch (activeTab) {
      case TAB_TODAY:
        return <TabToday eventDate={today} userId={creatorInfo.recipientId} />;
      case TAB_YESTERDAY:
        return (
          <TabYesterDay
            eventDate={yesterday}
            userId={creatorInfo.recipientId}
          />
        );
      default:
        return null;
    }
  };

  return (
    <>
      <div className='header'>
        <ul className='tab-task-list'>
          <li
            className={`${isEqual(TAB_TODAY, activeTab) && 'active'}`}
            onClick={() => setActiveTab(TAB_TODAY)}>
            Today ({dayjs(today).format('MMM DD')})
          </li>
          <li
            className={`${isEqual(TAB_YESTERDAY, activeTab) && 'active'}`}
            onClick={() => setActiveTab(TAB_YESTERDAY)}>
            Yesterday ({dayjs(yesterday).format('MMM DD')})
          </li>

          <a href={buildGoToMyTicketLink()} className='redirect-ticket'>
            Go to My Tickets
          </a>
        </ul>
      </div>
      <div className='content'>
        <Suspense fallback={<Loading />}>{renderTabContent()}</Suspense>

        <CreateTicketLGAView creatorInfo={creatorInfo} />
        <CreateTicketBoardOther creatorInfo={creatorInfo} />
        <CreateTicketLiveIncident creatorInfo={creatorInfo} />
        <PopupDetail />
      </div>
      <TicketDetailBoard />
    </>
  );
};

export default TabSwitcher;
