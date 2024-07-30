import { Component } from 'react';
import Select from 'react-select';
import { Input } from 'reactstrap';
import ArrowDownBoldIcon from 'mdi-react/ArrowDownBoldIcon';
import PropTypes from 'prop-types';
import {
  MatchStatuses,
  TimePickerDisplayFormat,
  TimePickerOutputFormat
} from '../../Shared/Constants';
import { getSpecialEventTemplate } from '../helper/apiHelper';

import { TimePicker } from 'antd';
import dayjs from 'dayjs';
import { assign, some, cloneDeep, isNil, remove, includes } from 'lodash';
import NumericTextbox from 'react-numeric-textbox';

const propTypes = {
  matchStatuses: PropTypes.array,
  bettypeData: PropTypes.array,
  isValidating: PropTypes.bool,
  parentEvent: PropTypes.object,
  getInfoSiteMatch: PropTypes.func
};

const inputFieldClass = 'input-field';
const defaultModel = {
  MatchStatus: null,
  SpecialEvent: null,
  MatchNo: '',
  InfoSiteId: null,
  KOTime: null,
  OtherTime: null,
  LeagueName: '',
  HomeName: '',
  AwayName: '',
  Division: '',
  Remark: '',
  Round: '',
  Website1: '',
  Website2: '',
  Website3: '',
  Website4: ''
};

class FormMatchInfo extends Component {
  constructor(props) {
    super(props);
    defaultModel.KOTime = null;
    defaultModel.OtherTime = null;
    defaultModel.Remark = '';
    defaultModel.MatchNo = '';
    defaultModel.Round = '';
    defaultModel.Division = '';
    defaultModel.LeagueName = '';
    defaultModel.HomeName = '';
    defaultModel.AwayName = '';
    defaultModel.Website1 = '';
    defaultModel.Website2 = '';
    defaultModel.Website3 = '';
    defaultModel.Website4 = '';
    if (props.parentEvent) {
      defaultModel.KOTime = dayjs(
        props.parentEvent.KickOffTime,
        TimePickerDisplayFormat
      ).format(TimePickerOutputFormat);
      defaultModel.OtherTime = props.parentEvent.OtherTime
        ? dayjs(props.parentEvent.OtherTime, TimePickerDisplayFormat).format(
            TimePickerOutputFormat
          )
        : '';
      defaultModel.MatchNo = props.parentEvent.MatchNo;
      defaultModel.Remark = props.parentEvent.Remark;
      defaultModel.Round = props.parentEvent.Round;
      defaultModel.Division = props.parentEvent.Division;
      defaultModel.LeagueName = props.parentEvent.League;
      defaultModel.HomeName = props.parentEvent.Home;
      defaultModel.AwayName = props.parentEvent.Away;
      defaultModel.Website1 = props.parentEvent.Website1;
      defaultModel.Website2 = props.parentEvent.Website2;
      defaultModel.Website3 = props.parentEvent.Website3;
      defaultModel.Website4 = props.parentEvent.Website4;
    }

    this.state = {
      model: cloneDeep(defaultModel)
    };
    this.getSpecialEventTemplate = this.getSpecialEventTemplate.bind(this);
    this.updateModel = this.updateModel.bind(this);
    this.handleMatchStatusChange = this.handleMatchStatusChange.bind(this);
    this.handleSpecialEventChange = this.handleSpecialEventChange.bind(this);
    this.getMatchStatus = this.getMatchStatus.bind(this);
    this.checkParentMatchStatus = this.checkParentMatchStatus.bind(this);
  }

  componentDidMount = () => {
    const { model } = this.state;
    model.MatchStatus = this.getMatchStatus();
    this.setState({ model });
  };

  checkParentMatchStatus = () => {
    const { parentEvent } = this.props;
    return (
      parentEvent &&
      includes(
        [
          MatchStatuses.Cancel,
          MatchStatuses.Suspended,
          MatchStatuses.Postponed
        ],
        parentEvent.MatchStatus
      )
    );
  };

  getMatchStatus = () => {
    const { matchStatuses, parentEvent } = this.props;
    let matchStatus;
    if (this.checkParentMatchStatus()) {
      matchStatus = matchStatuses
        .filter(item => item.Value === parentEvent.MatchStatus)
        .map(item => ({
          value: item.Id,
          label: item.Value
        }))[0];
    } else {
      matchStatus = matchStatuses
        .filter(item => item.Value === MatchStatuses.Live)
        .map(item => ({
          value: item.Id,
          label: item.Value
        }))[0];
    }
    return matchStatus;
  };

  reset = specialEvent => {
    const { bettypeData } = this.props;
    if (specialEvent) {
      remove(bettypeData, item => item.label === specialEvent);
    }
    this.setState({
      model: {
        ...cloneDeep(defaultModel),
        MatchStatus: this.getMatchStatus()
      }
    });
  };

