import { useCallback } from 'react';
import { buildAssignment } from '../helper/assignmentHelper';
import { TAssignment, TAssignmentBuilder, TBettype } from '../type/type';
import { useAppDispatch } from '../app/hooks';
import EditableButtonBase from '../../components/Assignment/EditableButtonBase';
import {
  onUpdatedAssignment,
  onDeletedAssignment
} from '../table/eventAssignmentTableSlice';
import Utils from '../../Shared/Utils';
import store from '../app/store';

const BettypeCell = (props: TAssignmentBuilder) => {
  const dispatch = useAppDispatch();

  const {
    policy,
    filter: {
      filterState: { selectedDate }
    },
    allowAddEvent
  } = store.getState();

  const assignment = buildAssignment(props, policy.entities);
  const {
    keyIndex,
    policyForBetType,
    main,
    others,
    assignmentBetType,
    isAllowAdd,
    otherTime
  } = assignment;

  const { bettypeName, rowIndex } = props;

  const handleOnSavedAssignment = useCallback((
    assignmentUpdate: TAssignment,
    bettypeName: TBettype,
    rowIndex: number
  ) => {
    dispatch(
      onUpdatedAssignment({
        assignment: assignmentUpdate,
        bettypeName,
        rowIndex
      })
    );
  }, [dispatch]);

  const handleOnDeletedAssignment = useCallback((
    assignmentUpdate: TAssignment,
    bettypeName: TBettype,
    rowIndex: number
  ) => {
    dispatch(
      onDeletedAssignment({
        assignment: assignmentUpdate,
        bettypeName,
        rowIndex
      })
    );
  }, [dispatch]);

  return (
    <EditableButtonBase
      key={keyIndex}
      policy={policyForBetType}
      filterDate={Utils.formatDate(selectedDate)}
      main={main}
      others={others}
      matchId={main.OWPMatchId}
      betType={assignmentBetType}
      isAllowAdd={isAllowAdd && allowAddEvent.isYesterdayOrAfter}
      index={keyIndex}
      columnName={bettypeName}
      rowIndex={rowIndex}
      onSaved={handleOnSavedAssignment}
      onDeleted={handleOnDeletedAssignment}
      otherTime={otherTime}
    />
  );
};

export default BettypeCell;
