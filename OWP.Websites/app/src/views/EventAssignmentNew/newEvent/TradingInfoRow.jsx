import { Component } from 'react';
import Select from 'react-select';
import DeleteIcon from 'mdi-react/DeleteIcon';
import PropTypes from 'prop-types';
import NumericTextbox from 'react-numeric-textbox';

const propTypes = {
  handleDeleteMatchSetting: PropTypes.func,
  handleMatchSettingChange: PropTypes.func,
  bettypesOpition: PropTypes.array
};

class TradingInfoRow extends Component {
  constructor(props) {
    super(props);
    this.notifyDeleteMatchSetting = this.notifyDeleteMatchSetting.bind(this);
    this.notifyMatchSettingChange = this.notifyMatchSettingChange.bind(this);
    this.state = {
      onlyRenderField: false
    };
  }

  static getDerivedStateFromProps = (nextProps, state) => {
    const { onlyRenderField } = state;
    if (!onlyRenderField) {
      const { matchSetting } = nextProps;
      return {
        bettype: matchSetting.bettype,
        maxbet: matchSetting.maxbet,
        spread: matchSetting.spread
      };
    }
    return {
      onlyRenderField: false
    };
  };

  notifyMatchSettingChange = match => {
    const key = Object.keys(match)[0];
    const value = Object.values(match)[0];
    this.setState(
      {
        [key]: value,
        onlyRenderField: true
      },
      () => this.props.handleMatchSettingChange(match)
    );
  };

  notifyDeleteMatchSetting = () => {
    this.props.handleDeleteMatchSetting();
  };

  render() {
    const { bettype, maxbet, spread } = this.state;
    const { bettypesOpition } = this.props;
    const { notifyDeleteMatchSetting, notifyMatchSettingChange } = this;
    return (
      <tr>
        <td>
          <Select
            options={bettypesOpition}
            className={
              'basic-multi-select' + (bettype === '' ? ' input-invalid' : '')
            }
            classNamePrefix='select'
            menuPlacement='top'
            value={bettype}
            name='bettype'
            onChange={event => notifyMatchSettingChange({ bettype: event })}
          />
        </td>
        <td>
          <div className='input-value'>
            <NumericTextbox
              className='input-field'
              value={maxbet}
              min={0}
              max={99999999}
              decimals={3}
              autoCorrect
              format=''
              name='maxbet'
              onChange={event => notifyMatchSettingChange({ maxbet: event })}
            />
            <span>K</span>
          </div>
          <div className='input-value'>
            <NumericTextbox
              className='input-field'
              value={spread}
              min={0}
              max={99}
              decimals={2}
              autoCorrect
              format=''
              name='spread'
              onChange={event => notifyMatchSettingChange({ spread: event })}
            />
            <span>%</span>
          </div>
        </td>
        <td className='text-center icon-button'>
          <button className='icon-delete'>
            <DeleteIcon
              className='delete-btn'
              name='iconDelete'
              size={12}
              onClick={() => notifyDeleteMatchSetting()}
            />
          </button>
        </td>
      </tr>
    );
  }
}

TradingInfoRow.propTypes = propTypes;
export default TradingInfoRow;
