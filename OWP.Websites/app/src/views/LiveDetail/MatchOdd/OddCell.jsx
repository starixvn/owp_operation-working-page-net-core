import { Component } from 'react';
import PropTypes from 'prop-types';
import { isNil } from 'lodash';
import NumericTextbox from 'react-numeric-textbox';

const propTypes = {
  odd: PropTypes.number,
  isEditing: PropTypes.bool,
  onChange: PropTypes.func.isRequired
};

class OddCell extends Component {
  onChange(odd) {
    const { onChange } = this.props;
    onChange(odd);
  }

  render() {
    const { odd, isEditing } = this.props;
    if (isEditing) {
      return (
        <td>
          <div className='content'>
            <NumericTextbox
              className={'input-field' + (isNil(odd) ? ' input-invalid' : '')}
              min={-1000}
              max={1000}
              decimals={2}
              autoCorrect
              value={odd}
              format=''
              onChange={event => this.onChange(event)}
            />
          </div>
        </td>
      );
    } else {
      return <td className={odd < 0 ? 'negative-odd' : ''}>{odd}</td>;
    }
  }
}

OddCell.propTypes = propTypes;
export default OddCell;
