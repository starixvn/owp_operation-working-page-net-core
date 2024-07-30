import { Component } from 'react';
import PropTypes from 'prop-types';
import { Table } from 'reactstrap';
import MatchSettingRow from './MatchSettingRow';

const propTypes = {
  isCreating: PropTypes.bool.isRequired,
  isEditing: PropTypes.bool.isRequired,
  editingIndex: PropTypes.number.isRequired,
  onEdit: PropTypes.func.isRequired,
  onDelete: PropTypes.func.isRequired,
  onSave: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
  matchSettings: PropTypes.array.isRequired,
  minorBettypeId: PropTypes.number,
  policy: PropTypes.object
};

class MatchSettingTable extends Component {
  render() {
    const { policy } = this.props;
    return (
      <Table bordered>
        <thead>
          <tr>
            <th className='col-bettype'>BetType</th>
            <th>MBC</th>
            {policy.HasWrite && <th className='action-col'>Action</th>}
          </tr>
        </thead>
        <tbody>{this.renderBody()}</tbody>
      </Table>
    );
  }

  renderBody() {
    const { matchSettings } = this.props;
    if (matchSettings.length === 0) {
      return (
        <tr>
          <td colSpan='4'>There is no data</td>
        </tr>
      );
    } else {
      return this.renderRows(matchSettings);
    }
  }

  renderRows(matchSettings) {
    const {
      policy,
      isCreating,
      isEditing,
      editingIndex,
      onEdit,
      onDelete,
      onSave,
      onCancel,
      minorBettypeId
    } = this.props;
    return matchSettings.map((setting, index) => {
      const key =
        isCreating && index === editingIndex
          ? 'new'
          : `${setting.BettypeId}-${index}`;
      return (
        <MatchSettingRow
          key={key}
          policy={policy}
          index={index}
          minorBettypeId={minorBettypeId}
          matchSetting={setting}
          isCreating={isCreating && editingIndex === index}
          isEditing={isEditing && editingIndex === index}
          allowEdit={!isEditing || editingIndex === index}
          onEdit={onEdit}
          onDelete={onDelete}
          onSave={onSave}
          onCancel={onCancel}
        />
      );
    });
  }
}

MatchSettingTable.propTypes = propTypes;
export default MatchSettingTable;
