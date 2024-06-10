import { Component } from 'react';
import TradingInfoRow from './TradingInfoRow';
import Converter from '../Shared/Converter';
import { Table } from 'reactstrap';
import PlusIcon from 'mdi-react/PlusIcon';
import PropTypes from 'prop-types';
import { isNil } from 'lodash';

let executeReset = false;
const maxRows = 9;
const propTypes = {
  bettypes: PropTypes.array
};

const updateMatchSettings = (matchSettings, position, match) => {
  const key = Object.keys(match)[0];
  const value = Object.values(match)[0];
  return matchSettings.map((matchSetting, index) => {
    if (index === position) {
      return {
        ...matchSetting,
        [key]: value
      };
    }
    return matchSetting;
  });
};

const createFirstAndSecondMatchSetting = bettypes => {
  const bettypesOpition = Converter.convertBettypesToOptions(bettypes);
  const matchSettingFirst = {
    bettype: bettypesOpition.find(b => b.label === 'HDP')
  };
  const matchSettingSecond = {
    bettype: bettypesOpition.find(b => b.label === 'O/U')
  };
  const matchSettings = [...[], matchSettingFirst, matchSettingSecond];
  return {
    matchSettings,
    bettypesOpition
  };
};

class TradingInfoTable extends Component {
  constructor(props) {
    super(props);
    this.state = {
      bettypesOpition: [],
      matchSettings: [],
      canAddMatchSetting: true,
      renderMatch: true
    };
    this.renderBody = this.renderBody.bind(this);
    this.renderMatchSettings = this.renderMatchSettings.bind(this);
    this.handleDeleteMatchSetting = this.handleDeleteMatchSetting.bind(this);
    this.handleMatchSettingChange = this.handleMatchSettingChange.bind(this);
  }

  static getDerivedStateFromProps = (nextProps, state) => {
    if (executeReset) {
      const { bettypes } = nextProps;
      const matchs = createFirstAndSecondMatchSetting(bettypes);
      executeReset = false;
      return {
        matchSettings: matchs.matchSettings,
        bettypesOpition: matchs.bettypesOpition,
        renderMatch: true
      };
    }
    return state;
  };

  renderBody() {
    const { matchSettings } = this.state;
    const { renderMatchSettings } = this;
    if (matchSettings.length === 0) {
      return (
        <tr>
          <td colSpan='4'>There is no data</td>
        </tr>
      );
    }
    return renderMatchSettings(matchSettings);
  }

  renderMatchSettings(matchSettings) {
    const { bettypesOpition } = this.state;
    const { handleMatchSettingChange, handleDeleteMatchSetting } = this;
    return matchSettings.map((matchSetting, index) => {
      const key = `${index}`;
      return (
        <TradingInfoRow
          handleMatchSettingChange={match =>
            handleMatchSettingChange(index, match)
          }
          handleDeleteMatchSetting={() => handleDeleteMatchSetting(index)}
          key={key}
          matchSetting={matchSetting}
          bettypesOpition={bettypesOpition}
        />
      );
    });
  }

  render() {
    const { renderBody, addNewMatchSetting } = this;
    const { canAddMatchSetting } = this.state;
    return (
      <div className='table tbl-maxbet'>
        <button
          name='addNewMatch'
          className='btn-add'
          disabled={!canAddMatchSetting}
          onClick={() => addNewMatchSetting()}>
          <PlusIcon size={15} />
          <span>Add</span>
        </button>
        <Table bordered>
          <thead>
            <tr>
              <th className='col-bettype'>BetType</th>
              <th>MBC</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>{renderBody()}</tbody>
        </Table>
      </div>
    );
  }

  reset() {
    executeReset = true;
  }

  validateAndGetData = () => {
    const { matchSettings } = this.state;
    for (let i = 0; i < matchSettings.length; i++) {
      const matchSetting = matchSettings[i];
      if (matchSetting.bettype === '') {
        return {
          code: 1,
          message: 'Please select bet type of Maxbet/Spread !'
        };
      }

      const sameMatchSettings = matchSettings.filter(
        m =>
          matchSetting.bettype && m.bettype.value === matchSetting.bettype.value
      );
      if (sameMatchSettings.length > 1) {
        return {
          code: 1,
          message: 'Duplicated Maxbet/Spread of a bet type. Please check !'
        };
      }
    }

    return {
      code: 0,
      matchSettings: matchSettings.map(m => ({
        Bettype: m.bettype.value,
        Maxbet: isNil(m.maxbet) ? null : m.maxbet * 1000,
        Spread: isNil(m.spread) ? null : m.spread / 100
      }))
    };
  };

  handleDeleteMatchSetting = position => {
    const { matchSettings } = this.state;
    this.setState({
      matchSettings: matchSettings.filter((_, index) => position !== index),
      canAddMatchSetting: true,
      renderMatch: true
    });
  };

  handleMatchSettingChange = (position, match) => {
    const { matchSettings } = this.state;
    const newMatchSettings = updateMatchSettings(
      matchSettings,
      position,
      match
    );
    this.setState({
      matchSettings: newMatchSettings,
      renderMatch: false
    });
  };

  addNewMatchSetting = () => {
    const { matchSettings } = this.state;
    const newMatchSettings = [
      ...matchSettings,
      {
        bettype: ''
      }
    ];
    this.setState({
      matchSettings: newMatchSettings,
      canAddMatchSetting: newMatchSettings.length !== maxRows,
      renderMatch: true
    });
  };

  shouldComponentUpdate(_, nextState) {
    const { renderMatch } = nextState;
    return renderMatch;
  }

  componentDidMount() {
    const { bettypes } = this.props;
    const matches = createFirstAndSecondMatchSetting(bettypes);
    this.setState({
      matchSettings: matches.matchSettings,
      bettypesOpition: matches.bettypesOpition
    });
  }
}

TradingInfoTable.propTypes = propTypes;
export default TradingInfoTable;
