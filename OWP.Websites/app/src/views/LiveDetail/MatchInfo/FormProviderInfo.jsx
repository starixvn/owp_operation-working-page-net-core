import { Component } from 'react';
import { Input, UncontrolledTooltip } from 'reactstrap';
import PropTypes from 'prop-types';
import AsyncSelect from 'react-select/async';
import Animated from 'react-select/animated';
import { cloneDeep, join } from 'lodash';
import MatchInfoService from './Service';
import Converter from '../../Shared/Converter';
import EditableCollapsePanel from '../EditableCollapsePanel';
import Toastr from '../../components/ToastrWrapper';
import Spinner from '../../components/Spinner';
import AgentService from '../Agent/Service';
import TelevisionService from '../Television/Service';

const propTypes = {
  data: PropTypes.object,
  policy: PropTypes.object.isRequired,
  onSave: PropTypes.func.isRequired,
  onEdit: PropTypes.func,
  onCancel: PropTypes.func
};

class FormProviderInfo extends Component {
  constructor(props) {
    super(props);

    const model = this.props.data;
    model.TVs = Converter.convertTVsToOptions(model.TVs || []);
    model.Agents = Converter.convertAgentsToOptions(model.Agents || []);
    model.IsUpdateTVs = true;
    model.IsUpdateChannel = true;
    model.IsUpdateWB = true;
    model.IsUpdateProviders = true;
    model.IsUpdateOtherProviders = true;
    model.IsUpdateAgents = true;
    model.IsUpdatePhone = true;

    this.defaultModel = cloneDeep(model);
    this.state = {
      isLoading: true,
      isEditing: false,
      model
    };
    this.updateModel = this.updateModel.bind(this);
    this.onSelectedChange = this.onSelectedChange.bind(this);
    this.onSave = this.onSave.bind(this);
    this.onCancel = this.onCancel.bind(this);
    this.getPlaceHolder = this.getPlaceHolder.bind(this);
    this.getValueOrDefault = this.getValueOrDefault.bind(this);
  }

  updateModel = e => {
    const { name, value } = e.target;
    const { model } = this.state;
    model[name] = value;
    this.setState({
      model
    });
  };

  onSelectedChange = (field, selectedValues) => {
    const { model } = this.state;
    model[field] = selectedValues;
    this.setState({ model });
  };

  onSave = async () => {
    const confirmResult = await Toastr.confirm(
      'Are you sure to save the changes ?'
    );
    if (confirmResult.value) {
      const { model } = this.state;
      model.TVIds = model.TVs
        ? join(
          model.TVs.map(item => item.value),
          ','
        )
        : null;
      model.AgentIds = model.Agents
        ? join(
          model.Agents.map(item => item.value),
          ','
        )
        : null;
      Spinner.show();
      const response = await MatchInfoService.updateProviderInfo(model);
      model.IsUpdateTVs = true;
      model.IsUpdateChannel = true;
      model.IsUpdateWB = true;
      model.IsUpdateProviders = true;
      model.IsUpdateOtherProviders = true;
      model.IsUpdateAgents = true;
      model.IsUpdatePhone = true;
      Spinner.hide();
      if (response.IsSuccess) {
        await Toastr.showInfo(response.Message);
        this.setState({
          isEditing: false
        });
        this.props.onSave();
        this.defaultModel = cloneDeep(model);
      } else {
        await Toastr.showError(response.Message);
      }
    }
  };

  onEdit = () => {
    this.setState({
      isEditing: true
    });
    this.props.onEdit(true);
  };

  onCancel = () => {
    this.setState({
      isEditing: false,
      model: cloneDeep(this.defaultModel)
    });

    this.props.onCancel(true);
  };

  renderCheckbox = (
    isEditing,
    isUpdateRelativeMatch,
    idTooltip,
    htmlFor,
    isChecked
  ) => {
    return (
      isEditing && (
        <div className='option-wrapper'>
          <Input
            type='checkbox'
            id={htmlFor}
            checked={isChecked}
            onChange={event =>
              this.onSelectedChange(isUpdateRelativeMatch, event.target.checked)}
          />
          <label id={idTooltip} htmlFor={htmlFor} />
          <UncontrolledTooltip placement='right' target={idTooltip}>
            Update for relevant matches
          </UncontrolledTooltip>
        </div>
      )
    );
  };

  getValueOrDefault = value => value || '';

  getPlaceHolder = (value, text) => (!value ? text : '');

  renderTV = (model, isEditing) =>
    <div className='info-item'>
      <div className='label'>TV</div>
      <div className='content'>
        <AsyncSelect
          menuPlacement='auto'
          className='basic-multi-select'
          classNamePrefix='select'
          isMulti
          cacheOptions
          defaultOptions
          isDisabled={!isEditing}
          loadOptions={TelevisionService.getSuggestionOptions}
          value={model.TVs}
          onChange={selectedValues =>
            this.onSelectedChange('TVs', selectedValues)}
          components={Animated}
          placeholder='Please input search'
          noOptionsMessage={() => null}
        />
        {this.renderCheckbox(
          isEditing,
          'IsUpdateTVs',
          'tooltip-10',
          '10',
          model.IsUpdateTVs
        )}
      </div>
    </div>;

