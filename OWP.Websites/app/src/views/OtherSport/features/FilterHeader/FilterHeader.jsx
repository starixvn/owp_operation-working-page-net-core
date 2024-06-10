import { useState, useRef } from 'react';
import DatePicker from 'react-datepicker';
import { MultiSelect } from 'react-multi-select-component';
import {
  buildTimeRangeLast30Day,
  getCurrentDateGmtPlus8
} from '../../../Shared/DateTime/dateTimeHelper';
import EmployeeFilterNative from './EmployeeFilterNative';
import {
  OFFER_LIVES,
  LINKED_MATCHES,
  MATCH_STATUSES,
  SPORTS,
  LOCAL_STORAGE_KEY
} from './FilterHeaderConst.json';
import { getDefaultValueSelected } from './FilterHeaderHelper';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import getOSAssignmentThunkNative from '../OSTable/OSTableSlice/getOSAssignmentThunkNative';
import PropTypes from 'prop-types';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { onSubmitFilterModel } from './FilterHeaderSlice';
import dayjs from 'dayjs';
import { getValueRef } from '../../../Shared/Utils';
import OtherSport from '../../otherSport.json';
import EditOfferLive from './EditOfferLive';
import { useSelector } from 'react-redux';
import {
  cancelEditOfferLive,
  setIsEdit,
  clearEditOfferLiveMatches
} from '../OSTable/OSTableSlice/reducer';
import AssignmentUpload from '../AssignmentUpload';
import { saveEditOfferLive } from '../../services/Service';
const propTypes = {
  policy: PropTypes.object.isRequired
};

const formatDate = 'YYYY-MM-DD';

