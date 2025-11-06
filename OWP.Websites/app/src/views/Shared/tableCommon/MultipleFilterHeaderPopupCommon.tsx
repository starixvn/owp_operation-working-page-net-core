import { useState } from 'react';
import { CloseIcon } from 'mdi-react';
import { Button } from 'reactstrap';
import { MultipleFilterPopupProps } from './typeCommon';
import { MultiSelect } from 'react-multi-select-component';
import { Option } from '../MultipleSelection/type';
import { SpecialFilterColumn } from '../Constants';

const MultipleFilterHeaderPopupCommon = ({
  isClearSingleColumnFilter,
  columnKey,
  options,
  selected,
  onClosedPopup,
  onFilter,
  onClear,
  onClearSingleColumnFilter,
  isFilter
}: MultipleFilterPopupProps) => {
  const [value, setValue] = useState(selected || []);
  const [oldValue, setOldValue] = useState(selected || []);

  const filterOptions = (optionList: Option[], filter: string): Option[] => {
    if (filter.length > 0) {
      const filteredOptions = optionList.filter(option =>
        option.label.toLowerCase().includes(filter.toLowerCase())
      );

      setValue(filteredOptions);
      return filteredOptions;
    }
    setValue(oldValue);
    return optionList;
  };

  const handleSearch = () => {
    setOldValue(value);
    onFilter(value);
  };

  const handleKeyDown = (event: React.KeyboardEvent) => {
    if (event.key === 'Enter') {
      handleSearch();
    }
  };

  const renderSingleColumnFilter = () => {
    if (isClearSingleColumnFilter) {
      return (
        <span
          className={`clear-all ${!isFilter ? 'disabled' : ''}`}
          onClick={isFilter ? onClearSingleColumnFilter : undefined}
          style={!isFilter ? { opacity: 0.5, cursor: 'not-allowed' } : {}}>
          Clear Column Filter
        </span>
      );
    }
  };

  return (
    <div
      className='filter-popup multiple is-show'
      onKeyDown={handleKeyDown}
      tabIndex={0}>
      <div className='clear-filter-container'>
        {renderSingleColumnFilter()}
        <span onClick={onClear} className='clear-all'>
          Clear All Filters
        </span>
      </div>
      <MultiSelect
        onChange={setValue}
        value={value || []}
        options={options || []}
        ClearSelectedIcon={null}
        className='multi-select'
        hasSelectAll
        filterOptions={
          columnKey === SpecialFilterColumn.LEAGUE ? filterOptions : undefined
        }
        labelledBy='Select...'
        overrideStrings={{
          selectAll: '-- Select All --',
          allItemsAreSelected: 'All'
        }}
        isOpen
      />
      <div className='command'>
        {/* {renderSingleColumnFilter()}
        <span className='clear-all' onClick={onClear}>
          Clear All Filters
        </span> */}
        <button
          className='btn-cancel btn btn btn-secondary'
          onClick={onClosedPopup}>
          Cancel
        </button>
        <Button className='btn-search' onClick={handleSearch}>
          Search
        </Button>
      </div>
    </div>
  );
};

export default MultipleFilterHeaderPopupCommon;
