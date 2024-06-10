import { Component } from 'react';
import PropTypes from 'prop-types';
import { cloneDeep, isNil, isEqual } from 'lodash';
import dayjs from 'dayjs';
import { Input } from 'reactstrap';
import { TimePicker } from 'antd';
import {
  TimePickerDisplayFormat,
  TimePickerOutputFormat
} from '../../Shared/Constants';
import EditableButtonGroup from '../EditableButtonGroup';
import Service from './Service';
import Toastr from '../../components/ToastrWrapper';
import Spinner from '../../components/Spinner';
import FlagWrapper from './FlagComponent/FlagWrapper';
import { getDayjsFromTime } from '../../Shared/DateTime/dateTimeHelper';

const inputInvalid = ' input-invalid';
const inputField = 'input-field';
const propTypes = {
  policy: PropTypes.object.isRequired,
  data: PropTypes.object.isRequired,
  onSave: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
  onEdit: PropTypes.func.isRequired
};

const isModelHaveValue = model => {
  return (
    model.KOTime !== '' &&
    model.LeagueName !== '' &&
    model.HomeName !== '' &&
    model.AwayName !== ''
  );
};

class GeneralInfo extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isEditing: false,
      isExpandHeader: false,
      model: this.cloneModel(),
      isNeutral: this.props.data.HomeName.endsWith(' (N)')
    };
    this.updateModel = this.updateModel.bind(this);
    this.expandGeneralInfoArea = this.expandGeneralInfoArea.bind(this);
  }

  cloneModel = () => {
    const model = cloneDeep(this.props.data);
    model.KOTime = getDayjsFromTime(model.KickOffTimeDisplay).format(TimePickerOutputFormat);
    return model;
  };

  onCancel = () => {
    this.setState({
      isEditing: false,
      model: this.cloneModel()
    });
    this.props.onCancel(true);
  };

  onSave = async () => {
    const { model, isNeutral } = this.state;
    const {
      KOTime,
      MatchNo,
      LeagueName,
      HomeName,
      AwayName,
      OWPMatchId
    } = model;
    const validateModel = {
      OWPMatchId,
      KOTime,
      MatchNo: MatchNo.trim(),
      LeagueName: LeagueName.trim(),
      HomeName: isNeutral
        ? HomeName.substring(0, HomeName.lastIndexOf(' (N)')).trim()
        : HomeName.trim(),
      AwayName: AwayName.trim()
    };

    if (!isModelHaveValue(validateModel)) {
      Toastr.showError('Please input required fields');
      return;
    }

    const validateResult = await Service.validateToEditEvent(validateModel);
    if (!validateResult.IsSuccess) {
      Toastr.showError(validateResult.Message);
      return;
    }

    const resultData = validateResult.Data;
    const confirmResult = await this.showConfirmMessage(model, resultData);
    if (confirmResult.value) {
      Spinner.show();
      const updateResult = await Service.editEvent(validateModel);
      Spinner.hide();
      await this.showToastAfterUpdate(updateResult);
    }
  };

  onEdit = () => {
    this.setState({
      isEditing: true
    });
    this.props.onEdit(true);
  };

  updateModel = e => {
    const { name, value } = e.target;
    const { model, isNeutral } = this.state;
    model[name] =
      isNeutral && isEqual(name, 'HomeName') ? `${value} (N)` : value;
    this.setState({
      model
    });
  };

  async showToastAfterUpdate(updateResult) {
    if (updateResult.IsSuccess) {
      await Toastr.showInfo(updateResult.Message);
      this.setState({
        isEditing: false
      });
      this.props.onSave();
    } else {
      await Toastr.showError(updateResult.Message);
    }
  }

  async showConfirmMessage(model, resultData) {
    let confirmMessage = 'Are you sure to save the changes ?';

    if (model.ParentMatchId === null && resultData.ParentMatchId !== null) {
      confirmMessage = (
        <div className='text-left'>
          <strong className='title'>
            {' '}
            You are updating a main match to special event? Please carefully be
            aware the attention:
          </strong>
          <ul className='list-warning'>
            <li>Recommend odds shall be update properly.</li>
            <li>
              Maxbet/Spread: system shall delete the redundant setting of bet
              types.
            </li>
            <li>
              Match status shall be changed properly, InfoSiteID shall be
              remove.
            </li>
            <li>
              Assignment of this match shall be removed redundant bet types.
              PLEASE BE AWARE!
            </li>
          </ul>
        </div>
      );
    }

    const confirmResult = await Toastr.confirm(confirmMessage);
    return confirmResult;
  }

  expandGeneralInfoArea(isExpandHeader) {
    this.setState({
      isExpandHeader: !isExpandHeader
    });
  }

  render() {
    const { policy, data } = this.props;
    const { model, isEditing, isNeutral, isExpandHeader } = this.state;
    const { HomeName, AwayName, LeagueName } = model;
    const homeName = this.getHomeName(isNeutral, HomeName);
    const isEdit = isEditing && isNil(model.ParentMatchId);

    const collapseExpand = isExpandHeader
      ? 'general-info is-collapse'
      : 'general-info';

    return (
      <div className={collapseExpand}>
        <div className='info-block'>
          <div>
            Event Date:
            <span className='daytime'>{model.EventDateDisplay}</span>
          </div>
          <div />
          {this.renderLeague(isEditing, isEdit, model, data)}
          {this.renderEvent(isEdit, model, isNeutral)}
        </div>
        <FlagWrapper
          isExpand={isExpandHeader}
          teamName={{ homeName, AwayName }}
          leagueName={LeagueName}
          onChangeGeneralArea={() => this.expandGeneralInfoArea(isExpandHeader)}
        />
        {policy.HasWrite && (
          <EditableButtonGroup
            isEditing={isEditing}
            onCancel={this.onCancel}
            onSave={this.onSave}
            onEdit={this.onEdit}
          />
        )}
      </div>
    );
  }

  getDateTimePickerClassName(model) {
    return model.KOTime ? inputField : inputInvalid;
  }

  getDateTimePickerValue(model) {
    return model.KOTime ? dayjs(model.KOTime, TimePickerOutputFormat) : null;
  }

  getClassNameInput(model) {
    return `${inputField}${isEqual(model.HomeName.trim(), '') ? inputInvalid : ''
      }`;
  }

  updateKOTime = time => {
    this.updateModel({
      target: {
        name: 'KOTime',
        value: time ? time.format(TimePickerOutputFormat) : ''
      }
    });
  };

  getHomeName(isNeutral, HomeName) {
    return isNeutral
      ? HomeName.substring(0, HomeName.lastIndexOf(' (N)'))
      : HomeName;
  }

  renderLeague(isEditing, isEdit, model, data) {
    const { LeagueName } = model;
    const classNameInput = this.getClassNameInput(model);
    const dateTimePickerValue = this.getDateTimePickerValue(model);
    const dateTimePickerClassName = this.getDateTimePickerClassName(model);
    return (
      <div className='league'>
        {isEditing ? (
          <>
            <span>Match No. </span>
            <Input
              type='text'
              name='MatchNo'
              className='input-field match-no'
              value={model.MatchNo}
              onChange={this.updateModel}
              maxLength='50'
              autoComplete='off'
            />
          </>
        ) : (
          model.MatchNo && (
            <>
              Match No.
              <span className='number'>{model.MatchNo}</span>
            </>
          )
        )}
        <div className='KO-time'>
          K.O:
          {isEdit ? (
            <TimePicker
              use12Hours
              id='KOTime'
              format={TimePickerDisplayFormat}
              getPopupContainer={trigger => trigger.parentNode}
              className={dateTimePickerClassName}
              onSelect={time => this.updateKOTime(time)}
              onChange={time => this.updateKOTime(time)}
              value={dateTimePickerValue}
              showNow={false}
            />
          ) : (
            <span className='daytime'>
              {dayjs(model.KOTime, TimePickerOutputFormat).format(
                TimePickerDisplayFormat
              )}
            </span>
          )}
        </div>
        {data.OtherTime && (
          <span>
            Other Time:
            <span className='daytime'>
              {dayjs(data.OtherTime, TimePickerOutputFormat).format(
                TimePickerDisplayFormat
              )}
            </span>
          </span>
        )}
        {isEdit ? (
          <Input
            type='text'
            name='LeagueName'
            placeholder='League Name'
            className={classNameInput}
            value={LeagueName}
            readOnly={!isNil(model.ParentMatchId)}
            onChange={this.updateModel}
            maxLength='200'
            autoComplete='off'
          />
        ) : (
          LeagueName
        )}
      </div>
    );
  }

  renderEvent(isEdit, model, isNeutral) {
    const classNameInput = this.getClassNameInput(model);
    const { HomeName, AwayName } = model;
    const homeName = this.getHomeName(isNeutral, HomeName);
    return (
      <div className='event'>
        {isEdit ? (
          <span className='team'>
            <Input
              type='text'
              name='HomeName'
              placeholder='Home Team'
              className={classNameInput}
              value={homeName}
              onChange={this.updateModel}
              maxLength='200'
              autoComplete='off'
            />
            <span>-vs-</span>
            <Input
              type='text'
              name='AwayName'
              placeholder='Away Team'
              className={classNameInput}
              value={AwayName}
              onChange={this.updateModel}
              maxLength='200'
              autoComplete='off'
            />
          </span>
        ) : (
          <span className='team'>
            {HomeName} -vs- {AwayName}
          </span>
        )}
      </div>
    );
  }
}

GeneralInfo.propTypes = propTypes;
export default GeneralInfo;
