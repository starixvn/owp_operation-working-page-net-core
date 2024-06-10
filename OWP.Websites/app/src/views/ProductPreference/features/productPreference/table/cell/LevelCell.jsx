import { dbColumnName, SECTION_LIVE_TRADING_ID } from '../../../../Const.json';
import DropdownCell from './dropdownCell/DropdownCell';
import PropTypes from 'prop-types';
import { updateUserUserInfo } from '../../productSlice';
import useStableDispatch from '../../../../../Shared/customHook/useStableDispatch';
import store from '../../../../app/storeNative';
const propTypes = {
  productInfo: PropTypes.object.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const LevelCell = ({ productInfo, rowIndex }) => {
  const dispatch = useStableDispatch();
  const { level } = dbColumnName;
  const { levelId, sectionId, ...rest } = productInfo;
  const { filter, policy } = store.getState();

  const { levelOptionLiveTrading, levelOptionLiveAssistant } =
    filter.dropdownOptions;

  const option =
    sectionId === SECTION_LIVE_TRADING_ID
      ? levelOptionLiveTrading
      : levelOptionLiveAssistant;

  const { HasWrite, ActorId, Username } = policy.permission;

  const handleSelectChange = value => {
    dispatch(
      updateUserUserInfo({
        key: 'levelId',
        rowIndex,
        value
      })
    );
  };

  const props = {
    options: option,
    ...rest,
    label: level,
    selected: levelId,
    hasWrite: HasWrite,
    actorId: ActorId,
    actorName: Username,
    onSelectChange: handleSelectChange
  };
  return <DropdownCell {...props} />;
};
LevelCell.propTypes = propTypes;
export default LevelCell;
