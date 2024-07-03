import Chip from '@mui/material/Chip';
import { RecipientIcon } from './RecipientIcon';
import PropTypes from 'prop-types';

export const RecipientsSelectedList = ({
  items = [],
  hasWrite = true,
  onDeleteItem = f => f,
  collapsed = false,
  onToggleList = f => f
}) => {
  const getLabelTag = item => {
    const isGroup =
      item.recipientType === 2 ||
      item.recipientType === 3 ||
      item.recipientType === 4;
    if (isGroup) {
      return item.groupName;
    }
    return item.userName || item.nickName;
  };

  const chips = items.map(item => (
    <Chip
      key={`${item.recipientId} - ${item.recipientType}`}
      avatar={<RecipientIcon type={item.recipientType} />}
      label={getLabelTag(item)}
      size='small'
      disabled={!hasWrite}
      onDelete={() => onDeleteItem(item)}
    />
  ));

  const collapsedChips = [
    ...chips.slice(0, 1),
    items.length > 1 && (
      <Chip
        label={`${items.length - 1} mores`}
        key={Number.MAX_VALUE}
        onClick={() => onToggleList(!collapsed)}
      />
    )
  ];

  return (
    <div className='selected-container'>
      {collapsed ? collapsedChips : chips}
    </div>
  );
};

RecipientsSelectedList.propTypes = {
  items: PropTypes.array,
  hasWrite: PropTypes.bool,
  onDeleteItem: PropTypes.func,
  collapsed: PropTypes.bool,
  onToggleList: PropTypes.func
};
