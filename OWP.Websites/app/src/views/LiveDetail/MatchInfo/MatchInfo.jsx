import { Component } from 'react';
import Select from 'react-select';
import PropTypes from 'prop-types';
import Animated from 'react-select/animated';
import { Input, UncontrolledTooltip } from 'reactstrap';
import dayjs from 'dayjs';
import MatchInfoService from './Service';
import NumericTextbox from 'react-numeric-textbox';
import { TimePicker } from 'antd';
import { cloneDeep, isNil, isEqual } from 'lodash';
import {
  TimePickerDisplayFormat,
  TimePickerOutputFormat,
  MatchStatuses
} from '../../Shared/Constants';
import LinkIcon from 'mdi-react/LinkIcon';
import LinkOffIcon from 'mdi-react/LinkOffIcon';
import EditableCollapsePanel from '../EditableCollapsePanel';
import Toastr from '../../components/ToastrWrapper';
import Spinner from '../../components/Spinner';
import { getDayjsFromTime } from '../../Shared/DateTime/dateTimeHelper';

const convertMatchStatusListToOptions = items => {
  return items.map(item => ({
    value: item.Value,
    label: item.DisplayText
  }));
};

const getMatchStatusSelected = items => {
  const found = items.find(item => item.IsSelected === true);
  return { value: found.Value, label: found.DisplayText };
};

const propTypes = {
  policy: PropTypes.object.isRequired,
  data: PropTypes.object.isRequired,
  onSave: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
  onEdit: PropTypes.func.isRequired
};

let defaultModel;

const checkboxHintString = 'Update for relevant matches';

class MatchInfo extends Component {
  constructor(props) {
    super(props);
    const { data } = this.props;

    const matchInfo = {
      ...data,
      OtherTime: data.OtherTime
        ? getDayjsFromTime(data.OtherTime).format(TimePickerOutputFormat)
        : null,
      SelectedMatchStatus: getMatchStatusSelected(data.MatchStatusList),
      Markets: data.Markets,
      IsUpdateRound: true,
      IsUpdateDivision: true,
      IsUpdateVenue: true,
      IsUpdateOtherTime: true,
      IsUpdateMarket: true,
      IsUpdateWebsite1: true,
      IsUpdateWebsite2: true,
      IsUpdateWebsite3: true,
      IsUpdateWebsite4: true
    };

    defaultModel = cloneDeep(matchInfo);

    this.state = {
      isLoading: true,
      isOpen: true,
      isEditing: false,
      isInfoSiteLinked: null,
      isLiveStatusSynchronized: null,
      matchInfo
    };

    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleMatchStatusSelectChange =
      this.handleMatchStatusSelectChange.bind(this);
    this.onSave = this.onSave.bind(this);
    this.onCancel = this.onCancel.bind(this);
    this.onEdit = this.onEdit.bind(this);
    this.createCheckbox = this.createCheckbox.bind(this);
    this.handleCheckboxChange = this.handleCheckboxChange.bind(this);
    this.getOtherTime = this.getOtherTime.bind(this);
    this.getValueOrDefault = this.getValueOrDefault.bind(this);
    this.onValidateInfoSiteMatchId = this.onValidateInfoSiteMatchId.bind(this);
    this.validateInfoSiteMatchId = this.validateInfoSiteMatchId.bind(this);
  }

  componentDidMount() {
    (async () => {
      const { matchInfo } = this.state;
      await this.validateInfoSiteMatchId(
        matchInfo.OWPMatchId,
        matchInfo.InfoSiteMatchId,
        {
          isLoading: false
        }
      );
    })();
  }

