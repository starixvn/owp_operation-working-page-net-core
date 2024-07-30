import { cloneDeep, isNil } from 'lodash';
import BroomIcon from 'mdi-react/BroomIcon';
import PropTypes from 'prop-types';
import { PureComponent } from 'react';
import Select from 'react-select';
import { MajorBetTypes } from '../Shared/Constants';
import Toastr from '../components/ToastrWrapper';
import OddsCell from './OddsCell';
import Service from './Service';
import {
  calculateAwayOdds,
  calculateOverOdds,
  validateHDPGoal,
  validateOUGoal
} from './helper/helper';

const propTypes = {
  policy: PropTypes.object.isRequired,
  data: PropTypes.object.isRequired,
  matchSettingData: PropTypes.array,
  event: PropTypes.object.isRequired,
  onChange: PropTypes.func.isRequired
};

const defaultOption = { value: null, label: '' };

const teamOptions = [
  { value: 1, label: 'Home' },
  { value: 2, label: 'Away' }
];

class OddsRow extends PureComponent {
  constructor(props) {
    super(props);
    this.onOddsChange = this.onOddsChange.bind(this);
    this.onRowDelete = this.onRowDelete.bind(this);
  }

  onTeamChange(value) {
    const { data } = this.state;
    const newData = cloneDeep(data);
    newData.List[0].TeamChoice = value;
    this.updateStateAndTrigger(newData);
  }

  updateStateAndTrigger(data) {
    const { onChange } = this.props;
    this.setState({ data });
    onChange(data);
  }

  async onRowDelete() {
    const { data, event } = this.props;
    const confirmResult = await Toastr.confirm(
      `Are you sure to delete odds values of Match${
        event.MatchNo ? ' #' + event.MatchNo : ''
      }?`
    );
    if (!confirmResult.value) {
      return;
    }

    const deleteList = [];
    if (!isNil(data.Handicap.Id)) {
      deleteList.push(cloneDeep(data.Handicap));
    }
    data.Handicap.Id = null;
    data.Handicap.HDP = null;
    data.Handicap.Odds1 = null;
    data.Handicap.Odds2 = null;

    if (!isNil(data.OverUnder.Id)) {
      deleteList.push(cloneDeep(data.OverUnder));
    }
    data.OverUnder.Id = null;
    data.OverUnder.HDP = null;
    data.OverUnder.Odds1 = null;
    data.OverUnder.Odds2 = null;

    if (!isNil(data.OneXTwo.Id)) {
      deleteList.push(cloneDeep(data.OneXTwo));
    }
    data.OneXTwo.Id = null;
    data.OneXTwo.Odds1 = null;
    data.OneXTwo.Odds2 = null;
    data.OneXTwo.Odds3 = null;

    this.forceUpdate();
    if (deleteList.length > 0) {
      await Service.deleteOdds({ List: deleteList });
    }

    this.triggerDataChange();
  }

  async onOddsChange(oddsData) {
    const validCount = this.countValidInput(oddsData);
    this.forceUpdate();
    if (isNil(oddsData.Id)) {
      if (validCount === 3) {
        const result = await Service.insertOdds(oddsData);
        oddsData.Id = Number.parseInt(result.Data, 10);
      }
    } else {
      if (validCount === 3) {
        await Service.updateOdds(oddsData);
      } else {
        if (validCount === 0) {
          await Service.deleteOdds({ List: [oddsData] });
          oddsData.Id = null;
        }
      }
    }

    this.triggerDataChange();
  }

  countValidInput(oddsData) {
    let validCount = 0;
    if (oddsData.Bettype !== MajorBetTypes.OneXTwo) {
      if (!isNil(oddsData.HDP)) {
        validCount++;
      }
    } else {
      if (!isNil(oddsData.Odds3)) {
        validCount++;
      }
    }

    if (!isNil(oddsData.Odds1)) {
      validCount++;
    }

    if (!isNil(oddsData.Odds2)) {
      validCount++;
    }
    return validCount;
  }

  isValidate(oddsData) {
    const validCount = this.countValidInput(oddsData);
    return validCount > 0;
  }

  triggerDataChange() {
    const { onChange, data } = this.props;
    onChange(data);
  }

