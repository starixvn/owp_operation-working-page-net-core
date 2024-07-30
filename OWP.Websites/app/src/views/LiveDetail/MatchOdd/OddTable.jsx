import { createRef, Component } from 'react';
import PropTypes from 'prop-types';
import { Table } from 'reactstrap';
import { times, noop, filter, includes } from 'lodash';
import OddRow from './OddRow';
import { v4 as uuidv4 } from 'uuid';
import { MajorBetTypes } from '../../Shared/Constants';

const propTypes = {
  rows: PropTypes.array.isRequired,
  policy: PropTypes.object.isRequired,
  isCreating: PropTypes.bool.isRequired,
  isEditing: PropTypes.bool.isRequired,
  editingIndex: PropTypes.number.isRequired,
  isMultipleEdit: PropTypes.bool,
  onEdit: PropTypes.func.isRequired,
  onDelete: PropTypes.func.isRequired,
  onSave: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
  onRowChange: PropTypes.func
};

const defaultProp = {
  onRowChange: () => noop
};
const betTypes = [
  MajorBetTypes.Handicap,
  MajorBetTypes.OverUnder,
  MajorBetTypes.OneXTwo
];
class OddTable extends Component {
  constructor(props) {
    super(props);
    this.rowRefs = [];
    this.onRowChange = this.onRowChange.bind(this);
  }

  render() {
    return (
      <Table bordered>
        <thead>
          <tr>
            <th className='bettype-col'>Bet Type</th>
            <th>Goal</th>
            <th className='odd-col'>Home</th>
            <th className='odd-col'>Away</th>
            <th className='odd-col'>Draw</th>
          </tr>
        </thead>
        <tbody>{this.renderBody()}</tbody>
      </Table>
    );
  }

  get rows() {
    return this.rowRefs.map(ref => ref.current.row);
  }

  renderBody() {
    const { rows } = this.props;
    const getBetTypes = filter(rows, function (r) {
      return r.Id > 0;
    }).map(x => x.BetType);
    const betTypeOptions = filter(betTypes, function (bt) {
      return !includes(getBetTypes, bt);
    });
    this.rowRefs = times(rows.length, () => createRef());
    if (rows.length === 0) {
      return (
        <tr>
          <td colSpan='6'>There is no data</td>
        </tr>
      );
    } else {
      return this.renderRows(rows, betTypeOptions);
    }
  }

  onRowChange(row, index) {
    this.props.onRowChange(row, index);
  }

  renderRows(rows, betTypeOptions) {
    const {
      policy,
      isCreating,
      isEditing,
      editingIndex,
      onEdit,
      onDelete,
      onSave,
      onCancel,
      isMultipleEdit
    } = this.props;
    return rows.map((row, index) => {
      const { TeamChoice } = row;
      return (
        <OddRow
          key={uuidv4()}
          policy={policy}
          index={index}
          isCreating={isMultipleEdit || (isCreating && editingIndex === index)}
          isEditing={isMultipleEdit || (isEditing && editingIndex === index)}
          allowEdit={isMultipleEdit || !isEditing || editingIndex === index}
          isMultipleEdit={isMultipleEdit}
          row={row}
          betTypeOptions={betTypeOptions}
          teamChoice={TeamChoice}
          onRowChange={this.onRowChange}
          onEdit={onEdit}
          onDelete={onDelete}
          onSave={onSave}
          onCancel={onCancel}
          ref={this.rowRefs[index]}
        />
      );
    });
  }
}

OddTable.propTypes = propTypes;
OddTable.defaultProps = defaultProp;
export default OddTable;
