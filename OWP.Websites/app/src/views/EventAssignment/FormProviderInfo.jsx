import { Component } from 'react';
import { Input } from 'reactstrap';
import PropTypes from 'prop-types';
import AsyncSelect from 'react-select/async';
import Animated from 'react-select/animated';
import { cloneDeep, join } from 'lodash';
import Converter from '../Shared/Converter';
import AgentService from '../LiveDetail/Agent/Service';
import TelevisionService from '../LiveDetail/Television/Service';

const propTypes = {
  parentMatchInfo: PropTypes.object
};

class FormProviderInfo extends Component {
  constructor(props) {
    super(props);
    const model = {
      Channel: '',
      WB: '',
      Phone: '',
      TVs: [],
      Agents: [],
      OtherProvider: '',
      Provider: ''
    };

    const { parentMatchInfo } = this.props;
    if (parentMatchInfo) {
      model.Channel = parentMatchInfo.Channel;
      model.WB = parentMatchInfo.WB;
      model.Phone = parentMatchInfo.Phone;
      model.Provider = parentMatchInfo.Provider;
      model.OtherProvider = parentMatchInfo.OtherProvider;
      model.TVs = Converter.convertTVsToOptions(parentMatchInfo.TVs);
      model.Agents = Converter.convertAgentsToOptions(parentMatchInfo.Agents);
    }

    this.state = {
      model
    };
    this.defaultModel = cloneDeep(model);
    this.updateModel = this.updateModel.bind(this);
    this.onSelectedChange = this.onSelectedChange.bind(this);
    this.setInfoSiteMatchData = this.setInfoSiteMatchData.bind(this);
  }

  updateModel = e => {
    const { name, value } = e.target;
    const { model } = this.state;
    model[name] = value;
    this.setState({
      model
    });
  };

  reset() {
    this.setState({
      model: cloneDeep(this.defaultModel)
    });
  }

  onSelectedChange = (field, selectedValues) => {
    const { model } = this.state;
    model[field] = selectedValues;
    this.setState({ model });
  };

  validateAndGetData = () => {
    const { model } = this.state;
    return {
      code: 0,
      data: {
        ...model,
        TVIds: model.TVs
          ? join(
            model.TVs.map(item => item.value),
            ','
          )
          : null,
        AgentIds: model.Agents
          ? join(
            model.Agents.map(item => item.value),
            ','
          )
          : null
      }
    };
  };

  renderTV = model =>
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
          loadOptions={TelevisionService.getSuggestionOptions}
          value={model.TVs}
          onChange={selectedValues =>
            this.onSelectedChange('TVs', selectedValues)}
          components={Animated}
          placeholder='Please input search'
          noOptionsMessage={() => null}
        />
      </div>
    </div>;

  renderIPTV = model =>
    <div className='info-item'>
      <div className='label'>IPTV/Channel</div>
      <div className='content'>
        <Input
          type='text'
          name='Channel'
          className='input-field input-channel'
          value={model.Channel ? model.Channel : ''}
          onChange={this.updateModel}
          maxLength='100'
        />
      </div>
    </div>;

  renderWB = model =>
    <div className='info-item'>
      <div className='label'>WB</div>
      <div className='content'>
        <Input
          type='text'
          name='WB'
          className='input-field input-wb'
          value={model.WB ? model.WB : ''}
          onChange={this.updateModel}
          maxLength='100'
        />
      </div>
    </div>;

  renderProvider = model =>
    <div className='info-item'>
      <div className='label'>Provider</div>
      <div className='content'>
        <Input
          type='text'
          name='Provider'
          className='input-field input-provider'
          value={model.Provider ? model.Provider : ''}
          onChange={this.updateModel}
          maxLength='900'
        />
      </div>
    </div>;

  renderOtherProvider = model =>
    <div className='info-item'>
      <div className='label'>Other Provider</div>
      <div className='content'>
        <Input
          type='text'
          name='OtherProvider'
          className='input-field input-other-provider'
          value={model.OtherProvider ? model.OtherProvider : ''}
          onChange={this.updateModel}
          maxLength='900'
        />
      </div>
    </div>;

  renderAgent = model =>
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
          loadOptions={AgentService.getSuggestionOptions}
          value={model.Agents}
          onChange={selectedValues =>
            this.onSelectedChange('Agents', selectedValues)}
          components={Animated}
          placeholder='Please input search'
          noOptionsMessage={() => null}
        />
      </div>
    </div>;

  renderPhone = model =>
    <div className='info-item'>
      <div className='label'>Phone</div>
      <div className='content'>
        <Input
          type='text'
          name='Phone'
          className='input-field input-phone'
          value={model.Phone ? model.Phone : ''}
          onChange={this.updateModel}
          maxLength='200'
        />
      </div>
    </div>;

  setInfoSiteMatchData = dataModel => {
    const { model } = this.state;
    model.TVs = dataModel.Televisions
      ? Converter.convertTVsToOptions(dataModel.Televisions)
      : [];
    model.Agents = dataModel.Agents
      ? Converter.convertAgentsToOptions(dataModel.Agents)
      : [];
    model.Channel = dataModel.Channel;
    model.WB = dataModel.WB;
    this.setState({ model });
  };

  render() {
    const { model } = this.state;
    return (
      <div className='card-item'>
        <div className='header'>Provider Info</div>
        <div className='content form-vertical'>
          {this.renderTV(model)}
          {this.renderIPTV(model)}
          {this.renderWB(model)}
          {this.renderProvider(model)}
          {this.renderOtherProvider(model)}
          {this.renderAgent(model)}
          {this.renderPhone(model)}
        </div>
      </div>
    );
  }
}

FormProviderInfo.propTypes = propTypes;
export default FormProviderInfo;
