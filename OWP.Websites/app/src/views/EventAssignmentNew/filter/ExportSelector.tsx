import DropdownNative from '../../Shared/common/Dropdown/DropdownNative';
import Spinner from '../../components/Spinner';
import ToastrWrapper from '../../components/ToastrWrapper';
import { useAppSelector } from '../app/hooks';
import store from '../app/store';
import { buildRequestURLForLGA } from '../helper/apiHelper';
import {
  eventPickingListFileName,
  eventPickingListUrlNew,
  eventPickingOldUrl,
  liveGameAssignmentFileName,
  liveGameAssignmentUrl
} from '../helper/exportExcelHelpper';
import Service from '../service/Service';

const ExportSelector = () => {
  const policy = useAppSelector(state => state.policy);
  const isHostEventPicking = policy.entities.EventPicking.HasWrite;
  const filterState = store.getState().filter.filterState;
  const { selectedMatchStatus, selectedOffVRs, selectedMatchType } =
    filterState;

  const exportExcel = async (url: string, fileName: string) => {
    (async () => {
      Spinner.show();
      const data = await Service.queryDataExcel(url);
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
      selectedMatchTypeIsValid &&
      selectedSyncOffVRIsValid
    ) {
      const url = buildRequestURLForLGA(liveGameAssignmentUrl(filterState));
      exportExcel(url, liveGameAssignmentFileName(filterState.selectedDate));
    } else {
      !selectedMatchStatusIsValid &&
        ToastrWrapper.showError('Please select at least one match status');
      !selectedSyncOffVRIsValid &&
        ToastrWrapper.showError('Please select at least one sync off VR');
      !selectedMatchTypeIsValid &&
        ToastrWrapper.showError('Please select at least one match type');
    }
  };

  const handleClickExportEventPickingList = () => {
    const url = buildRequestURLForLGA(eventPickingOldUrl(filterState));
    exportExcel(url, eventPickingListFileName(filterState.selectedDate));
  };

  const handleClickExportEventPickingListNew = () => {
    const url = eventPickingListUrlNew(filterState);
    exportExcel(url, 'Misc Match.xlsx');
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

export default ExportSelector;
