import { useCallback, useRef } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { getShiftByDateAndEmployee } from '../league/shiftSlice/shiftSlice';
import { updateFilterEmployer, updateFilterDate } from '../filter/filterSlice';
import { URL_DAY_SHIFT } from '../../const/const';
import { getMMDDYYYYStringFromDateTime } from '../../../Shared/DateTime/dateTimeHelper';
import { backupFilterModel } from '../signalR/signalRSlice';
import { getDisableRef, getValueRef } from '../../../Shared/Utils';
import useAssignToMe from './useAssignToMe';
import FilterPrematchFactory from './FilterPrematchFactory';
import ToastrWrapper from '../../../components/ToastrWrapper';

const ViewFilterDayShift = () => {
  const employeeRef = useRef();
  const leagueRef = useRef();

  const policy = useSelector(state => state.policy);
  const model = useSelector(state => state.filter);

  const dispatch = useDispatch();
  const stableDispatch = useCallback(dispatch, [dispatch]);

  const empKeyLocalStore = `${policy.permission.Username}-isFilterMe-day`;

  const [handleOnClick, isDisableEmp] = useAssignToMe(
    empKeyLocalStore,
    policy.permission.Username,
    employeeRef
  );

  const handleSubmit = async selectedDate => {
    const employeeValue = getValueRef(employeeRef);
    const leagueNameValue = getValueRef(leagueRef);
    const matchOffersValue = model.matchOffers;
    const earlyOfferedValue = model.earlyOffered;
    const eventDate = getMMDDYYYYStringFromDateTime(selectedDate);

    if (!matchOffersValue.length) {
      ToastrWrapper.showError('Please select at least one Match Offer');
      return;
    }

    if (!earlyOfferedValue?.length) {
      ToastrWrapper.showError(
        'Please select at least one Early Offered League'
      );
      return;
    }
    const filter = {
      matchOffers: matchOffersValue,
      earlyOffered: earlyOfferedValue,
      EventDate: eventDate,
      Employee: employeeValue,
      LeagueName: leagueNameValue,
      ActorId: policy.permission.ActorId,
      url: URL_DAY_SHIFT
    };

    stableDispatch(updateFilterDate(selectedDate));
    stableDispatch(updateFilterEmployer({ employer: employeeValue }));

    stableDispatch(getShiftByDateAndEmployee(filter));
    stableDispatch(backupFilterModel(filter));

    if (getDisableRef(employeeRef)) {
      localStorage.setItem(empKeyLocalStore, policy.permission.Username);
    } else {
      localStorage.removeItem(empKeyLocalStore);
    }
  };

  return (
    <FilterPrematchFactory
      model={model}
      policy={policy}
      ref={{
        employeeRef,
        leagueRef
      }}
      onSubmit={handleSubmit}
      isDisableEmp={isDisableEmp}
      onClickMyAssignee={handleOnClick}
    />
  );
};

export default ViewFilterDayShift;
