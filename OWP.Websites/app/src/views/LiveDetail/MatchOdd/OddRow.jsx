import { Component } from 'react';
import PropTypes from 'prop-types';
import BetTypeCell from './BetTypeCell';
import GoalCell from './GoalCell';
import OddCell from './OddCell';
import { MajorBetTypes } from '../../Shared/Constants';
import service from './Service';

const propTypes = {
  policy: PropTypes.object.isRequired,
  index: PropTypes.number.isRequired,
  isCreating: PropTypes.bool.isRequired,
  isEditing: PropTypes.bool.isRequired,
  allowEdit: PropTypes.bool.isRequired,
  isMultipleEdit: PropTypes.bool,
  row: PropTypes.object.isRequired,
  betTypeOptions: PropTypes.array,
  onEdit: PropTypes.func.isRequired,
  onDelete: PropTypes.func.isRequired,
  onSave: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
  onRowChange: PropTypes.func
};

class OddRow extends Component {
  constructor(props) {
    super(props);
    const { row } = this.props;
    const { BetType, HDP, HomeOdd, AwayOdd, DrawOdd, TeamChoice } = row;
    this.state = { BetType, HDP, HomeOdd, AwayOdd, DrawOdd, TeamChoice };
    this.onHandicapChange = this.onHandicapChange.bind(this);
  }

  get row() {
    const { BetType, HDP, HomeOdd, AwayOdd, DrawOdd, TeamChoice } = this.state;
    return {
      BetType,
      HDP,
      HomeOdd,
      AwayOdd,
      DrawOdd,
      TeamChoice
    };
  }

  render() {
    const { isCreating, isEditing, betTypeOptions } = this.props;
    const { BetType, HomeOdd, AwayOdd } = this.state;
    return (
      <tr>
        <BetTypeCell
          betTypeOptions={betTypeOptions}
          betType={BetType || betTypeOptions[0]}
          isCreating={isCreating}
          onChange={value => this.onRowChange('BetType', value)}
        />
        {this.renderGoalCell()}
        <OddCell
          odd={HomeOdd}
          isEditing={isEditing}
          onChange={value => this.onRowChange('HomeOdd', value)}
        />
        <OddCell
          odd={AwayOdd}
          isEditing={isEditing}
          onChange={value => this.onRowChange('AwayOdd', value)}
        />
        {this.renderDrawCell()}
      </tr>
    );
  }

  renderGoalCell() {
    const { isEditing, betTypeOptions } = this.props;
    const { BetType, HDP } = this.state;
    if (BetType === MajorBetTypes.OneXTwo) {
      return this.renderEmptyCell();
    } else {
      return (
        <GoalCell
          teamChoice={this.state.TeamChoice}
          betType={BetType || betTypeOptions[0]}
          handicap={HDP}
          isEditing={isEditing}
          onChange={this.onHandicapChange}
        />
      );
    }
  }

  renderDrawCell() {
    const { isEditing } = this.props;
    const { BetType, DrawOdd } = this.state;
    if (BetType === MajorBetTypes.OneXTwo) {
      return (
        <OddCell
          odd={DrawOdd}
          isEditing={isEditing}
          onChange={event => this.onRowChange('DrawOdd', event)}
        />
      );
    } else {
      return this.renderEmptyCell();
    }
  }

  renderEmptyCell() {
    return <td />;
  }

  onHandicapChange(handicap, selectedOption) {
    const teamChoice = selectedOption;
    this.setState({ HDP: handicap, TeamChoice: teamChoice }, () =>
      this.raiseRowChange()
    );
  }

  onRowChange = (key, value) => {
    this.setState({ [key]: value }, () => {
      this.raiseRowChange();
    });
  };

  raiseRowChange = () => {
    const { betTypeOptions } = this.props;
    const { BetType, HDP, HomeOdd, AwayOdd, TeamChoice, DrawOdd } = this.state;
    const newRow = {
      BetType: BetType || betTypeOptions[0],
      HDP,
      HomeOdd,
      AwayOdd,
      DrawOdd,
      TeamChoice
    };
    this.props.onRowChange(newRow, this.props.index);
  };

  onEdit() {
    const { index, onEdit } = this.props;
    onEdit(index);
  }

  onDelete() {
    const { onDelete, index } = this.props;
    onDelete(index);
  }

  onSave() {
    const { onSave, betTypeOptions } = this.props;
    const { BetType, HDP, HomeOdd, AwayOdd, DrawOdd, TeamChoice } = this.state;
    const newRow = {
      BetType: BetType || betTypeOptions[0],
      HDP,
      HomeOdd,
      AwayOdd,
      DrawOdd,
      TeamChoice
    };
    if (!service.validate(newRow)) {
      return;
    }

    onSave(newRow);
  }

  onCancel() {
    const { onCancel, row } = this.props;
    const { BetType, HDP, HomeOdd, AwayOdd, DrawOdd, TeamChoice } = row;
    this.setState({ BetType, HDP, HomeOdd, AwayOdd, DrawOdd, TeamChoice });
    onCancel();
  }
}

OddRow.propTypes = propTypes;
export default OddRow;
