import { useState } from 'react';
import { CloseIcon } from 'mdi-react';
import { Button } from 'reactstrap';
import { MultipleFilterPopupProps } from './typeCommon';
import { MultiSelect } from 'react-multi-select-component';

const MultipleFilterHeaderPopupCommon = ({
  options,
  selected,
  onClosedPopup,
  onFilter,
  onClear
}: MultipleFilterPopupProps) => {
  const [value, setValue] = useState(selected || []);

  return (
    <div className='filter-popup multiple is-show'>
      <span className='close-popup' onClick={onClosedPopup}>
        <CloseIcon size={14} />
      </span>
      <MultiSelect
        onChange={setValue}
        value={value || []}
        options={options || []}
        ClearSelectedIcon={null}
        className='multi-select'
        hasSelectAll
        labelledBy='Select...'
        overrideStrings={{
          selectAll: '-- Select All --',
          allItemsAreSelected: 'All'
        }}
        isOpen
      />
      <div className='command'>
        <span className='clear-all' onClick={onClear}>
          Clear All Filters
        </span>
        <button
          className='btn-cancel btn btn btn-secondary'
          onClick={onClosedPopup}>
          Cancel
        </button>
        <Button className='btn-search' onClick={() => onFilter(value)}>
          Search
        </Button>
      </div>
    </div>
  );
};

export default MultipleFilterHeaderPopupCommon;