  render() {
    const { policy } = this.props;
    const { data, matchSettingData } = this.props;
    const selectedOption = data.Handicap.TeamChoice
      ? teamOptions[data.Handicap.TeamChoice - 1]
      : defaultOption;
    const isValidateHandicap = this.isValidate(data.Handicap);
    const isValidateOverUnder = this.isValidate(data.OverUnder);
    const isValidateOneXTwo = this.isValidate(data.OneXTwo);
    const isEditHDPOdd =
      data.Handicap.TeamChoice && data.Handicap.TeamChoice > 0;
    return (
      <div className='rows'>
        <div className='cell-handicap'>
          <div className='select-wrapper-default'>
            <Select
              options={teamOptions}
              value={selectedOption}
              className='basic-multi-select'
              classNamePrefix='select'
              isSearchable={false}
              onChange={event => {
                data.Handicap.TeamChoice = event.value;
                this.onOddsChange(data.Handicap);
              }}
              isDisabled={!policy.HasWrite}
            />
          </div>
          <span className='value-hdp'>@</span>
          <OddsCell
            policy={policy}
            value={data.Handicap.HDP}
            isValidate={isValidateHandicap}
            validateValue={validateHDPGoal}
            onChange={value => {
              data.Handicap.HDP = value;
              this.onOddsChange(data.Handicap);
            }}
            isAutoField={!isEditHDPOdd}
          />
        </div>
        <div className='cell-handicap-home'>
          <OddsCell
            policy={policy}
            value={data.Handicap.Odds1}
            isValidate={isValidateHandicap}
            onChange={value => {
              data.Handicap.Odds1 = value;
              data.Handicap.Odds2 = calculateAwayOdds(
                matchSettingData,
                data.Handicap.Odds1
              );
              this.onOddsChange(data.Handicap);
            }}
            isAutoField={!isEditHDPOdd}
          />
        </div>
        <div className='cell-handicap-away'>
          <OddsCell
            policy={policy}
            value={data.Handicap.Odds2}
            isValidate={isValidateHandicap}
            isAutoField
          />
        </div>
        <div className='cell-over-under'>
          <OddsCell
            policy={policy}
            value={data.OverUnder.HDP}
            isValidate={isValidateOverUnder}
            validateValue={validateOUGoal}
            onChange={value => {
              data.OverUnder.HDP = value;
              this.onOddsChange(data.OverUnder);
            }}
          />
        </div>
        <div className='cell-over'>
          <OddsCell
            policy={policy}
            value={data.OverUnder.Odds1}
            isValidate={isValidateOverUnder}
            isAutoField
          />
        </div>
        <div className='cell-under'>
          <OddsCell
            policy={policy}
            value={data.OverUnder.Odds2}
            isValidate={isValidateOverUnder}
            onChange={value => {
              data.OverUnder.Odds2 = value;
              data.OverUnder.Odds1 = calculateOverOdds(
                matchSettingData,
                data.OverUnder.Odds2
              );
              this.onOddsChange(data.OverUnder);
            }}
          />
        </div>
        <div className='cell-one'>
          <OddsCell
            policy={policy}
            value={data.OneXTwo.Odds1}
            isValidate={isValidateOneXTwo}
            onChange={value => {
              data.OneXTwo.Odds1 = value;
              this.onOddsChange(data.OneXTwo);
            }}
          />
        </div>
        <div className='cell-draw'>
          <OddsCell
            policy={policy}
            value={data.OneXTwo.Odds3}
            isValidate={isValidateOneXTwo}
            onChange={value => {
              data.OneXTwo.Odds3 = value;
              this.onOddsChange(data.OneXTwo);
            }}
          />
        </div>
        <div className='cell-two'>
          <OddsCell
            policy={policy}
            value={data.OneXTwo.Odds2}
            isValidate={isValidateOneXTwo}
            onChange={value => {
              data.OneXTwo.Odds2 = value;
              this.onOddsChange(data.OneXTwo);
            }}
          />
        </div>
        {policy.HasWrite && (
          <div className='cell-action'>
            <button onClick={this.onRowDelete} className='btn-delete'>
              <BroomIcon size={13} />
            </button>
          </div>
        )}
      </div>
    );
  }
}

OddsRow.propTypes = propTypes;
export default OddsRow;
