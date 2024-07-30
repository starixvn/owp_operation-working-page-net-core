import { Component } from 'react';
import PropTypes from 'prop-types';
import NumericTextbox from 'react-numeric-textbox';

const propTypes = {
  Spread: PropTypes.number,
  Maxbet: PropTypes.number,
  isEditing: PropTypes.bool,
  labelMaxbet: PropTypes.string,
  labelSpread: PropTypes.string,
  handleMaxbetChange: PropTypes.func,
  handleSpreadChange: PropTypes.func,
  highlightInput: PropTypes.bool
};

class MaxbetSpreadCell extends Component {
  render() {
    const {
      Maxbet,
      Spread,
      isEditing,
      labelMaxbet,
      labelSpread,
      highlightInput
    } = this.props;
    return (
      <td>
        <div className='content'>
          <NumericTextbox
            className={'input-field' + (highlightInput ? ' input-invalid' : '')}
            value={Maxbet}
            min={0}
            max={99999999}
            decimals={3}
            autoCorrect
            format=''
            disabled={!isEditing}
            name={labelMaxbet}
            onChange={event => this.props.handleMaxbetChange(event)}
          />
          <span>K</span>
          <NumericTextbox
            className={'input-field' + (highlightInput ? ' input-invalid' : '')}
            value={Spread}
            min={0}
            max={99}
            decimals={2}
            autoCorrect
            disabled={!isEditing}
            format=''
            name={labelSpread}
            onChange={event => this.props.handleSpreadChange(event)}
          />
          <span>%</span>
        </div>
      </td>
    );
  }
}
MaxbetSpreadCell.propTypes = propTypes;
export default MaxbetSpreadCell;