  validateAndGetData = () => {
    const { model } = this.state;
    const mandatoryFields = ['KOTime', 'LeagueName', 'HomeName', 'AwayName'];
    if (this.props.parentEvent) {
      mandatoryFields.push('SpecialEvent');
    }

    const hasInvalidField = some(
      mandatoryFields,
      field => model[field] === null || model[field] === ''
    );
    if (hasInvalidField) {
      return {
        code: 1,
        message: 'Please input required fields'
      };
    }
    return {
      code: 0,
      data: {
        ...model,
        SpecialEvent: model.SpecialEvent ? model.SpecialEvent.label : null,
        MatchStatus: model.MatchStatus.label
      }
    };
  };

  handleMatchStatusChange = e => {
    const { model } = this.state;
    model.MatchStatus = e;
    this.setState({
      model
    });
  };

  handleSpecialEventChange = e => {
    const { model } = this.state;
    model.SpecialEvent = e;
    this.setState({
      model
    });
    this.getSpecialEventTemplate(e.value);
  };

  updateModel = e => {
    const { name, value } = e.target;
    const { model } = this.state;
    model[name] = value;
    this.setState({
      model
    });
  };

  setInfoSiteMatchData = dataModel => {
    const { model } = this.state;
    assign(model, dataModel);
    this.setState({ model });
  };

  onClickGetInfoSiteMatch = async () => {
    const { model } = this.state;
    const { getInfoSiteMatch } = this.props;
    getInfoSiteMatch(model.InfoSiteId);
  };

  getSpecialEventTemplate = async minorBettypeId => {
    const { parentEvent } = this.props;
    const { model } = this.state;
    const specialEventTemplateData = await getSpecialEventTemplate(
      parentEvent.OWPMatchId,
      minorBettypeId
    );
    assign(model, specialEventTemplateData);
    this.setState({ model });
  };

  renderSpecialEvent = (model, invalidInputClass) => {
    const { bettypeData, isValidating } = this.props;
    return (
      <div className='info-item required'>
        <div className='label'>Special Event</div>
        <div className='content'>
          <Select
            name='SpecialEvent'
            value={model.SpecialEvent}
            options={bettypeData}
            className={
              'basic-multi-select' +
              (isValidating && model.SpecialEvent === null
                ? invalidInputClass
                : '')
            }
            classNamePrefix='select'
            onChange={this.handleSpecialEventChange}
          />
        </div>
      </div>
    );
  };

  renderMatchStatus = (model, parentEvent) => {
    const { matchStatuses } = this.props;
    return (
      <div className='info-item'>
        <div className='label'>Match Status</div>
        <div className='content'>
          <Select
            name='MatchStatus'
            value={model.MatchStatus}
            onChange={this.handleMatchStatusChange}
            options={matchStatuses
              .filter(
                item =>
                  item.Value !== MatchStatuses.InPlay &&
                  item.Value !== MatchStatuses.Completed
              )
              .map(item => ({
                value: item.Id,
                label: item.Value
              }))}
            className='basic-multi-select'
            classNamePrefix='select'
            isDisabled={this.checkParentMatchStatus(parentEvent)}
          />
        </div>
      </div>
    );
  };

  renderMatchNo = model => {
    return (
      <div className='info-item'>
        <div className='label'>Match No.</div>
        <div className='content'>
          <Input
            type='text'
            name='MatchNo'
            className={inputFieldClass}
            value={model.MatchNo}
            onChange={this.updateModel}
            maxLength='50'
          />
        </div>
      </div>
    );
  };

  updateKOTime = time => {
    this.updateModel({
      target: {
        name: 'KOTime',
        value: time ? time.format(TimePickerOutputFormat) : ''
      }
    });
  };

  updateOtherTime = time => {
    this.updateModel({
      target: {
        name: 'OtherTime',
        value: time ? time.format(TimePickerOutputFormat) : ''
      }
    });
  };

  renderInfoSiteId = (model, parentEvent) => {
    const { getInfoSiteMatch } = this.props;
    return (
      <div className='info-item infosite-id'>
        <div className='label'>InfoSiteID</div>
        <div className='content'>
          <NumericTextbox
            type='number'
            name='InfoSiteId'
            className={`${inputFieldClass} form-control`}
            format='0'
            value={model.InfoSiteId}
            onChange={e =>
              this.updateModel({ target: { name: 'InfoSiteId', value: e } })
            }
            min={0}
            max={9999999999}
            autoCorrect
            disabled={!isNil(parentEvent)}
          />
          <button
            className='icon-getInfo'
            onClick={() => getInfoSiteMatch(model.InfoSiteId)}
            disabled={!model.InfoSiteId || parentEvent}>
            <ArrowDownBoldIcon size={12} />
          </button>
        </div>
      </div>
    );
  };