  handleInputChange = async event => {
    const { target } = event;
    const { value, name } = target;

    let matchInfo = {
      ...this.state.matchInfo
    };

    const isInfoSiteMatchId = isEqual(name, 'InfoSiteMatchId');

    if (isInfoSiteMatchId && (value > 0 || value === null)) {
      matchInfo = {
        ...this.state.matchInfo,
        [name]: value
      };
    }
    if (!isInfoSiteMatchId) {
      matchInfo = {
        ...this.state.matchInfo,
        [name]: value
      };
    }

    this.setState({
      matchInfo
    });
  };

  onValidateInfoSiteMatchId = async () => {
    const { matchInfo } = this.state;
    await this.validateInfoSiteMatchId(
      matchInfo.OWPMatchId,
      matchInfo.InfoSiteMatchId
    );
  };

  validateInfoSiteMatchId = async (owpMatchId, infoSiteMatchId, extraState) => {
    const isInfoSiteIdValid = await MatchInfoService.validateInfoSiteMatchId(
      owpMatchId,
      infoSiteMatchId
    );
    this.setState({
      isInfoSiteLinked: isInfoSiteIdValid.IsInfoSiteLinked,
      isLiveStatusSynchronized: isInfoSiteIdValid.IsLiveStatusSynchronized
    });
    this.setState(extraState);
  };

  handleMatchStatusSelectChange(event) {
    const newValue = event.value;
    this.setState({
      matchInfo: {
        ...this.state.matchInfo,
        SelectedMatchStatus: { label: newValue, value: newValue }
      }
    });
  }

  onSave = async () => {
    const { matchInfo } = this.state;
    const confirmMessage = 'Are you sure to save the changes ?';
    const confirmResult = await Toastr.confirm(confirmMessage);
    if (confirmResult.value) {
      matchInfo.MatchStatus =
        matchInfo.SelectedMatchStatus.label === MatchStatuses.InPlay ||
        matchInfo.SelectedMatchStatus.label === MatchStatuses.Completed
          ? MatchStatuses.Live
          : matchInfo.SelectedMatchStatus.label;

      Spinner.show();
      const updateResult = await MatchInfoService.updateMatchInfo(matchInfo);
      Spinner.hide();
      if (updateResult.IsSuccess) {
        const resultMessage = 'The event has been edited successfully!';
        await Toastr.showInfo(resultMessage);
        matchInfo.IsUpdateDivision = true;
        matchInfo.IsUpdateMarket = true;
        matchInfo.IsUpdateOtherTime = true;
        matchInfo.IsUpdateRound = true;
        matchInfo.IsUpdateVenue = true;
        matchInfo.IsUpdateWebsite1 = true;
        matchInfo.IsUpdateWebsite2 = true;
        matchInfo.IsUpdateWebsite3 = true;
        matchInfo.IsUpdateWebsite4 = true;
        defaultModel = cloneDeep(matchInfo);
        this.setState({
          isEditing: false
        });
        const { onSave } = this.props;
        onSave({
          OtherTime: matchInfo.OtherTime
        });
      } else {
        await Toastr.showError(updateResult.Message);
      }
    }
  };

  onCancel = async () => {
    await this.validateInfoSiteMatchId(
      defaultModel.OWPMatchId,
      defaultModel.InfoSiteMatchId,
      {
        matchInfo: defaultModel,
        isEditing: false
      }
    );
    this.props.onCancel(true);
  };

  onEdit = async () => {
    this.setState({ isEditing: true });
    this.props.onEdit(true);
  };

  handleCheckboxChange = event => {
    const target = event.target;
    const value = target.checked;
    const name = target.name;

    const matchInfo = {
      ...this.state.matchInfo,
      [name]: value
    };

    this.setState({
      matchInfo
    });
  };

  updateOtherTime = time => {
    this.handleInputChange({
      target: {
        name: 'OtherTime',
        value: time ? time.format(TimePickerOutputFormat) : ''
      }
    });
  };

