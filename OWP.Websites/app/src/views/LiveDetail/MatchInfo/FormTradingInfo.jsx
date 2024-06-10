import { Component } from 'react';
import Select from 'react-select';
import PropTypes from 'prop-types';
import { cloneDeep } from 'lodash';
import MatchInfoService from './Service';
import EditableCollapsePanel from '../EditableCollapsePanel';
import Toastr from '../../components/ToastrWrapper';
import Spinner from '../../components/Spinner';

const propTypes = {
  noOfHDPOptions: PropTypes.array.isRequired,
  matchOfferOptions: PropTypes.array.isRequired,
  data: PropTypes.object.isRequired,
  policy: PropTypes.object.isRequired,
  onSave: PropTypes.func.isRequired,
  onEdit: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired
};

class FormTradingInfo extends Component {
  constructor(props) {
    super(props);

    const { data } = this.props;
    this.defaultModel = {
      NoOfHDP: data.NoOfHDP,
      MatchOffers: data.MatchOffers
    };
    this.state = {
      isEditing: false,
      model: cloneDeep(this.defaultModel)
    };
    this.updateModel = this.updateModel.bind(this);
  }

  updateModel = (name, value) => {
    const { model } = this.state;
    model[name] = value;
    this.setState({
      model
    });
  };

  onSave = async () => {
    const confirmResult = await Toastr.confirm(
      'Are you sure to save the changes ?'
    );
    if (confirmResult.value) {
      const { data, onSave } = this.props;
      const { model } = this.state;
      const { NoOfHDP, MatchOffers } = model;
      const updateModel = {
        OWPMatchId: data.OWPMatchId,
        NoOfHDPId: NoOfHDP.value,
        MatchOfferIds: MatchOffers.map(v => v.value).join(',')
      };

      Spinner.show();
      const response = await MatchInfoService.updateTradingInfo(updateModel);
      Spinner.hide();
      if (response.IsSuccess) {
        await Toastr.showInfo(response.Message);
        this.defaultModel = cloneDeep(model);
        this.setState({
          isEditing: false
        });
        onSave({
          NoOfHDP,
          MatchOffers
        });
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

  render() {
    const { isEditing, model } = this.state;
    const { MatchOffers, NoOfHDP } = model;
    const { policy, noOfHDPOptions, matchOfferOptions } = this.props;

    return (
      <EditableCollapsePanel
        policy={policy}
        title='Trading Info'
        onEdit={this.onEdit}
        onCancel={this.onCancel}
        onSave={this.onSave}
        isEditing={isEditing}>
        <div className='content form-vertical'>
          <div className='info-item'>
            <div className='label'>Match Offer</div>
            <div className='content'>
              <Select
                options={matchOfferOptions}
                className='basic-multi-select'
                classNamePrefix='select'
                isMulti
                value={MatchOffers}
                name='MatchOffers'
                isDisabled={!isEditing}
                onChange={value => this.updateModel('MatchOffers', value || [])}
              />
            </div>
          </div>
          <div className='info-item'>
            <div className='label'>No. HDP</div>
            <div className='content'>
              <Select
                options={noOfHDPOptions}
                className='basic-multi-select'
                classNamePrefix='select'
                value={NoOfHDP}
                name='NoOfHDP'
                isDisabled={!isEditing}
                onChange={value => this.updateModel('NoOfHDP', value)}
              />
            </div>
          </div>
        </div>
      </EditableCollapsePanel>
    );
  }
}

FormTradingInfo.propTypes = propTypes;
export default FormTradingInfo;
