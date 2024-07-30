import { useRef, useCallback } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
  getShiftByDateAndEmployee,
  clearEntities
} from '../league/shiftSlice/shiftSlice';
import { updateFilterDate } from '../filter/filterSlice';
import { updateStartTimeOriginal } from '../header/headerSlice';
import { URL_NIGHT_SHIFT } from '../../const/const';
import {
  getMMDDYYYYStringFromDateTime,
  getHourAndMinutes
} from '../../../Shared/DateTime/dateTimeHelper';
import Spinner from '../../../components/Spinner';
import { buildStartTimeQueryModel } from '../../helper/preMatchHelper';
import { backupFilterModel } from '../signalR/signalRSlice';
import { post } from '../../ImportPrematchHelperNative';
import { getDisableRef, getValueRef } from '../../../Shared/Utils';
import useAssignToMe from './useAssignToMe';
import FilterPrematchFactory from './FilterPrematchFactory';
import ToastrWrapper from '../../../components/ToastrWrapper';
import PropTypes from 'prop-types';

const propTypes = {
  isNightShift: PropTypes.bool
};
const ViewFilterNighShift = props => {
  const { isNightShift } = props;
  const employeeRef = useRef();
  const leagueRef = useRef();
  const multiSelectRef = useRef();

  const policy = useSelector(state => state.policy);
  const model = useSelector(state => state.filter);

  const dispatch = useDispatch();
  const stableDispatch = useCallback(dispatch, [dispatch]);

  const empKeyLocalStore = `${policy.permission.Username}-isFilterMe-night`;

  const [handleOnClick, isDisableEmp] = useAssignToMe(
    empKeyLocalStore,
    policy.permission.Username,
    employeeRef
  );

  const handleSubmit = async selectedDate => {
    const selectedDateFormatted = getMMDDYYYYStringFromDateTime(selectedDate);

    const startTimeQueryModel = buildStartTimeQueryModel(selectedDateFormatted);
    Spinner.show();
    const startTimeRespond = await post(startTimeQueryModel);
    Spinner.hide();
    const startTimeOriginal = startTimeRespond.data[0]
      ? startTimeRespond.data[0].StartTime
      : null;
    !startTimeOriginal && stableDispatch(clearEntities());
    stableDispatch(updateStartTimeOriginal(startTimeOriginal));

    const employeeValue = getValueRef(employeeRef);
    const leagueNameValue = getValueRef(leagueRef);
    const matchOffersValue = model.matchOffers;
    const earlyOfferedValue = model.earlyOffered;
    if (!matchOffersValue) {
      return;
    }

    if (!matchOffersValue?.length) {
      ToastrWrapper.showError('Please select at least one Match Offer');
      return;
    }

    if (!earlyOfferedValue?.length) {
      ToastrWrapper.showError(
        'Please select at least one Early Offered League'
      );
      return;
    }

    const filterModel = {
      matchOffers: matchOffersValue,
      earlyOffered: earlyOfferedValue,
      EventDate: getMMDDYYYYStringFromDateTime(selectedDate),
      Employee: employeeValue,
      LeagueName: leagueNameValue,
      ActorId: policy.permission.ActorId,
      url: URL_NIGHT_SHIFT,
      StartTime: getHourAndMinutes(startTimeOriginal) || ''
    };
    stableDispatch(updateFilterDate(selectedDate));

    if (startTimeOriginal) {
      stableDispatch(getShiftByDateAndEmployee(filterModel));
      stableDispatch(backupFilterModel(filterModel));
    }

    if (getDisableRef(employeeRef)) {
      localStorage.setItem(empKeyLocalStore, policy.permission.Username);
    } else {
      localStorage.removeItem(empKeyLocalStore);
    }
  };

  return (
    <FilterPrematchFactory
      isNightShift={isNightShift}
      model={model}
      policy={policy}
      ref={{
        employeeRef,
        leagueRef,
        multiSelectRef
      }}
      onSubmit={handleSubmit}
      isDisableEmp={isDisableEmp}
      onClickMyAssignee={handleOnClick}
    />
  );
};

ViewFilterNighShift.propTypes = propTypes;
export default ViewFilterNighShift;