  createCheckbox = (className, id) => {
    const labelId = `tooltip-${id}`;
    return (
      <div className='option-wrapper'>
        <Input
          disabled={!this.state.isEditing}
          className={className}
          type='checkbox'
          id={id}
          defaultChecked={this.state.matchInfo[id]}
          name={id}
          onChange={this.handleCheckboxChange}
        />
        <label id={labelId} htmlFor={id} />
        <UncontrolledTooltip placement='right' target={labelId}>
          {checkboxHintString}
        </UncontrolledTooltip>
      </div>
    );
  };

  getOtherTime = otherTime =>
    otherTime ? dayjs(otherTime, TimePickerOutputFormat) : null;

  getValueOrDefault = value => value ?? '';

  renderInfoSiteLinked() {
    if (this.isLiveStatusSynchronized()) {
      return <span className='match-status-live'>LIVE</span>;
    }

    return this.state.isInfoSiteLinked ? (
      <LinkIcon color='#00920A' size={15} />
    ) : (
      <LinkOffIcon color='#FF0000' size={15} />
    );
  }

  isLiveStatusSynchronized() {
    const { matchInfo, isLiveStatusSynchronized } = this.state;
    const matchStatus = matchInfo.SelectedMatchStatus.value;
    const isLive =
      isEqual(matchStatus, MatchStatuses.InPlay) ||
      isEqual(matchStatus, MatchStatuses.Live) ||
      isEqual(matchStatus, MatchStatuses.Postponed);
    return isLiveStatusSynchronized && isLive;
  }

  isParentLive = () => {
    const { data } = this.props;
    return data.ParentMatchId
      ? data.ParentMatchStatus === MatchStatuses.Live ||
          data.ParentMatchStatus === MatchStatuses.Completed ||
          data.ParentMatchStatus === MatchStatuses.InPlay
      : true;
  };

  renderInfoSiteID = (matchInfo, isEditing) => {
    return (
      <div className='info-item'>
        <span className='label'>InfoSiteID</span>
        <div className='content'>
          <NumericTextbox
            type='number'
            className='input-field form-control'
            format='0'
            value={matchInfo.InfoSiteMatchId}
            onChange={e =>
              this.handleInputChange({
                target: { name: 'InfoSiteMatchId', value: e }
              })
            }
            onBlur={this.onValidateInfoSiteMatchId}
            min={0}
            max={9999999999}
            placeholder={!matchInfo.ParentMatchId ? 'Add InfoSiteID' : ''}
            autoCorrect
            disabled={!isEditing || !isNil(matchInfo.ParentMatchId)}
          />
          {this.renderInfoSiteLinked()}
        </div>
      </div>
    );
  };

  renderAdminSiteID = matchInfo => {
    return (
      <div className='info-item'>
        <span className='label'>AdminSiteID</span>
        <div className='content'>
          <NumericTextbox
            type='number'
            className='input-field form-control'
            format='0'
            value={!matchInfo.ParentMatchId ? matchInfo.AdminSiteId : null}
            min={0}
            max={9999999999}
            disabled
          />
        </div>
      </div>
    );
  };

  renderRemark = (matchInfo, isEditing) => {
    return (
      <div className='info-item'>
        <span className='label'>Remark</span>
        <div className='content'>
          <Input
            className='input-field'
            id='Remark'
            type='text'
            name='Remark'
            value={matchInfo.Remark ? matchInfo.Remark : ''}
            onChange={event => this.handleInputChange(event)}
            maxLength='2000'
            readOnly={!isEditing}
            placeholder='Add Remark'
          />
          {matchInfo.Remark && !isEditing && (
            <UncontrolledTooltip placement='right' target='Remark'>
              {matchInfo.Remark}
            </UncontrolledTooltip>
          )}
        </div>
      </div>
    );
  };

