import { createRef, Component } from 'react';
import axios from 'axios';
import PropTypes from 'prop-types';
import CloseCircleIcon from 'mdi-react/CloseCircleIcon';
import GeneralInfo from './MatchInfo/GeneralInfo';
import MatchInfo from './MatchInfo/MatchInfo';
import FormProviderInfo from './MatchInfo/FormProviderInfo';
import FormTradingInfo from './MatchInfo/FormTradingInfo';
import LiveAssignment from './Assignment/LiveAssignment';
import Notes from './MatchNote/Notes';
import OddPanel from './MatchOdd/OddPanel';
import UserService from '../Shared/User/UserService';
import MatchInfoService from './MatchInfo/Service';
import AuthorizeService from '../Authorize/Service';
import Loading from '../components/Loading';
import MatchSettingPanel from './MatchSetting/MatchSettingPanel';
import ResourceService from '../ResourceManagement/Service';
import NoteContext from '../LiveDetail/MatchNote/NoteContext';
import AssignmentContext from '../components/Assignment/AssignmentContext';
import isDeepEqual from 'lodash.isequal';
import Converter from '../Shared/Converter';
import Utils from '../Shared/Utils';
import { FunctionKeys } from '../Shared/Constants';

const editProviderConst = 'edit-provider-info';
const editTradingConst = 'edit-trading-info';
const editMatchConst = 'edit-match-info';
const editGeneralConst = 'edit-general-info';
const propTypes = {
  MatchId: PropTypes.number
};

class LiveDetail extends Component {
  constructor(props) {
    super(props);
    const paths = window.location.href.split('/');
    const matchId = props.MatchId || parseInt(paths[paths.length - 1], 10);
    this.state = {
      isLoading: true,
      matchId
    };
    this.formProviderInfo = createRef();
    this.onSaveGeneralInfo = this.onSaveGeneralInfo.bind(this);
    this.onSaveMatchInfo = this.onSaveMatchInfo.bind(this);
    this.onSaveTradingInfo = this.onSaveTradingInfo.bind(this);
    this.onSaveProviderInfo = this.onSaveProviderInfo.bind(this);
  }

  componentDidMount() {
    (async () => {
      const { matchId } = this.state;
      const [policy, notePolicy, users, matchInfo, noOfHDPs, matchOffers] =
        await axios.all([
          AuthorizeService.authorize(FunctionKeys.DetailsInformation),
          AuthorizeService.authorize(FunctionKeys.Note),
          UserService.dataSourceUsersFromServer(),
          MatchInfoService.query(matchId),
          ResourceService.getByRefName('NoOfHDP'),
          ResourceService.getByRefName('MatchOffer')
        ]);
      if (!matchInfo) {
        this.setState({
          isLoading: false
        });
        return;
      }

      const matchOfferOptions =
        Converter.convertMatchOffersToOptions(matchOffers);
      const noOfHDPOptions = Converter.convertNoOfHDPToOptions(noOfHDPs);
      matchInfo.NoOfHDP = noOfHDPOptions.filter(
        option => option.value === (matchInfo.NoOfHDPId || 0)
      )[0];
      matchInfo.MatchOffers = Converter.convertMatchOffersToOptions(
        matchInfo.MatchOffers
      );
      const isYesterdayOrAfter = Utils.isYesterdayOrAfterForLiveDetail(
        matchInfo.EventDate,
        matchInfo.KickOffTimeDisplay
      );
      policy.HasWrite = policy.HasWrite && isYesterdayOrAfter;
      notePolicy.HasWrite = notePolicy.HasWrite && isYesterdayOrAfter;
      this.setState({
        isLoading: false,
        policy,
        notePolicy,
        users,
        matchInfo,
        noOfHDPOptions,
        matchOfferOptions
      });
    })();
  }

  onUpdateAttribute(key, value) {
    if (window.frameElement) {
      window.frameElement.setAttribute('updated', 1);
      if (key && value) {
        window.frameElement.setAttribute(key, value);
      }
    }
  }

  onSave = () => {
    this.onUpdateAttribute(null, null);
  };

  onSaveProviderInfo = () => {
    this.onUpdateAttribute(editProviderConst, '0');
  };

  onSaveTradingInfo = data => {
    const { matchInfo } = this.state;
    if (!isDeepEqual(matchInfo.MatchOffers, data.MatchOffers)) {
      matchInfo.MatchOffers = data.MatchOffers;
      this.setState({
        matchInfo
      });
    }
    this.onUpdateAttribute(editTradingConst, '0');
  };

  onCancelEditMatchInfo = () => {
    this.onUpdateAttribute(editMatchConst, '0');
  };

  onEditMatchInfo = () => {
    this.onUpdateAttribute(editMatchConst, '1');
  };