const FilterHeader = props => {
  const model = useSelector(state => state.osAssignmentTable);
  const { editOfferLiveMatches, isEdit } = model;
  const stableDispatch = useStableDispatch();
  const { Username, HasWrite } = props.policy;
  const storageKeyByName = `${LOCAL_STORAGE_KEY}_${Username}`;
  const {
    defaultOfferLives,
    defaultMatchStatuses,
    defaultLinkedMatches,
    defaultSports
  } = getDefaultValueSelected(storageKeyByName);
  const defaultFromDate = getCurrentDateGmtPlus8();
  const defaultToDate = new Date(
    new Date().setDate(defaultFromDate.getDate() + 2)
  );

  const [selectedFromDate, setSelectedFromDate] = useState(defaultFromDate);
  const [selectedToDate, setSelectedToDate] = useState(defaultToDate);
  const [selectedSports, setSelectedSports] = useState(defaultSports);
  const [selectedOfferLives, setSelectedOfferLives] =
    useState(defaultOfferLives);
  const [selectedMatchStatuses, setSelectedMatchStatuses] =
    useState(defaultMatchStatuses);
  const [selectedLinkedMatches, setSelectedLinkedMatches] =
    useState(defaultLinkedMatches);

  const empRef = useRef();

  const handleOnSubmit = () => {
    const diffTime = Math.abs(selectedToDate - selectedFromDate);
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    if (diffDays >= 3) {
      ToastrWrapper.showError('Please select date range within 3 days');
    } else if (
      !selectedMatchStatuses.length ||
      !selectedSports.length ||
      !selectedOfferLives.length ||
      !selectedLinkedMatches.length
    ) {
      ToastrWrapper.showError(
        'Please select at least one option for sport, offer live, match status and linked match'
      );
    } else {
      const employee = getValueRef(empRef);
      const configString = JSON.stringify({
        employee,
        selectedOfferLives,
        selectedMatchStatuses
      });
      localStorage.setItem(`${storageKeyByName}`, configString);
      handleSubmitOnStart(employee);
    }
  };

  const buildFilterModel = empValue => {
    return {
      From: dayjs(selectedFromDate).format(formatDate),
      To: dayjs(selectedToDate).format(formatDate),
      Employee: empValue,
      Sports: selectedSports.map(x => x.value),
      OfferLives: selectedOfferLives.map(x => x.value),
      MatchStatuses: selectedMatchStatuses.map(x => x.value),
      LinkedMatches: selectedLinkedMatches.map(x => x.value)
    };
  };

  const handleSubmitOnStart = empValue => {
    const filterModel = buildFilterModel(empValue);
    stableDispatch(getOSAssignmentThunkNative(filterModel));
    stableDispatch(onSubmitFilterModel(filterModel));
  };

  const handleOnDownload = () => {
    const from = dayjs(selectedFromDate).format(formatDate);
    const to = dayjs(selectedToDate).format(formatDate);
    const employee = getValueRef(empRef);
    const sports = selectedSports.map(x => x.value).join(',');
    const offerLives = selectedOfferLives.map(x => x.value).join(',');
    const matchStatuses = selectedMatchStatuses.map(x => x.value).join(',');
    const linkedMatches = selectedLinkedMatches.map(x => x.value).join(',');
    if (
      !selectedMatchStatuses.length ||
      !selectedSports.length ||
      !selectedOfferLives.length ||
      !selectedLinkedMatches.length
    ) {
      ToastrWrapper.showError(
        'Please select at least one option for sport, offer live, match status and linked match'
      );
      return;
    }
    const url =
      process.env.NODE_ENV === 'development'
        ? `${OtherSport.rootUrl}${OtherSport.download}`
        : `${window.netCoreRootUrl}/api/OtherSport${OtherSport.download}`;
    window.location.href = `${url}?From=${from}&To=${to}&Employee=${employee}&Sports=${sports}
    &OfferLives=${offerLives}&MatchStatuses=${matchStatuses}&LinkedMatches=${linkedMatches}`;
  };

  const handleCancel = () => {
    stableDispatch(setIsEdit(false));
    stableDispatch(cancelEditOfferLive());
    stableDispatch(clearEditOfferLiveMatches());
  };
  const handleOnOpenEditOfferLive = () => {
    stableDispatch(setIsEdit(true));
  };

  const handleOnSave = async () => {
    const model = {
      fromDate: selectedFromDate,
      toDate: selectedToDate,
      offerLiveInfos: editOfferLiveMatches
    };

    const result = await saveEditOfferLive(model);
    if (result === 200) {
      handleOnSubmit();
      ToastrWrapper.show({
        position: 'top',
        showConfirmButton: false,
        timer: 2000,
        html: `<span class="os__upload--success">The assignment has been updated successfully!</span>`
      });
    }
    stableDispatch(clearEditOfferLiveMatches());
    stableDispatch(setIsEdit(false));
  };

  return (
    <>
      <div className='filter'>
        <div className='filter-control calendar'>
          <div className='label'>From</div>
          <DatePicker
            className='from-date'
            selected={selectedFromDate}
            onChange={date => setSelectedFromDate(date)}
            minDate={buildTimeRangeLast30Day(new Date())}
            dateFormat='MMM dd, yyyy'
          />
        </div>
        <div className='filter-control calendar'>
          <div className='label'>To</div>
          <DatePicker
            className='to-date'
            selected={selectedToDate}
            onChange={date => setSelectedToDate(date)}
            minDate={selectedFromDate}
            dateFormat='MMM dd, yyyy'
          />
        </div>
        <EmployeeFilterNative
          defaultValue={props.policy.Username}
          onLoadEmployeeDone={employee => handleSubmitOnStart(employee)}
          ref={empRef}
        />
        <div className='filter-control multiselect'>
          <div className='label'>Sport</div>
          <MultiSelect
            className='sport'
            options={SPORTS}
            value={selectedSports}
            disableSearch
            ClearSelectedIcon={null}
            onChange={setSelectedSports}
          />
        </div>
        <div className='filter-control multiselect'>
          <div className='label'>Offer Live</div>
          <MultiSelect
            className='offer-live'
            options={OFFER_LIVES}
            ClearSelectedIcon={null}
            value={selectedOfferLives}
            disableSearch
            onChange={setSelectedOfferLives}
          />
        </div>
        <div className='filter-control multiselect'>
          <div className='label'>Match Status</div>
          <MultiSelect
            className='match-status'
            options={MATCH_STATUSES}
            ClearSelectedIcon={null}
            value={selectedMatchStatuses}
            disableSearch
            onChange={setSelectedMatchStatuses}
          />
        </div>
        <div className='filter-control multiselect'>
          <div className='label'>Linked Match</div>
          <MultiSelect
            className='linked-match'
            options={LINKED_MATCHES}
            ClearSelectedIcon={null}
            disableSearch
            value={selectedLinkedMatches}
            onChange={setSelectedLinkedMatches}
          />
        </div>
        <div className='filter-control btn-block'>
          <button
            disabled={isEdit}
            className={`button-submit ${isEdit ? 'button-disable' : ''}`}
            onClick={handleOnSubmit}>
            Submit
          </button>
          <button
            disabled={isEdit}
            className={`button-submit ${isEdit ? 'button-disable' : ''}`}
            onClick={handleOnDownload}>
            Download
          </button>
        </div>
      </div>
      <div className='upload-wrapper-button'>
        {HasWrite && (
          <div className='edit-offer-live-wrapper'>
            <EditOfferLive
              editOfferLiveMatches={editOfferLiveMatches}
              handleOnSave={handleOnSave}
              handleCancel={handleCancel}
              handleOnOpenEditOfferLive={handleOnOpenEditOfferLive}
              isEdit={isEdit}
            />
          </div>
        )}
        {HasWrite && (
          <div className='btn-upload-assignment'>
            <AssignmentUpload />
          </div>
        )}
      </div>
    </>
  );
};

FilterHeader.propTypes = propTypes;
export default FilterHeader;