  renderOtherTime = (matchInfo, isEditing, isHost, updateCheckboxClass) => {
    return (
      <div className='info-item'>
        <div className='label'>Other Time</div>
        <div className='content'>
          {isHost ? (
            <TimePicker
              use12Hours
              format={TimePickerDisplayFormat}
              getPopupContainer={trigger => trigger.parentNode}
              name='OtherTime'
              className='input-field'
              id='OtherTime'
              onSelect={time => this.updateOtherTime(time)}
              onChange={time => this.updateOtherTime(time)}
              value={this.getOtherTime(matchInfo.OtherTime)}
              defaultValue={dayjs('00:00', TimePickerDisplayFormat)}
              autoComplete='off'
              disabled={!isEditing}
              showNow={false}
            />
          ) : (
            <Input
              type='text'
              name='OtherTime'
              className='input-field'
              id='OtherTime'
              value={this.getOtherTime(matchInfo.OtherTime)?.format(
                TimePickerDisplayFormat
              )}
              readOnly
            />
          )}
          {isEditing &&
            isHost &&
            this.createCheckbox(updateCheckboxClass, 'IsUpdateOtherTime')}
        </div>
      </div>
    );
  };

  renderTeamRound = matchInfo => {
    return (
      <div className='info-item'>
        <span className='label'>Round (H-A/T)</span>
        <div className='content'>
          <span>{matchInfo.TeamRound}</span>
        </div>
      </div>
    );
  };

  renderRound = (matchInfo, isEditing, isHost, updateCheckboxClass) => {
    return (
      <div className='info-item'>
        <span className='label'>Round</span>
        <div className='content'>
          <Input
            className='input-field'
            id='Round'
            type='text'
            name='Round'
            placeholder='Add Round'
            value={matchInfo.Round ? matchInfo.Round : ''}
            onChange={event => this.handleInputChange(event)}
            maxLength='2000'
            readOnly={!isEditing}
          />
          {matchInfo.Round && !isEditing && (
            <UncontrolledTooltip placement='right' target='Round'>
              {matchInfo.Round}
            </UncontrolledTooltip>
          )}
          {isEditing &&
            isHost &&
            this.createCheckbox(updateCheckboxClass, 'IsUpdateRound')}
        </div>
      </div>
    );
  };

  renderDivision = (matchInfo, isEditing, isHost, updateCheckboxClass) => {
    return (
      <div className='info-item'>
        <span className='label'>Division</span>
        <div className='content'>
          <Input
            className='input-field'
            id='Division'
            type='text'
            name='Division'
            placeholder='Add Division'
            value={matchInfo.Division ? matchInfo.Division : ''}
            onChange={event => this.handleInputChange(event)}
            maxLength='2000'
            readOnly={!isEditing}
          />
          {matchInfo.Division && !isEditing && (
            <UncontrolledTooltip placement='right' target='Division'>
              {matchInfo.Division}
            </UncontrolledTooltip>
          )}
          {isEditing &&
            isHost &&
            this.createCheckbox(updateCheckboxClass, 'IsUpdateDivision')}
        </div>
      </div>
    );
  };

  renderVenue = (matchInfo, isEditing, isHost, updateCheckboxClass) => {
    return (
      <div className='info-item'>
        <span className='label'>Venue</span>
        <div className='content'>
          <Input
            className='input-field'
            id='Venue'
            type='text'
            name='Venue'
            placeholder='Add Venue'
            value={matchInfo.Venue ? matchInfo.Venue : ''}
            onChange={event => this.handleInputChange(event)}
            maxLength='100'
            readOnly={!isEditing}
          />
          {matchInfo.Venue && !isEditing && (
            <UncontrolledTooltip placement='right' target='Venue'>
              {matchInfo.Venue}
            </UncontrolledTooltip>
          )}
          {isEditing &&
            isHost &&
            this.createCheckbox(updateCheckboxClass, 'IsUpdateVenue')}
        </div>
      </div>
    );
  };

