import { Component } from 'react';
import PropTypes from 'prop-types';
import ActionCell from '../MatchOdd/ActionCell';
import MaxbetSpreadCell from './MaxbetSpreadCell';
import { isNil } from 'lodash';
import BettypeCell from './BettypeCell';

const propTypes = {
  policy: PropTypes.object,
  isCreating: PropTypes.bool,
  isEditing: PropTypes.bool,
  matchSetting: PropTypes.object,
  index: PropTypes.number,
  onSave: PropTypes.func,
  onEdit: PropTypes.func,
  onCancel: PropTypes.func,
  onDelete: PropTypes.func,
  allowEdit: PropTypes.bool,
  minorBettypeId: PropTypes.number
};

class MatchSettingRow extends Component {
  constructor(props) {
    super(props);
    const { matchSetting } = this.props;
    const { BettypeId, Maxbet, Spread } = matchSetting;
    this.state = { BettypeId, Maxbet, Spread };
  }

  render() {
    const {
      policy,
      isCreating,
      isEditing,
      allowEdit,
      minorBettypeId
    } = this.props;
    const { BettypeId, Maxbet, Spread } = this.state;
    const highlightInput = !this.validMaxbetSpread();
    return (
      <tr>
        <BettypeCell
          minorBettypeId={minorBettypeId}
          BettypeId={BettypeId}
          isDisabled={!isCreating}
          handleBettypeChange={this.handleBettypeChange}
        />
        <MaxbetSpreadCell
          Maxbet={Maxbet}
          Spread={Spread}
          isEditing={isEditing || isCreating}
          labelMaxbet='maxbet'
          labelSpread='spread'
          highlightInput={highlightInput}
          handleMaxbetChange={this.handleMaxbetChange}
          handleSpreadChange={this.handleSpreadChange}
        />
        {policy.HasWrite && (
          <ActionCell
            isEditing={isEditing || isCreating}
            disabled={!allowEdit}
            onSave={this.onSave}
            onCancel={this.onCancel}
            onEdit={this.onEdit}
            onDelete={this.onDelete}
          />
        )}
      </tr>
    );
  }

  handleBettypeChange = bettypeId => {
    this.setState({ BettypeId: bettypeId });
  };

  handleMaxbetChange = maxbet => {
    this.setState({ Maxbet: maxbet });
  };

  handleSpreadChange = spread => {
    this.setState({ Spread: spread });
  };

  validMaxbetSpread = () => {
    const { Maxbet, Spread } = this.state;
    return (
      !isNil(Maxbet) ||
      !isNil(Spread)
    );
  };

  onSave = () => {
    const { onSave } = this.props;
    const { BettypeId, Maxbet, Spread } = this.state;
    if (!isNil(BettypeId) && this.validMaxbetSpread()) {
      onSave({
        BettypeId,
        Maxbet,
        Spread
      });
    }
  };

  onCancel = () => {
    const { onCancel, matchSetting } = this.props;
    const { BettypeId, Maxbet, Spread } = matchSetting;
    this.setState(
      {
        BettypeId,
        Maxbet,
        Spread
      },
      () => onCancel()
    );
  };

  onEdit = () => {
    const { index, onEdit } = this.props;
    onEdit(index);
  };

  onDelete = () => {
    const { index, onDelete } = this.props;
    onDelete(index);
  };
}

MatchSettingRow.propTypes = propTypes;
export default MatchSettingRow;