  renderIPTV = (model, isEditing) =>
    <div className='info-item'>
      <div className='label'>IPTV/Channel</div>
      <div className='content'>
        <Input
          type='text'
          id='ChannelTooltip'
          name='Channel'
          readOnly={!isEditing}
          className='input-field'
          placeholder={this.getPlaceHolder(model.Channel, 'Add Channel')}
          value={model.Channel}
          onChange={this.updateModel}
          maxLength='100'
        />
        {model.Channel && (
          <UncontrolledTooltip
            placement='right'
            target='ChannelTooltip'
            id='ChannelTooltip'>
            {model.Channel}
          </UncontrolledTooltip>
        )}
        {this.renderCheckbox(
          isEditing,
          'IsUpdateChannel',
          'tooltip-11',
          '11',
          model.IsUpdateChannel
        )}
      </div>
    </div>;

  renderStreaming = (model, isEditing) =>
    <div className='info-item'>
      <div className='label'>Streaming</div>
      <div className='content'>
        <Input
          type='text'
          id='WB'
          name='WB'
          readOnly={!isEditing}
          className='input-field'
          placeholder={this.getPlaceHolder(model.WB, 'Add streaming')}
          value={this.getValueOrDefault(model.WB)}
          onChange={this.updateModel}
          maxLength='100'
        />
        {model.WB && (
          <UncontrolledTooltip
            placement='right'
            target='WB'
            id='WBTooltip'>
            {model.WB}
          </UncontrolledTooltip>
        )}
        {this.renderCheckbox(
          isEditing,
          'IsUpdateWB',
          'tooltip-12',
          '12',
          model.IsUpdateWB
        )}
      </div>
    </div>;

  renderProvider = (model, isEditing) =>
    <div className='info-item'>
      <div className='label'>Provider</div>
      <div className='content'>
        <Input
          type='text'
          id='Provider'
          name='Provider'
          readOnly={!isEditing}
          className='input-field'
          placeholder={this.getPlaceHolder(
            model.Provider,
            'Add Provider'
          )}
          value={this.getValueOrDefault(model.Provider)}
          onChange={this.updateModel}
          maxLength='900'
        />
        {model.Provider && (
          <UncontrolledTooltip
            placement='right'
            target='Provider'
            id='ProviderTooltip'>
            {model.Provider}
          </UncontrolledTooltip>
        )}
        {this.renderCheckbox(
          isEditing,
          'IsUpdateProviders',
          'tooltip-13',
          '13',
          model.IsUpdateProviders
        )}
      </div>
    </div>;

  renderOtherProvider = (model, isEditing) =>
    <div className='info-item'>
      <div className='label'>Other Provider</div>
      <div className='content'>
        <Input
          type='text'
          id='OtherProvider'
          name='OtherProvider'
          readOnly={!isEditing}
          className='input-field'
          placeholder={this.getPlaceHolder(
            model.OtherProvider,
            'Add OtherProvider'
          )}
          value={this.getValueOrDefault(model.OtherProvider)}
          onChange={this.updateModel}
          maxLength='900'
        />
        {model.OtherProvider && (
          <UncontrolledTooltip
            placement='right'
            target='OtherProvider'
            id='OtherProviderTooltip'>
            {model.OtherProvider}
          </UncontrolledTooltip>
        )}

        {this.renderCheckbox(
          isEditing,
          'IsUpdateOtherProviders',
          'tooltip-14',
          '14',
          model.IsUpdateOtherProviders
        )}
      </div>
    </div>;

  renderAgent = (model, isEditing) =>
    <div className='info-item'>
      <div className='label'>Agent</div>
      <div className='content'>
        <AsyncSelect
          menuPlacement='auto'
          className='basic-multi-select'
          classNamePrefix='select'
          isMulti
          cacheOptions
          defaultOptions
          isDisabled={!isEditing}
          loadOptions={AgentService.getSuggestionOptions}
          value={model.Agents}
          onChange={selectedValues =>
            this.onSelectedChange('Agents', selectedValues)}
          components={Animated}
          placeholder='Please input search'
          noOptionsMessage={() => null}
        />
        {this.renderCheckbox(
          isEditing,
          'IsUpdateAgents',
          'tooltip-15',
          '15',
          model.IsUpdateAgents
        )}
      </div>
    </div>;

  renderPhone = (model, isEditing) =>
    <div className='info-item'>
      <div className='label'>Phone</div>
      <div className='content'>
        <Input
          type='text'
          id='Phone'
          name='Phone'
          readOnly={!isEditing}
          className='input-field'
          value={this.getValueOrDefault(model.Phone)}
          placeholder={this.getPlaceHolder(model.Phone, 'Add Phone')}
          onChange={this.updateModel}
          maxLength='200'
        />
        {model.Phone && (
          <UncontrolledTooltip
            placement='right'
            target='Phone'
            id='PhoneTooltip'>
            {model.Phone}
          </UncontrolledTooltip>
        )}
        {this.renderCheckbox(
          isEditing,
          'IsUpdatePhone',
          'tooltip-16',
          '16',
          model.IsUpdatePhone
        )}
      </div>
    </div>;

  render() {
    const { model, isEditing } = this.state;
    const { policy } = this.props;
    return (
      <EditableCollapsePanel
        policy={policy}
        title='Provider Info'
        onEdit={this.onEdit}
        onCancel={this.onCancel}
        onSave={this.onSave}
        isEditing={isEditing}>
        <div className='content form-vertical'>
          {model && this.renderTV(model, isEditing)}
          {model && this.renderIPTV(model, isEditing)}
          {model && this.renderStreaming(model, isEditing)}
          {model && this.renderProvider(model, isEditing)}
          {model && this.renderOtherProvider(model, isEditing)}
          {model && this.renderAgent(model, isEditing)}
          {model && this.renderPhone(model, isEditing)}
        </div>
      </EditableCollapsePanel>
    );
  }
}

FormProviderInfo.propTypes = propTypes;
export default FormProviderInfo;
