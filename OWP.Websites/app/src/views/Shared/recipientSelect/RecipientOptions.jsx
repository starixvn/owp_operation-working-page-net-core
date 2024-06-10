import { RecipientIcon } from './RecipientIcon';
import PropTypes from 'prop-types';

export const RecipientOptions = ({ options = [], onAddItem = f => f }) => {
  const contentOption = option => {
    const { groupName } = option;
    return groupName ?? contentUserName(option);
  };

  const contentUserName = ({ nickName, userName }) =>
    nickName ? `${userName} (${nickName})` : `${userName}`;

  const renderOption = option => {
    return (
      <>
        <span className='suggest-option-content'>
          <span>
            <RecipientIcon type={option.recipientType} />
          </span>
          <span>{contentOption(option)}</span>
        </span>
        <button onClick={() => onAddItem(option)}>Add</button>
      </>
    );
  };

  return (
    <ul className='suggest-popup'>
      {options?.length ? (
        options.map(item => (
          <li
            className='suggest-option'
            key={`${item.recipientId} - ${item.recipientType}`}>
            {renderOption(item)}
          </li>
        ))
      ) : (
        <li className='suggest-option MuiAutocomplete-noOptions'>No options</li>
      )}
    </ul>
  );
};

RecipientOptions.propTypes = {
  options: PropTypes.array,
  onAddItem: PropTypes.func
};
