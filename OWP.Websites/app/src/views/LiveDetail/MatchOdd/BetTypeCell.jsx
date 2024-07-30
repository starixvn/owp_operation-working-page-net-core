import { Component } from 'react';
import PropTypes from 'prop-types';
import Select from 'react-select';
import Animated from 'react-select/animated';
import { getBetTypeName } from '../../Shared/BetTypes';

const propTypes = {
  betType: PropTypes.number.isRequired,
  betTypeOptions: PropTypes.array.isRequired,
  isCreating: PropTypes.bool,
  onChange: PropTypes.func.isRequired
};

class BetTypeCell extends Component {
  constructor(props) {
    super(props);
    const { betTypeOptions, betType } = this.props;
    const newOptions = betTypeOptions.map(bt => {
      return {
        value: bt,
        label: getBetTypeName(bt)
      };
    });
    const selectedOption = newOptions.filter(
      option => option.value === betType
    )[0];
    this.state = { selectedOption, newOptions };
  }

  onChange(selectedOption) {
    const { onChange } = this.props;
    this.setState({ selectedOption });
    onChange(selectedOption.value);
  }

  render() {
    const { betType, isCreating } = this.props;
    const { selectedOption, newOptions } = this.state;
    if (isCreating) {
      return (
        <td>
          <div className='content select-up'>
            <Select
              placeholder='Select'
              components={Animated}
              options={newOptions}
              value={selectedOption}
              className='basic-multi-select'
              classNamePrefix='select'
              menuPlacement='top'
              onChange={option => this.onChange(option)}
            />
          </div>
        </td>
      );
    } else {
      return <td>{getBetTypeName(betType)}</td>;
    }
  }
}

BetTypeCell.propTypes = propTypes;
export default BetTypeCell;
