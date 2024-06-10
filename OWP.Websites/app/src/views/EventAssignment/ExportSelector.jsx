import PropTypes from 'prop-types';
import Utils from '../Shared/Utils';
import EventAssignmentService from './Service';
import ToastrWrapper from '../components/ToastrWrapper';
import {
  exportExcelTypes,
  eventPickingListFileName,
  liveGameAssignmentFileName,
  eventPickingListUrl,
  liveGameAssignmentUrl,
  eventPickingListUrlNew
} from './constants/ExportExcel';
import Spinner from '../components/Spinner';
import DropdownNative from '../Shared/common/Dropdown/DropdownNative';

const propTypes = {
  userNameIsValid: PropTypes.bool.isRequired,
  userName: PropTypes.string.isRequired,
  selectedDate: PropTypes.object.isRequired,
  selectedMatchStatus: PropTypes.array.isRequired,
  selectedOffVRs: PropTypes.array.isRequired,
  selectedMatchType: PropTypes.string.isRequired,
  isHostEventPicking: PropTypes.bool.isRequired,
  lastUpdated: PropTypes.string.isRequired,
  selectedSyncLive: PropTypes.number.isRequired
};

const { EventPickingList, LiveGameAssignment, EventPickingListNew } =
  exportExcelTypes;
const exportInfo = {
  [EventPickingList]: (model, selectedDate) => {
    const [url, fileName] = [
      eventPickingListUrl(model),
      eventPickingListFileName(selectedDate)
    ];
    return { url, fileName };
  },
  [LiveGameAssignment]: (model, selectedDate) => {
    const [url, fileName] = [
      liveGameAssignmentUrl(model),
      liveGameAssignmentFileName(selectedDate)
    ];
    return { url, fileName };
  },
  [EventPickingListNew]: model => {
    const [url, fileName] = [eventPickingListUrlNew(model), 'Misc Match.xlsx'];
    return { url, fileName };
  }
};

const ExportSelector = props => {
  const {
    isHostEventPicking,
    selectedDate,
    selectedMatchStatus,
    selectedOffVRs,
    selectedMatchType,
    userName,
    userNameIsValid,
    lastUpdated,
    selectedSyncLive
  } = props;

  const createUrlFileName = exportType => {
    const model = {
      EventDate: Utils.formatDate(selectedDate),
      Username: userName || '',
      MatchStatusIds: selectedMatchStatus || [],
      OffVRIds: selectedOffVRs || [],
      MatchType: selectedMatchType || [],
      SelectedMatchType: selectedMatchType || [],
      SyncLiveStatus: selectedSyncLive || 0,
      LastUpdated: lastUpdated
    };

    return exportInfo[exportType](model, selectedDate);
  };

  const exportExcel = (url, fileName) => {
    (async () => {
      Spinner.show();
      const data = await EventAssignmentService.queryDataExcel(url);
      if (data) {
        const urlFake = window.URL.createObjectURL(new Blob([data]));
        const link = document.createElement('a');
        link.href = urlFake;
        link.setAttribute('download', fileName);
        document.body.appendChild(link);
        link.click();
      }
      Spinner.hide();
    })();
  };

  const handleClickExportLiveGameAssignment = () => {
    const selectedMatchStatusIsValid = selectedMatchStatus.length > 0;
    const selectedSyncOffVRIsValid = selectedOffVRs.length > 0;
    const selectedMatchTypeIsValid = selectedMatchType.length > 0;
    if (
      selectedMatchStatusIsValid &&
      userNameIsValid &&
      selectedMatchTypeIsValid
    ) {
      const { url, fileName } = createUrlFileName(LiveGameAssignment);
      exportExcel(url, fileName);
    } else {
      !selectedMatchStatusIsValid &&
        ToastrWrapper.showError('Please select at least one match status');
      !selectedSyncOffVRIsValid &&
        ToastrWrapper.showError('Please select at least one sync off VR');
      !userNameIsValid && ToastrWrapper.showError('User not found');
      !selectedMatchTypeIsValid &&
        ToastrWrapper.showError('Please select at least one match type');
    }
  };

  const handleClickExportEventPickingList = () => {
    const { url, fileName } = createUrlFileName(EventPickingList);
    exportExcel(url, fileName);
  };

  const handleClickExportEventPickingListNew = () => {
    const { url, fileName } = createUrlFileName(EventPickingListNew);
    exportExcel(url, fileName);
  };

  if (isHostEventPicking) {
    const items = [
      {
        title: 'Live Game Assignment',
        handler: handleClickExportLiveGameAssignment
      },
      {
        title: 'Event Picking List',
        handler: handleClickExportEventPickingList
      },
      {
        title: 'Misc Match',
        handler: handleClickExportEventPickingListNew
      }
    ];
    return <DropdownNative items={items} />;
  }

  return (
    <button
      className='button-export'
      onClick={handleClickExportLiveGameAssignment}>
      Export
    </button>
  );
};

ExportSelector.propTypes = propTypes;
export default ExportSelector;
