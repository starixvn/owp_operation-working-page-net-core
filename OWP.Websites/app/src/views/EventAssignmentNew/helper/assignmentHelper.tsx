import { isEqual } from 'lodash';
import { ASSIGNMENT_BETTYPE } from '../const.json';
import { v4 as uuidv4 } from 'uuid';
import { TAssignmentBuilder, TAssignment, TPolicy } from '../type/type';
import { MinorBetTypes } from '../../Shared/Constants';

const checkAllowAddAssignmentSpecific15Mins = (
  minorBettypeId: number,
  bettypeName: string,
  assignments: TAssignment[]
) => {
  if (
    isEqual(bettypeName, 'Specific15Mins') &&
    minorBettypeId !== null &&
    !(
      minorBettypeId === MinorBetTypes.NoOfCorners ||
      minorBettypeId === MinorBetTypes.TotalBooking
    )
  ) {
    return assignments?.length > 0;
  }

  return true;
};

const checkAllowAddAssignmentSpecific10Mins = (
  minorBettypeId: number,
  bettypeName: string,
  assignments: TAssignment[]
) => {
  if (isEqual(bettypeName, 'Specific10Mins') && minorBettypeId !== null) {
    return assignments?.length > 0;
  }

  return true;
};

const checkAllowAddAssignmentFM = (
  minorBettypeId: number,
  bettypeName: string,
  assignments: TAssignment[]
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
  minorBettypeId: number,
  bettypeName: string,
  assignments: TAssignment[]
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
  minorBettypeId: number,
  bettypeName: string,
  assignments: TAssignment[]
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
  minorBettypeId: number,
  bettypeName: string,
  assignments: TAssignment[],
  policy: TPolicy
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

const buildWritePermission = (bettypeName: string, policy: TPolicy) => {
  const { FullBetTypes, PHBetTypes } = policy;

  return (
    FullBetTypes.HasWrite ||
    (PHBetTypes.HasWrite &&
      (isEqual(bettypeName, 'FM') || isEqual(bettypeName, 'AIO')))
  );
};

const buildAssignment = (props: TAssignmentBuilder, policy: TPolicy) => {
  const { dataSource, rowIndex, bettypeName } = props;
  const event = dataSource[rowIndex];
  const otherTime = event.OtherTime;
  const assignments = event[bettypeName];
  let main = null;
  let others = null;
  if (assignments) {
    main = assignments.find(a => a.AssignmentType === 1);
    others = assignments.filter(a => a.AssignmentType === 2);
  }
  if (!main) {
    main = assignments.find(a => a.AssignmentType === 0);
  }
  if (!main) {
    main = { OWPMatchId: event.OWPMatchId };
  }

  const assignmentBetType = ASSIGNMENT_BETTYPE[bettypeName];
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

export { buildAssignment };
