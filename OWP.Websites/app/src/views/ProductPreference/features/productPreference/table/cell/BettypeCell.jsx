import { dbColumnName, SECTION_LIVE_TRADING_ID } from '../../../../Const.json';
import PropTypes from 'prop-types';
import {
  buildUpdateProductUrl,
  compareTwoNumberArray
} from '../../../../utils';
import { post } from '../../../../../Shared/Service/Service';
import MultipleSelectionTemplate from '../../../filter/MultipleSelectionTemplate';
import { updateUserUserInfo } from '../../productSlice';
import useStableDispatch from '../../../../../Shared/customHook/useStableDispatch';
import { uniqueId } from 'lodash';
import store from '../../../../app/storeNative';
const propTypes = {
  productInfo: PropTypes.object.isRequired,
  rowIndex: PropTypes.number.isRequired,
  onShowDropdownListOption: PropTypes.func.isRequired
};

const BettypeCell = ({ productInfo, rowIndex, onShowDropdownListOption }) => {
  const dispatch = useStableDispatch();
  const { betTypePreferences } = dbColumnName;
  const { sectionId, userId, username, betTypePreferencesValues } = productInfo;
  const { filter, policy } = store.getState();
  const { HasWrite, ActorId, Username } = policy.permission;
  const { mainBettypeOptionLiveTrading, mainBettypeOptionLiveAssistant } =
    filter.dropdownOptions;
  const option =
    sectionId === SECTION_LIVE_TRADING_ID
      ? mainBettypeOptionLiveTrading
      : mainBettypeOptionLiveAssistant;

  const handleSelectionChanged = async event => {
    const currentBettypeValue = betTypePreferencesValues.map(x => x.value);
    const newBettypeValue = event.map(x => x.value);

    if (compareTwoNumberArray(currentBettypeValue, newBettypeValue)) {
      return;
    }
    const url = buildUpdateProductUrl();
    const prop = {
      url,
      userId,
      actorName: Username,
      label: betTypePreferences,
      value: newBettypeValue.join(','),
      actorId: ActorId,
      username
    };
    await post(prop);
    dispatch(
      updateUserUserInfo({
        key: 'betTypePreferencesValues',
        rowIndex,
        value: event
      })
    );
  };
  return (
    <MultipleSelectionTemplate
      key={uniqueId()}
      options={option}
      value={betTypePreferencesValues}
      disabled={!HasWrite}
      handleChange={handleSelectionChanged}
      onShowDropdownListOption={onShowDropdownListOption}
    />
  );
};

BettypeCell.propTypes = propTypes;
export default BettypeCell;
