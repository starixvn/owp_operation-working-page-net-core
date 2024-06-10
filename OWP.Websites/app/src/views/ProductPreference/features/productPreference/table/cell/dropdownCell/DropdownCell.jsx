import { Cell } from 'fixed-data-table-2';
import DropdownDefaultNative from '../../../../../../Shared/common/Dropdown/DropdownDefaultNative';
import { uniqueId } from 'lodash';
import {
  buildPostModelWhenUpdateUserInfo,
  buildUpdateProductUrl,
  getValueIsSelectedFromDropdown
} from '../../../../../utils';
import { post } from '../../../../../../Shared/Service/Service';
import PropTypes from 'prop-types';

const propTypes = {
  options: PropTypes.array.isRequired,
  userId: PropTypes.number.isRequired,
  label: PropTypes.string.isRequired,
  selected: PropTypes.number.isRequired,
  hasWrite: PropTypes.bool,
  username: PropTypes.string.isRequired,
  actorId: PropTypes.number.isRequired,
  actorName: PropTypes.string.isRequired,
  onSelectChange: PropTypes.func
};

const DropdownCell = props => {
  const {
    options,
    userId,
    label,
    selected,
    hasWrite,
    username,
    actorId,
    actorName,
    onSelectChange
  } = props;

  const handleSelectionChanged = async event => {
    const newSelectedValue = getValueIsSelectedFromDropdown(event, options);
    const postModel = buildPostModelWhenUpdateUserInfo(userId, username, label, newSelectedValue);
    const url = buildUpdateProductUrl();
    const prop = {
      url,
      ...postModel,
      actorId,
      actorName
    };
    await post(prop);
    onSelectChange && onSelectChange(newSelectedValue);
  };

  return <Cell>
    <DropdownDefaultNative
      key={uniqueId()}
      options={options}
      selected={selected}
      disabled={!hasWrite}
      onSelectionChanged={handleSelectionChanged}
    />
  </Cell>;
};
DropdownCell.propTypes = propTypes;
export default DropdownCell;
