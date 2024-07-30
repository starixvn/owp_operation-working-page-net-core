import { Component } from 'react';
import PropTypes from 'prop-types';
import PlusIcon from 'mdi-react/PlusIcon';
import ChevronDownIcon from 'mdi-react/ChevronDownIcon';
import ChevronRightIcon from 'mdi-react/ChevronRightIcon';
import { Button, CardHeader, Collapse } from 'reactstrap';
import { isEqual, isNil } from 'lodash';
import MatchSettingTable from './MatchSettingTable';
import Matchservice from './Service';
import {
  allBettype,
  bettypeIdXminOfNoOfCornersTotalBooking,
  bettypeIdDetectNoOfCornersTotalBooking,
  bettypeIdXmins
} from '../../Shared/Constants';
import Toastr from '../../components/ToastrWrapper';
import Spinner from '../../components/Spinner';
import Utils from '../../Shared/Utils';

const propTypes = {
  policy: PropTypes.object,
  matchId: PropTypes.number,
  minorBettypeId: PropTypes.number,
  onSave: PropTypes.func.isRequired,
  data: PropTypes.object.isRequired
};

class MatchSettingPanel extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isCreating: false,
      isOpen: true,
      isEditing: false,
      editingIndex: 0,
      isLoading: true,
      matchSettings: []
    };
    this.toggle = this.toggle.bind(this);
    this.newMatch = this.newMatch.bind(this);
    this.onDelete = this.onDelete.bind(this);
  }

  renderMatchOffers = items => {
    const text = items.map(item => item.label).join(', ');
    if (text) {
      return <span className='KO'>{text}</span>;
    }

    return null;
  };

  render() {
    const { policy, matchId, minorBettypeId, data } = this.props;
    const {
      isCreating,
      isEditing,
      isOpen,
      editingIndex,
      matchSettings,
      isLoading
    } = this.state;
    return (
      <div className='widget-wrap odds'>
        <CardHeader className={isOpen ? 'card-expand' : 'card-collapse'}>
          <Button className='title' color='default' onClick={this.toggle}>
            {isOpen ? (
              <ChevronDownIcon size={16} />
            ) : (
              <ChevronRightIcon size={16} />
            )}
            Maxbet/Spread {this.renderMatchOffers(data.MatchOffers)}
          </Button>
          {policy.HasWrite && (
            <Button
              className='btn-add-odd'
              onClick={this.newMatch}
              disabled={isCreating || isEditing}>
              <PlusIcon size={15} />
              <span>Add</span>
            </Button>
          )}
        </CardHeader>
        {isLoading ? (
          <div>Loading ...</div>
        ) : (
          <Collapse isOpen={isOpen}>
            <MatchSettingTable
              matchSettings={matchSettings}
              policy={policy}
              editingIndex={editingIndex}
              isCreating={isCreating}
              matchId={matchId}
              minorBettypeId={minorBettypeId}
              isEditing={isEditing}
              onEdit={this.onEdit}
              onDelete={this.onDelete}
              onSave={this.onSave}
              onCancel={this.onCancel}
            />
          </Collapse>
        )}
      </div>
    );
  }

  componentDidMount() {
    (async () => {
      const { matchId } = this.props;
      let matchSettings = [];
      if (!isNil(matchId)) {
        matchSettings = await Matchservice.getMatchsettingByMatchId(matchId);
      }
      const newMatchSettings = matchSettings.map(m => ({
        BettypeId: bettypeIdXminOfNoOfCornersTotalBooking.some(
          b => b === m.BettypeId
        )
          ? -1
          : m.BettypeId,
        Maxbet: m.Maxbet > 0 ? Utils.convertToThousand(m.Maxbet) : m.Maxbet,
        Spread: m.Spread > 0 ? Utils.convertToPercent(m.Spread) : m.Spread
      }));
      this.setState({
        isLoading: false,
        matchSettings: newMatchSettings
      });
    })();
  }

  toggle = () => {
    const { isOpen } = this.state;
    this.setState({ isOpen: !isOpen });
  };

  newMatch = () => {
    const { matchSettings } = this.state;
    const newMatchSetting = {
      BettypeId: null
    };
    this.setState({
      isCreating: true,
      isEditing: true,
      editingIndex: 0,
      matchSettings: [newMatchSetting].concat(matchSettings)
    });
  };

  onEdit = index => {
    this.setState({
      editingIndex: index,
      isEditing: true
    });
  };

  onSave = async matchSetting => {
    const { matchSettings, editingIndex, isCreating } = this.state;
    const { Maxbet, Spread } = matchSetting;
    let { BettypeId } = matchSetting;
    const { UserId } = this.props.policy;
    if (isEqual(matchSettings[editingIndex], matchSetting)) {
      this.setState({
        isCreating: false,
        isEditing: false
      });
      return;
    }

    const sameBettype = matchSettings.some(
      (item, itemIndex) =>
        item.BettypeId === matchSetting.BettypeId && itemIndex !== editingIndex
    );
    if (sameBettype) {
      Toastr.showError(
        'The bettype is already existed. Please choose another one!'
      );
      return;
    }

    const { minorBettypeId } = this.props;
    const Bettype = allBettype.find(a => a.value === BettypeId).label;

    BettypeId = this.getStandardBetTypeIfTotalBooking(
      minorBettypeId,
      BettypeId
    );

    const postModel = {
      UserId,
      MinorBettypeId: minorBettypeId,
      MatchId: this.props.matchId,
      BettypeId,
      Bettype,
      Maxbet: this.getStandardizeMaxBet(Maxbet),
      Spread: this.getStandardizeSpread(Spread)
    };

    await this.createOrUpdateMatchSetting(isCreating, postModel);

    matchSettings[editingIndex] = matchSetting;
    this.setState({
      isCreating: false,
      isEditing: false,
      matchSettings
    });
    this.props.onSave();
  };

  getStandardBetTypeIfTotalBooking(minorBettypeId, bettypeId) {
    if (
      minorBettypeId === bettypeIdDetectNoOfCornersTotalBooking[0] &&
      bettypeId === bettypeIdXmins
    ) {
      return bettypeIdXminOfNoOfCornersTotalBooking[0];
    }

    if (
      minorBettypeId === bettypeIdDetectNoOfCornersTotalBooking[1] &&
      bettypeId === bettypeIdXmins
    ) {
      return bettypeIdXminOfNoOfCornersTotalBooking[1];
    }

    return bettypeId;
  }

  getStandardizeMaxBet(maxbet) {
    return isNil(maxbet) ? null : maxbet * 1000;
  }

  getStandardizeSpread(spread) {
    return isNil(spread) ? null : spread / 100;
  }

  async createOrUpdateMatchSetting(isCreating, postModel) {
    Spinner.show();
    isCreating
      ? await Matchservice.createMatchSetting(postModel)
      : await Matchservice.updateMatchSetting(postModel);
    Spinner.hide();
  }

  onCancel = () => {
    const { isCreating, matchSettings } = this.state;
    const newMatchSettings = isCreating
      ? matchSettings.slice(1)
      : matchSettings.slice();

    this.setState({
      isCreating: false,
      isEditing: false,
      matchSettings: newMatchSettings
    });
  };

  async onDelete(index) {
    const result = await Toastr.confirm(
      'Are you sure you want to permanently delete this matchsetting?'
    );
    if (result.value) {
      const { matchSettings } = this.state;
      const { UserId } = this.props.policy;
      let { BettypeId } = matchSettings[index];
      const { minorBettypeId, matchId } = this.props;
      const Bettype = allBettype.find(a => a.value === BettypeId).label;
      if (
        minorBettypeId === bettypeIdDetectNoOfCornersTotalBooking[0] &&
        BettypeId === bettypeIdXmins
      ) {
        BettypeId = bettypeIdXminOfNoOfCornersTotalBooking[0];
      }

      if (
        minorBettypeId === bettypeIdDetectNoOfCornersTotalBooking[1] &&
        BettypeId === bettypeIdXmins
      ) {
        BettypeId = bettypeIdXminOfNoOfCornersTotalBooking[1];
      }

      const deleteModel = {
        UserId,
        Bettype,
        MinorBettypeId: minorBettypeId,
        MatchId: matchId,
        BettypeId
      };
      Spinner.show();
      await Matchservice.deleteMatchSetting(deleteModel);
      Spinner.hide();
      this.setState({
        matchSettings: matchSettings.filter(
          (_, itemIndex) => itemIndex !== index
        )
      });
      this.props.onSave();
    }
  }
}

MatchSettingPanel.propTypes = propTypes;
export default MatchSettingPanel;
