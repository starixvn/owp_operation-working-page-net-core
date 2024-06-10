import Filter from './filterArea/Filter';
import { OWPPage, OWPHeader, OWPBody } from '../Shared/common/OWP/OPWPage';
import EventPickingTable from './table/EventPickingTable';
import usePermission from '../Shared/customHook/usePermission';
import { FunctionKeys } from '../Shared/Constants';
import Loading from '../components/Loading';
import ExportNative from './filterArea/ExportNative';
import UserGuideWrapper from './userGuide/UserGuideWrapper';
import SumProviderWrapper from './sumProvider/SumProviderWrapper';
import EditWrapper from './mode/EditWrapper';
import InfoIcon from '../../assets/svg-element/InfoIcon';

const EventPickingManagementNative = () => {
  const pagePolicy = usePermission(FunctionKeys.EventPickingNew, 'pagePolicy');
  const turnOverMarginPolicy = usePermission(
    FunctionKeys.EventPickingNew_EventAndPerformance,
    'turnOverMarginPolicy'
  );

  if (
    !turnOverMarginPolicy.permission.Username ||
    !pagePolicy.permission.Username
  ) {
    return <Loading />;
  }

  return (
    <div className='event-picking'>
      <OWPPage className='event-picking-page'>
        <OWPHeader className='tickets-tab'>
          <div className='filter-control'>
            <Filter />
            <ExportNative />
            <EditWrapper />
            <InfoIcon />
            <UserGuideWrapper />
            <SumProviderWrapper />
          </div>
        </OWPHeader>
        <OWPBody className='tickets-tab'>
          <EventPickingTable />
        </OWPBody>
      </OWPPage>
    </div>
  );
};

export default EventPickingManagementNative;
