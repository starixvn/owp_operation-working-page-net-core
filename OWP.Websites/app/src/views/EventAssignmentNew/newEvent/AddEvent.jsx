import { Component, createRef } from 'react';
import axios from 'axios';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import FormMatchInfo from './FormMatchInfo';
import FormProviderInfo from './FormProviderInfo';
import FormTradingInfo from './FormTradingInfo';
import FormRecommendOdds from './FormRecommendOdds';
import PropTypes from 'prop-types';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import Utils from '../../Shared/Utils';
import Spinner from '../../components/Spinner';
import dayjs from 'dayjs';
import {
  addNew,
  getBettypeForMatchInfo,
  getBettypeForTraderInfo,
  getByRefName,
  getInfoSiteMatch,
  queryMatchInfo
} from '../helper/apiHelper';
import ToastrWrapper from '../../components/ToastrWrapper';

const Toastr = withReactContent(Swal);
let matchId = null;
const showError = message => {
  Toastr.fire({
    icon: 'error',
    html: message,
    allowOutsideClick: false,
    allowEnterKey: false
  });
};

const propTypes = {
  matchStatuses: PropTypes.array.isRequired,
  selectedDate: PropTypes.object.isRequired,
  parentEvent: PropTypes.object,
  onSaved: PropTypes.func.isRequired,
  onToggleModal: PropTypes.func.isRequired,
  oddsPolicy: PropTypes.object.isRequired
};

let parentMatchInfo = null;