  renderKoTime = (model, parentEvent) => {
    const invalidInputClass = ' input-invalid';
    const { isValidating } = this.props;
    const inputKoTime = 'input-KO-time ';
    return (
      <div className='info-item required'>
        <div className='label'>KO Time</div>
        <div className='content'>
          {parentEvent && (
            <Input
              type='text'
              name='KOTime'
              className={
                inputKoTime +
                inputFieldClass +
                (isValidating && model.KOTime === '' ? invalidInputClass : '')
              }
              value={
                model.KOTime
                  ? dayjs(model.KOTime, TimePickerOutputFormat).format(
                      TimePickerDisplayFormat
                    )
                  : ''
              }
              readOnly={!isNil(parentEvent)}
            />
          )}
          {!parentEvent && (
            <TimePicker
              use12Hours
              format={TimePickerDisplayFormat}
              name='KOTime'
              id='KOTime'
              getPopupContainer={trigger => trigger.parentNode}
              className={
                inputFieldClass +
                (isValidating && model.KOTime === '' ? invalidInputClass : '')
              }
              onSelect={time => this.updateKOTime(time)}
              onChange={time => this.updateKOTime(time)}
              defaultValue={dayjs('00:00', TimePickerDisplayFormat)}
              value={
                model.KOTime
                  ? dayjs(model.KOTime, TimePickerOutputFormat)
                  : null
              }
              showNow={false}
            />
          )}
        </div>
      </div>
    );
  };

  renderWeb = (valueInput, webName, inputWebName) => {
    return (
      <div className='info-item'>
        <div className='label'>{webName}</div>
        <div className='content'>
          <Input
            type='text'
            name={inputWebName}
            className={inputFieldClass}
            value={valueInput || ''}
            onChange={this.updateModel}
            maxLength='1000'
          />
        </div>
      </div>
    );
  };

  renderInputCanReadOnlyField = (
    value,
    className,
    title,
    readOnly,
    inputName
  ) => {
    return (
      <div className='info-item required'>
        <div className='label'>{title}</div>
        <div className='content'>
          <Input
            type='text'
            name={inputName}
            className={className}
            value={value}
            readOnly={readOnly}
            onChange={this.updateModel}
            maxLength='500'
            autoComplete='off'
          />
        </div>
      </div>
    );
  };

  renderInputField = (value, title, inputName) => {
    return (
      <div className='info-item'>
        <div className='label'>{title}</div>
        <div className='content'>
          <Input
            type='text'
            name={inputName}
            className={inputFieldClass}
            value={value || ''}
            onChange={this.updateModel}
            maxLength='2000'
          />
        </div>
      </div>
    );
  };

  renderTimePicker = otherTime => {
    return (
      <div className='info-item'>
        <div className='label'>Other Time</div>
        <div className='content'>
          <TimePicker
            use12Hours
            format={TimePickerDisplayFormat}
            name='OtherTime'
            id='OtherTime'
            getPopupContainer={trigger => trigger.parentNode}
            className={inputFieldClass}
            onSelect={time => this.updateOtherTime(time)}
            onChange={time => this.updateOtherTime(time)}
            defaultValue={dayjs('00:00', TimePickerDisplayFormat)}
            value={otherTime ? dayjs(otherTime, TimePickerOutputFormat) : null}
            showNow={false}
          />
        </div>
      </div>
    );
  };

  render() {
    const { parentEvent, isValidating } = this.props;
    const { model } = this.state;
    const invalidInputClass = ' input-invalid';
    const inputLeagueName = 'input-league-name ';
    const inputHomeName = 'input-home-name ';
    const inputAwayName = 'input-away-name ';
    return (
      <div className='card-item'>
        <div className='header'>Match Info</div>
        <div className='content form-vertical'>
          {parentEvent && this.renderSpecialEvent(model, invalidInputClass)}
          {this.renderMatchStatus(model, parentEvent)}
          <div className='row'>
            <div className='col-6'>{this.renderMatchNo(model)}</div>
            <div className='col-6 last'>
              {!parentEvent && this.renderInfoSiteId(model, parentEvent)}
            </div>
          </div>
          <div className='row'>
            <div className='col-6'>{this.renderKoTime(model, parentEvent)}</div>
            <div className='col-6 last'>
              {this.renderTimePicker(model.OtherTime)}
            </div>
          </div>
          {this.renderInputCanReadOnlyField(
            model.LeagueName,
            inputLeagueName +
              inputFieldClass +
              (isValidating && model.LeagueName === ''
                ? invalidInputClass
                : ''),
            'League',
            !isNil(parentEvent),
            'LeagueName'
          )}
          {this.renderInputCanReadOnlyField(
            model.HomeName,
            inputHomeName +
              inputFieldClass +
              (isValidating && model.HomeName === '' ? invalidInputClass : ''),
            'Home Team',
            !isNil(parentEvent),
            'HomeName'
          )}
          {this.renderInputCanReadOnlyField(
            model.AwayName,
            inputAwayName +
              inputFieldClass +
              (isValidating && model.AwayName === '' ? invalidInputClass : ''),
            'Away Team',
            !isNil(parentEvent),
            'AwayName'
          )}
          {this.renderInputField(model.Remark, 'Remark', 'Remark')}
          {this.renderInputField(model.Round, 'Round', 'Round')}
          {this.renderInputField(model.Division, 'Division', 'Division')}
          {this.renderWeb(model.Website1, 'Web1', 'Website1')}
          {this.renderWeb(model.Website2, 'Web2', 'Website2')}
          {this.renderWeb(model.Website3, 'Web3', 'Website3')}
          {this.renderWeb(model.Website4, 'Web4', 'Website4')}
        </div>
      </div>
    );
  }
}

FormMatchInfo.propTypes = propTypes;
export default FormMatchInfo;