  renderMarket = (matchInfo, isEditing, isHost, updateCheckboxClass) => {
    return (
      <div className='info-item'>
        <span className='label'>Market</span>
        <div className='content'>
          <Input
            className='input-field'
            id='Market'
            type='text'
            name='Market'
            placeholder='Add Market'
            value={matchInfo.Market ? matchInfo.Market : ''}
            onChange={event => this.handleInputChange(event)}
            maxLength='250'
            readOnly={!isEditing}
          />
          {matchInfo.Market && !isEditing && (
            <UncontrolledTooltip placement='right' target='Market'>
              {matchInfo.Market}
            </UncontrolledTooltip>
          )}
          {isEditing &&
            isHost &&
            this.createCheckbox(updateCheckboxClass, 'IsUpdateMarket')}
        </div>
      </div>
    );
  };

  renderWeb1 = (matchInfo, isEditing, isHost, updateCheckboxClass) => {
    return (
      <div className='info-item'>
        <div className='label'>Web1</div>
        <div className='content weblink'>
          {isEditing || !matchInfo.Website1 ? (
            <Input
              type='text'
              name='Website1'
              id='Website1'
              className='input-field'
              value={this.getValueOrDefault(matchInfo.Website1)}
              onChange={event => this.handleInputChange(event)}
              readOnly={!isEditing}
              placeholder='Add Web'
              maxLength='1000'
            />
          ) : (
            <a
              id='Website1'
              href={matchInfo.Website1}
              target='_blank'
              rel='noopener noreferrer'>
              {matchInfo.Website1}
            </a>
          )}
          {matchInfo.Website1 && !isEditing && (
            <UncontrolledTooltip placement='right' target='Website1'>
              {matchInfo.Website1}
            </UncontrolledTooltip>
          )}
          {isEditing &&
            isHost &&
            this.createCheckbox(updateCheckboxClass, 'IsUpdateWebsite1')}
        </div>
      </div>
    );
  };

  renderWeb2 = (matchInfo, isEditing, isHost, updateCheckboxClass) => {
    return (
      <div className='info-item'>
        <div className='label'>Web2</div>
        <div className='content weblink'>
          {isEditing || !matchInfo.Website2 ? (
            <Input
              type='text'
              name='Website2'
              id='Website2'
              className='input-field'
              value={this.getValueOrDefault(matchInfo.Website2)}
              onChange={event => this.handleInputChange(event)}
              readOnly={!isEditing}
              placeholder='Add Web'
              maxLength='1000'
            />
          ) : (
            <a
              id='Website2'
              href={matchInfo.Website2}
              target='_blank'
              rel='noopener noreferrer'>
              {matchInfo.Website2}
            </a>
          )}
          {matchInfo.Website2 && !isEditing && (
            <UncontrolledTooltip placement='right' target='Website2'>
              {matchInfo.Website2}
            </UncontrolledTooltip>
          )}
          {isEditing &&
            isHost &&
            this.createCheckbox(updateCheckboxClass, 'IsUpdateWebsite2')}
        </div>
      </div>
    );
  };

  renderWeb3 = (matchInfo, isEditing, isHost, updateCheckboxClass) => {
    return (
      <div className='info-item'>
        <div className='label'>Web3</div>
        <div className='content weblink'>
          {isEditing || !matchInfo.Website3 ? (
            <Input
              type='text'
              name='Website3'
              id='Website3'
              className='input-field'
              value={this.getValueOrDefault(matchInfo.Website3)}
              onChange={event => this.handleInputChange(event)}
              readOnly={!isEditing}
              placeholder='Add Web'
              maxLength='1000'
            />
          ) : (
            <a
              id='Website3'
              href={matchInfo.Website3}
              target='_blank'
              rel='noopener noreferrer'>
              {matchInfo.Website3}
            </a>
          )}
          {matchInfo.Website3 && !isEditing && (
            <UncontrolledTooltip placement='right' target='Website3'>
              {matchInfo.Website3}
            </UncontrolledTooltip>
          )}
          {isEditing &&
            isHost &&
            this.createCheckbox(updateCheckboxClass, 'IsUpdateWebsite3')}
        </div>
      </div>
    );
  };