  onEditGeneralInfo = () => {
    this.onUpdateAttribute(editGeneralConst, '1');
  };

  onCancelGeneralInfo = () => {
    this.onUpdateAttribute(editGeneralConst, '0');
  };

  onEditTradingInfo = () => {
    this.onUpdateAttribute(editTradingConst, '1');
  };

  onCancelTradingInfo = () => {
    this.onUpdateAttribute(editTradingConst, '0');
  };

  onEditProviderInfo = () => {
    this.onUpdateAttribute(editProviderConst, '1');
  };

  onCancelProviderInfo = () => {
    this.onUpdateAttribute(editProviderConst, '0');
  };

  onSaveMatchInfo = data => {
    const { matchInfo } = this.state;
    if (data.OtherTime !== matchInfo.OtherTime) {
      matchInfo.OtherTime = data.OtherTime;
      this.setState({
        matchInfo
      });
    }

    this.onUpdateAttribute(editMatchConst, '0');
  };

  onSaveGeneralInfo = async () => {
    this.onUpdateAttribute(editGeneralConst, '0');
    if (window.frameElement) {
      const editGeneralInfo =
        window.frameElement.getAttribute(editGeneralConst);
      const editMatchInfo = window.frameElement.getAttribute(editMatchConst);
      const editTradingInfo =
        window.frameElement.getAttribute(editTradingConst);
      const editProviderInfo =
        window.frameElement.getAttribute(editProviderConst);

      const isEditGeneralInfo = this.isEditInfo(editGeneralInfo);
      const isEditMatchInfo = this.isEditInfo(editMatchInfo);
      const isEditTradingInfo = this.isEditInfo(editTradingInfo);
      const isEditProviderInfo = this.isEditInfo(editProviderInfo);

      let canReload = true;
      if (
        isEditGeneralInfo ||
        isEditMatchInfo ||
        isEditTradingInfo ||
        isEditProviderInfo
      ) {
        canReload = false;
      }

      if (canReload) {
        location.reload();
      }
    }
  };

  isEditInfo(info) {
    return info && info === '1';
  }

  render() {
    const {
      isLoading,
      matchId,
      matchInfo,
      notePolicy,
      policy,
      noOfHDPOptions,
      matchOfferOptions
    } = this.state;
    if (isLoading) {
      return <Loading />;
    } else {
      if (matchInfo) {
        return (
          <div className='live-detail animated fadeIn'>
            <div className='left-info'>
              <GeneralInfo
                policy={policy}
                data={matchInfo}
                onSave={this.onSaveGeneralInfo}
                onEdit={this.onEditGeneralInfo}
                onCancel={this.onCancelGeneralInfo}
              />
              <div className='odd-maxbet'>
                {policy.HasRead && (
                  <div className='recommend-odds'>
                    <OddPanel
                      policy={policy}
                      matchId={matchId}
                      onSave={this.onSave}
                    />
                  </div>
                )}
                <div className='match-setting'>
                  <MatchSettingPanel
                    policy={policy}
                    matchId={matchId}
                    onSave={this.onSave}
                    minorBettypeId={matchInfo.MinorBettypeId}
                    data={matchInfo}
                  />
                </div>
              </div>
              {notePolicy.HasRead && (
                <NoteContext.Provider value={{ onUpdateNotes: this.onSave }}>
                  <Notes policy={notePolicy} matchId={matchId} />
                </NoteContext.Provider>
              )}
              <AssignmentContext.Provider
                value={{ onUpdateAssignments: this.onSave }}>
                <LiveAssignment
                  policy={policy}
                  matchId={matchInfo.ParentMatchId || matchId}
                />
              </AssignmentContext.Provider>
            </div>
            <aside className='information'>
              <MatchInfo
                policy={policy}
                data={matchInfo}
                onSave={this.onSaveMatchInfo}
                onEdit={this.onEditMatchInfo}
                onCancel={this.onCancelEditMatchInfo}
              />
              <FormTradingInfo
                policy={policy}
                data={matchInfo}
                noOfHDPOptions={noOfHDPOptions}
                matchOfferOptions={matchOfferOptions}
                onEdit={this.onEditTradingInfo}
                onCancel={this.onCancelTradingInfo}
                onSave={this.onSaveTradingInfo}
              />
              <FormProviderInfo
                ref={this.formProviderInfo}
                policy={policy}
                data={matchInfo}
                onEdit={this.onEditProviderInfo}
                onCancel={this.onCancelProviderInfo}
                onSave={this.onSaveProviderInfo}
              />
            </aside>
          </div>
        );
      } else {
        return (
          <div className='message-error'>
            <CloseCircleIcon size={25} />
            This match is not existed or you do not have permission
          </div>
        );
      }
    }
  }
}

LiveDetail.propTypes = propTypes;
export default LiveDetail;
