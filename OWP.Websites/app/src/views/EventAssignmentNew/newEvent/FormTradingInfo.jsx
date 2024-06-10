import { createRef, Component } from 'react';
import Select from 'react-select';
import { cloneDeep } from 'lodash';
import Converter from '../../Shared/Converter';
import TradingInfoTable from './TradingInfoTable';
import PropTypes from 'prop-types';

const tradingModel = {
  matchOfferValue: '',
  noHDPvalue: '',
  matchSettings: []
};

const propTypes = {
  matchOffers: PropTypes.array,
  noOfHDPs: PropTypes.array,
  bettypes: PropTypes.array
};

const defaultModel = {
  matchOffer: '',
  noHDP: ''
};

class FormTradingInfo extends Component {
  constructor() {
    super();
    this.state = {
      isLoading: true,
      ...cloneDeep(defaultModel)
    };
    this.tableRef = createRef();
    this.handleMatchOfferChange = this.handleMatchOfferChange.bind(this);
    this.handlenoHDPChange = this.handlenoHDPChange.bind(this);
  }

  handlenoHDPChange = noHDP => {
    tradingModel.noHDPvalue = noHDP.value;
    this.setState({
      noHDP
    });
  };

  handleMatchOfferChange = matchOffer => {
    const matchOfferValues = matchOffer.map(s => s.value);
    tradingModel.matchOfferValue = matchOfferValues.join(',');
    this.setState({
      matchOffer
    });
  };

  reset() {
    this.tableRef.current.reset();
    this.setState({
      ...cloneDeep(defaultModel)
    });
  }

  validateAndGetData = () => {
    const matchSettingsValidateResult =
      this.tableRef.current.validateAndGetData();
    if (matchSettingsValidateResult.code) {
      return {
        code: matchSettingsValidateResult.code,
        message: matchSettingsValidateResult.message
      };
    }

    return {
      code: 0,
      data: {
        MatchOfferIds: tradingModel.matchOfferValue,
        NoOfHDPId: tradingModel.noHDPvalue ? tradingModel.noHDPvalue : null,
        MatchSettings: matchSettingsValidateResult.matchSettings
      }
    };
  };

  render() {
    const { matchOffers, noOfHDPs, bettypes } = this.props;
    const [matchOffersOption, noOfHDPsOption] = [
      Converter.convertMatchOffersToOptions(matchOffers),
      Converter.convertNoOfHDPToOptions(noOfHDPs)
    ];
    const { matchOffer, noHDP } = this.state;
    const { handleMatchOfferChange, handlenoHDPChange } = this;
    return (
      <div className='card-item'>
        <div className='header'>Trading Info</div>
        <div className='content form-vertical'>
          <div className='info-item'>
            <div className='label'>Match Offer</div>
            <div className='content'>
              <Select
                options={matchOffersOption}
                className='basic-multi-select'
                classNamePrefix='select'
                isMulti
                value={matchOffer}
                name='match-offer'
                onChange={select => handleMatchOfferChange(select)}
              />
            </div>
          </div>
          <div className='info-item'>
            <div className='label'>No. HDP</div>
            <div className='content'>
              <Select
                options={noOfHDPsOption}
                className='basic-multi-select'
                classNamePrefix='select'
                value={noHDP}
                name='no.HDP'
                onChange={select => handlenoHDPChange(select)}
              />
            </div>
          </div>
        </div>
        <TradingInfoTable bettypes={bettypes} ref={this.tableRef} />
      </div>
    );
  }
}

FormTradingInfo.propTypes = propTypes;
export default FormTradingInfo;
