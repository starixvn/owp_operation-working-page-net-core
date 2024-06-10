import { PureComponent } from 'react';
import PropTypes from 'prop-types';
import Select from 'react-select';
import Animated from 'react-select/animated';
import NumericTextbox from 'react-numeric-textbox';
import { isNil } from 'lodash';
import { MajorBetTypes } from '../../Shared/Constants';

const propTypes = {
  betType: PropTypes.number.isRequired,
  handicap: PropTypes.number,
  teamChoice: PropTypes.oneOfType([PropTypes.object, PropTypes.number]),
  isEditing: PropTypes.bool,
  onChange: PropTypes.func.isRequired
};

class GoalCell extends PureComponent {
  constructor(props) {
    super(props);
    this.options = [
      { value: 1, label: 'Home' },
      { value: 2, label: 'Away' }
    ];
    this.onHandicapChange = this.onHandicapChange.bind(this);
    this.onTeamChange = this.onTeamChange.bind(this);
  }

  render() {
    const { isEditing, handicap } = this.props;
    if (isEditing) {
      return (
        <td className='goal-cell'>
          {this.renderTeamChoice()}
          <span>@</span>
          <div className='content'>
            <NumericTextbox
              className={
                'input-field goal' + (isNil(handicap) ? ' input-invalid' : '')
              }
              min={-1000}
              max={1000}
              decimals={2}
              autoCorrect
              value={handicap}
              format=''
              onChange={this.onHandicapChange}
            />
          </div>
        </td>
      );
    } else {
      const handicapText = handicap ? handicap.toString() : '0.0';
      return (
        <td>
          {this.renderTeam()}
          <span>@</span>
          <strong className={handicap < 0 ? 'negative-odd' : ''}>
            {handicapText}
          </strong>
        </td>
      );
    }
  }

  onHandicapChange(handicap) {
    this.raiseOnChange(this.props.teamChoice, handicap);
  }

  onTeamChange(selectedOption) {
    const teamChoice = selectedOption.value;
    this.raiseOnChange(teamChoice, this.props.handicap);
  }

  raiseOnChange(selectedOption, handicap) {
    const { onChange } = this.props;
    let handicapValue = parseFloat(handicap);
    if (isNaN(handicapValue)) {
      handicapValue = null;
    }
    onChange(handicapValue, selectedOption);
  }

  renderTeamChoice() {
    const { betType, teamChoice } = this.props;
    if (betType === MajorBetTypes.Handicap) {
      const selectedOption =
        teamChoice === 2 ? this.options[1] : this.options[0];
      return (
        <div className='content team-choice select-up'>
          <Select
            components={Animated}
            name='TeamChoice'
            options={this.options}
            value={selectedOption}
            className='basic-multi-select'
            classNamePrefix='select'
            menuPlacement='top'
            isSearchable={false}
            onChange={this.onTeamChange}
          />
        </div>
      );
    } else {
      return <div className='content' />;
    }
  }

  renderTeam() {
    const { betType, teamChoice } = this.props;
    if (betType === MajorBetTypes.Handicap) {
      const selectedOption =
        teamChoice === 2 ? this.options[1] : this.options[0];
      return <span>{selectedOption.label}</span>;
    } else {
      return <span />;
    }
  }
}

GoalCell.propTypes = propTypes;
export default GoalCell;
