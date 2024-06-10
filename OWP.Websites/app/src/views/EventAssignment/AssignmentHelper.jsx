import { findIndex, filter, find, remove, isEqual } from 'lodash';
import { MinorBetTypes } from '../Shared/Constants';
import { buildWritePermission } from './PolicyHelper';
import { AssignmentBettype } from './constants/Column';
import { v4 as uuidv4 } from 'uuid';

const checkAllowAddAssignmentSpecific15Mins = (
  minorBettypeId,
  bettypeName,
  assignments
) => {
  if (
    isEqual(bettypeName, 'Specific15Mins') &&
    minorBettypeId !== null &&
    !(
      minorBettypeId === MinorBetTypes.NoOfCorners ||
      minorBettypeId === MinorBetTypes.TotalBooking
    )
  ) {
    return assignments && assignments.length > 0;
  }

  return true;
};

const checkAllowAddAssignmentSpecific10Mins = (
  minorBettypeId,
  bettypeName,
  assignments
) => {
  if (isEqual(bettypeName, 'Specific10Mins') && minorBettypeId !== null) {
    return assignments && assignments.length > 0;
  }

  return true;
};

const checkAllowAddAssignmentFM = (
  minorBettypeId,
  bettypeName,
  assignments
) => {
  const isFMOrAIO = isEqual(bettypeName, 'FM') || isEqual(bettypeName, 'AIO');
  const isMinorBettypeValid =
    minorBettypeId !== null &&
    ![MinorBetTypes.NoOfCorners, MinorBetTypes.TotalBooking].includes(
      minorBettypeId
    );

  if (isFMOrAIO && isMinorBettypeValid) {
    return assignments && assignments.length > 0;
  }
  return true;
};

const checkAllowAddAssignmentFullBetTypes = (
  minorBettypeId,
  bettypeName,
  assignments
) => {
  return (
    checkAllowAddAssignmentSpecific15Mins(
      minorBettypeId,
      bettypeName,
      assignments
    ) &&
    checkAllowAddAssignmentSpecific10Mins(
      minorBettypeId,
      bettypeName,
      assignments
    ) &&
    checkAllowAddAssignmentFM(minorBettypeId, bettypeName, assignments)
  );
};

const checkAllowAddAssignmentPHBetTypes = (
  minorBettypeId,
  bettypeName,
  assignments
) => {
  const isFMOrAIO = isEqual(bettypeName, 'FM') || isEqual(bettypeName, 'AIO');
  const isMinorBettypeValid =
    minorBettypeId !== null &&
    ![MinorBetTypes.NoOfCorners, MinorBetTypes.TotalBooking].includes(
      minorBettypeId
    );

  if (isFMOrAIO && isMinorBettypeValid) {
    return assignments && assignments.length > 0;
  }

  return false;
};

const checkAllowAddAssignment = (
  minorBettypeId,
  bettypeName,
  assignments,
  policy
) => {
  const { FullBetTypes, PHBetTypes } = policy;
  if (!FullBetTypes.HasWrite && !PHBetTypes.HasWrite) {
    return false;
  }
  if (FullBetTypes.HasWrite) {
    return checkAllowAddAssignmentFullBetTypes(
      minorBettypeId,
      bettypeName,
      assignments
    );
  } else {
    return checkAllowAddAssignmentPHBetTypes(
      minorBettypeId,
      bettypeName,
      assignments
    );
  }
};

const onSavedAssigment = (dataSource, assigment, bettypeName, rowIndex) => {
  const newDataSource = dataSource.map(x => ({ ...x }));

  if (assigment && bettypeName) {
    const { events } = newDataSource;
    const event = { ...newDataSource[rowIndex] };
    const assignments = [...event[bettypeName]];

    if (assignments) {
      const index = findIndex(
        assignments,
        el =>
          isEqual(el.AssignmentId, assigment.AssignmentId) &&
          isEqual(el.OWPMatchId, assigment.OWPMatchId)
      );
      if (index < 0) {
        assignments.push(assigment);
      } else {
        assignments[index] = assigment;
      }

      remove(assignments, { AssignmentId: 0 });
      remove(assignments, { AssignmentId: null });
      event[bettypeName] = [...assignments];
    }
    newDataSource[rowIndex] = event;
    const indexOfAssignment = findIndex(events, el =>
      isEqual(el.OWPMatchId, event.OWPMatchId)
    );
    if (indexOfAssignment > -1) {
      events[indexOfAssignment] = event;
    }
    return newDataSource;
  }
  return undefined;
};

const onDeletedAssigment = (dataSource, assigment, bettypeName, rowIndex) => {
  const newDataSource = dataSource.map(x => ({ ...x }));
  if (assigment && bettypeName) {
    const { events } = newDataSource;
    const event = { ...newDataSource[rowIndex] };

    if (assigment.AssignmentType === 1) {
      event[bettypeName] = [];
      assigment.UserId = 0;
      assigment.AssignmentId = 0;
    } else {
      const assignments = [...event[bettypeName]];
      remove(assignments, { AssignmentId: assigment.AssignmentId });
      event[bettypeName] = [...assignments];
    }

    newDataSource[rowIndex] = event;
    const indexOfAssignment = findIndex(
      events,
      el => el.OWPMatchId === event.OWPMatchId
    );
    if (indexOfAssignment > -1) {
      events[indexOfAssignment] = event;
    }
    return newDataSource;
  }
  return undefined;
};

const buildAssignment = props => {
  const { data, rowIndex, bettypeName, policy } = props;
  const event = data[rowIndex];
  const otherTime = event.OtherTime;
  const assignments = event[bettypeName];
  let main = { OWPMatchId: 0 };
  let others = [];
  if (assignments) {
    main = find(assignments, { AssignmentType: 1 });
    others = filter(assignments, { AssignmentType: 2 });
  }
  if (!main) {
    main = find(assignments, { AssignmentType: 0 });
  }
  if (!main) {
    main = { OWPMatchId: event.OWPMatchId };
  }

  const assignmentBetType = AssignmentBettype[bettypeName];
  const isAllowAdd = checkAllowAddAssignment(
    event.MinorBettypeId,
    bettypeName,
    assignments,
    policy
  );
  const keyIndex = uuidv4();
  const policyForBetType = {
    UserId: policy.UserId,
    Username: policy.Username,
    HasWrite: buildWritePermission(bettypeName, policy)
  };
  return {
    keyIndex,
    policyForBetType,
    main,
    others,
    assignmentBetType,
    isAllowAdd,
    otherTime
  };
};

export {
  checkAllowAddAssignment,
  onSavedAssigment,
  buildAssignment,
  onDeletedAssigment
};
