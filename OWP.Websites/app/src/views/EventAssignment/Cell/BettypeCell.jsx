import { PureComponent } from 'react';
import { buildAssignment } from '../AssignmentHelper';
import EditableButtonGroup from '../../components/Assignment/EditableButtonGroup';
import PropTypes from 'prop-types';

class BettypeCell extends PureComponent {
  render() {
    const assignment = buildAssignment(this.props);
    const {
      keyIndex,
      policyForBetType,
      main,
      others,
      assignmentBetType,
      isAllowAdd,
      isLoading,
      otherTime
    } = assignment;

    const { bettypeName, rowIndex, onSavedAssigment, onDeletedAssigment } =
      this.props;
    if (isLoading) {
      return <span>Loading</span>;
    }
    return (
      <EditableButtonGroup
        key={keyIndex}
        policy={policyForBetType}
        main={main}
        others={others}
        matchId={main.OWPMatchId}
        betType={assignmentBetType}
        isAllowAdd={isAllowAdd}
        index={keyIndex}
        columnName={bettypeName}
        rowIndex={rowIndex}
        onSaved={onSavedAssigment}
        onDeleted={onDeletedAssigment}
        otherTime={otherTime}
      />
    );
  }
}

const propTypesBettype = {
  bettypeName: PropTypes.string.isRequired,
  rowIndex: PropTypes.number.isRequired,
  onSavedAssigment: PropTypes.func.isRequired,
  onDeletedAssigment: PropTypes.func.isRequired
};

BettypeCell.propTypes = propTypesBettype;
export default BettypeCell;