class AddEvent extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isLoading: true,
      isValidating: false,
      televisions: [],
      providers: [],
      otherProviders: [],
      agents: [],
      matchOffers: [],
      noOfHDPs: [],
      bettypesForTraderInfo: [],
      bettypeData: null
    };
    this.recommendOddsRef = createRef();
    this.matchSettingsRef = createRef();
    this.onAddEvent = this.onAddEvent.bind(this);
    this.onAddEventAndContinue = this.onAddEventAndContinue.bind(this);
    this.formMatchInfo = createRef();
    this.formProviderInfo = createRef();
    this.getInfoSiteMatch = this.getInfoSiteMatch.bind(this);
    this.confirmClose = this.confirmClose.bind(this);
  }

  async addEvent(matchInfo, callback) {
    Spinner.show();
    const parentEvent = this.state.parentEvent;
    if (parentEvent) {
      matchInfo.parentMatchId = parentEvent.OWPMatchId;
    }

    const result = await addNew(matchInfo);
    Spinner.hide();
    if (result.IsSuccess) {
      matchId = result.Data;
      await ToastrWrapper.showInfo(result.Message);
      callback(matchId);
    } else {
      const existedMatchId = result.Data.ExistedMatchId;
      if (existedMatchId) {
        const confirmResult = await Toastr.fire({
          icon: 'warning',
          text: 'This match is existing. Do you want to go to this match?',
          showCancelButton: true
        });
        if (confirmResult.value) {
          this.props.onSaved(existedMatchId);
        }
      } else {
        Toastr.fire({
          icon: 'error',
          text: result.Message
        });
      }
    }
  }

  validateAndGetMatchInfo() {
    const { selectedDate } = this.props;
    this.setState({ isValidating: true });
    const matchInfoValidateResult =
      this.formMatchInfo.current.validateAndGetData();
    if (matchInfoValidateResult.code) {
      showError(matchInfoValidateResult.message);
      return null;
    }
    let matchInfo = matchInfoValidateResult.data;

    const { oddsPolicy } = this.props;
    if (oddsPolicy.HasRead) {
      const recommendOddsValidateResult =
        this.recommendOddsRef.current.validateAndGetData();
      if (recommendOddsValidateResult.code) {
        showError(recommendOddsValidateResult.message);
        return null;
      }
      matchInfo = {
        ...matchInfo,
        ...recommendOddsValidateResult.data
      };
    } else {
      matchInfo = {
        ...matchInfo,
        MatchOdds: []
      };
    }

    const matchSettingsValidateResult =
      this.matchSettingsRef.current.validateAndGetData();
    if (matchSettingsValidateResult.code) {
      showError(matchSettingsValidateResult.message);
      return null;
    }

    matchInfo = {
      ...matchInfo,
      ...matchSettingsValidateResult.data
    };

    const providerInfoValidateResult =
      this.formProviderInfo.current.validateAndGetData();

    matchInfo = {
      ...matchInfo,
      ...providerInfoValidateResult.data
    };

    matchInfo.EventDate = Utils.formatDate(selectedDate);
    return matchInfo;
  }

  async onAddEvent() {
    const { onSaved } = this.props;
    const matchInfo = this.validateAndGetMatchInfo();
    if (matchInfo) {
      await this.addEvent(matchInfo, onSaved);
    }
  }

  async onAddEventAndContinue() {
    const matchInfo = this.validateAndGetMatchInfo();
    const { oddsPolicy } = this.props;

    if (matchInfo) {
      await this.addEvent(matchInfo, () => {
        this.setState({ isValidating: false, homeName: '', awayName: '' });
        this.formMatchInfo.current.reset(matchInfo.SpecialEvent);
        this.formProviderInfo.current.reset();
        oddsPolicy.HasRead && this.recommendOddsRef.current.reset();
        this.matchSettingsRef.current.reset();
      });
    }
  }

  getInfoSiteMatch = async InfoSiteId => {
    const postModel = {
      InfositeMatchId: InfoSiteId,
      EventDate: this.props.selectedDate
    };

    const response = await getInfoSiteMatch(postModel);
    if (response.Code > 0) {
      showError(response.Message);
      if (response.Code === 2) {
        this.formMatchInfo.current.setInfoSiteMatchData({ InfoSiteId: null });
        this.formProviderInfo.current.setInfoSiteMatchData({
          InfoSiteId: null
        });
      }
      return;
    }
    const modelDataSource = response.Data;
    this.formMatchInfo.current.setInfoSiteMatchData(modelDataSource);
    this.formProviderInfo.current.setInfoSiteMatchData(modelDataSource);
  };

  componentDidMount() {
    (async () => {
      const { parentEvent } = this.props;
      let bettypeData = [];
      Spinner.show();
      const [matchOffers, noOfHDPs, bettypesForTraderInfo] = await axios.all([
        getByRefName('MatchOffer'),
        getByRefName('NoOfHDP'),
        getBettypeForTraderInfo()
      ]);
      if (parentEvent) {
        bettypeData = await getBettypeForMatchInfo(parentEvent.OWPMatchId);
        parentMatchInfo = await queryMatchInfo(parentEvent.OWPMatchId);
      } else {
        parentMatchInfo = null;
      }
      Spinner.hide();
      this.setState({
        isLoading: false,
        matchOffers,
        noOfHDPs,
        bettypesForTraderInfo,
        bettypeData: bettypeData.map(item => ({
          value: item.MinorBettypeId,
          label: item.MinorBettypeName
        }))
      });
    })();
  }

  async confirmClose() {
    const result = await Toastr.fire({
      icon: 'question',
      text: 'Do you want to close ?',
      showCloseButton: true,
      showCancelButton: true
    });
    if (result.value) {
      matchId ? this.props.onSaved(matchId) : this.props.onToggleModal();
    }
  }

  render() {
    const { matchStatuses, oddsPolicy, parentEvent, selectedDate } = this.props;
    const {
      isLoading,
      matchOffers,
      noOfHDPs,
      bettypesForTraderInfo,
      bettypeData,
      isValidating,
      infoSiteMatchData
    } = this.state;

    return (
      <Modal tag='div' size='lg' isOpen className='modal-dark-title'>
        <ModalHeader toggle={this.confirmClose}>
          {(parentEvent ? 'Add Special Event' : 'Add Event') +
            ` (${dayjs(selectedDate).format('MMM DD, YYYY')})`}
        </ModalHeader>
        <ModalBody tag='div'>
          {!isLoading && (
            <div className='masonry'>
              <div className='flex-item'>
                <div className='masonry-item'>
                  <FormMatchInfo
                    ref={this.formMatchInfo}
                    matchStatuses={matchStatuses}
                    bettypeData={bettypeData}
                    parentEvent={parentEvent}
                    isValidating={isValidating}
                    getInfoSiteMatch={this.getInfoSiteMatch}
                    infoSiteMatchData={infoSiteMatchData}
                  />
                </div>
                {oddsPolicy.HasRead && (
                  <div className='masonry-item'>
                    <FormRecommendOdds
                      policy={oddsPolicy}
                      ref={this.recommendOddsRef}
                    />
                  </div>
                )}
              </div>
              <div className='flex-item'>
                <div className='masonry-item'>
                  <FormProviderInfo
                    ref={this.formProviderInfo}
                    parentMatchInfo={parentMatchInfo}
                    infoSiteMatchData={infoSiteMatchData}
                  />
                </div>
                <div className='masonry-item'>
                  <FormTradingInfo
                    matchOffers={matchOffers}
                    noOfHDPs={noOfHDPs}
                    bettypes={bettypesForTraderInfo}
                    ref={this.matchSettingsRef}
                  />
                </div>
              </div>
            </div>
          )}
        </ModalBody>
        {!isLoading && (
          <ModalFooter>
            <Button
              color='primary'
              className='btn-add'
              onClick={this.onAddEvent}>
              Add{' '}
            </Button>
            <Button
              color='primary'
              className='btn-add'
              onClick={this.onAddEventAndContinue}>
              Add & Continue{' '}
            </Button>
            <Button
              color='secondary'
              className='btn-cancel'
              onClick={this.confirmClose}>
              Cancel{' '}
            </Button>
          </ModalFooter>
        )}
      </Modal>
    );
  }
}

AddEvent.propTypes = propTypes;
export default AddEvent;