  renderWeb4 = (matchInfo, isEditing, isHost, updateCheckboxClass) => {
    return (
      <div className='info-item'>
        <div className='label'>Web4</div>
        <div className='content weblink'>
          {isEditing || !matchInfo.Website4 ? (
            <Input
              type='text'
              name='Website4'
              id='Website4'
              className='input-field'
              value={this.getValueOrDefault(matchInfo.Website4)}
              onChange={event => this.handleInputChange(event)}
              readOnly={!isEditing}
              placeholder='Add Web'
              maxLength='1000'
            />
          ) : (
            <a
              id='Website4'
              href={matchInfo.Website4}
              target='_blank'
              rel='noopener noreferrer'>
              {matchInfo.Website4}
            </a>
          )}
          {matchInfo.Website4 && !isEditing && (
            <UncontrolledTooltip placement='right' target='Website4'>
              {matchInfo.Website4}
            </UncontrolledTooltip>
          )}
          {isEditing &&
            isHost &&
            this.createCheckbox(updateCheckboxClass, 'IsUpdateWebsite4')}
        </div>
      </div>
    );
  };

  render() {
    const { isEditing, matchInfo } = this.state;
    const { data, policy } = this.props;
    const isHost = this.props.policy.HasWrite;
    const matchStatusOptions = convertMatchStatusListToOptions(
      data.MatchStatusList
    );

    const renderMatchStatus =
      isHost && this.isParentLive ? (
        <Select
          value={matchInfo.SelectedMatchStatus}
          components={Animated}
          name='Status'
          options={matchStatusOptions}
          className='basic-multi-select'
          classNamePrefix='select'
          onChange={this.handleMatchStatusSelectChange}
          isDisabled={!isEditing}
          isSearchable={false}
        />
      ) : (
        matchInfo.SelectedMatchStatus.label
      );
    const updateCheckboxClass = 'update-relevant-checkbox';

    return (
      <EditableCollapsePanel
        policy={policy}
        title='Match Info'
        onEdit={this.onEdit}
        onCancel={this.onCancel}
        onSave={this.onSave}
        isEditing={isEditing}>
        <div className='content form-vertical'>
          <div className='info-item'>
            <span className='label'>Match Status</span>
            <div className='content'>{renderMatchStatus}</div>
          </div>
          {this.renderInfoSiteID(matchInfo, isEditing)}
          {this.renderAdminSiteID(matchInfo)}
          {this.renderRemark(matchInfo, isEditing)}
          <div className='info-item'>
            <span className='label'>Match Solo</span>
            <div className='content js-soloMatch-wrapper'>
              <span>{matchInfo.IsSoloMatch ? 'Yes' : 'No'}</span>
            </div>
          </div>
          {this.renderOtherTime(
            matchInfo,
            isEditing,
            isHost,
            updateCheckboxClass
          )}
          {this.renderRound(matchInfo, isEditing, isHost, updateCheckboxClass)}
          {this.renderTeamRound(matchInfo)}
          {this.renderDivision(
            matchInfo,
            isEditing,
            isHost,
            updateCheckboxClass
          )}
          {this.renderVenue(matchInfo, isEditing, isHost, updateCheckboxClass)}
          {this.renderMarket(matchInfo, isEditing, isHost, updateCheckboxClass)}
          {this.renderWeb1(matchInfo, isEditing, isHost, updateCheckboxClass)}
          {this.renderWeb2(matchInfo, isEditing, isHost, updateCheckboxClass)}
          {this.renderWeb3(matchInfo, isEditing, isHost, updateCheckboxClass)}
          {this.renderWeb4(matchInfo, isEditing, isHost, updateCheckboxClass)}
        </div>
      </EditableCollapsePanel>
    );
  }
}

MatchInfo.propTypes = propTypes;
export default MatchInfo;
