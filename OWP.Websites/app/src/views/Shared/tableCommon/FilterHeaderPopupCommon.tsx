import { isEqual } from 'lodash';
import { KeyboardEvent } from 'react';
import { CloseIcon } from 'mdi-react';
import { Button } from 'reactstrap';
import { TFilterState } from './typeCommon';

type FilterPopupProps = {
  columnKey: string;
  model: any; //NOSONAR
  isClearSingleColumnFilter?: boolean;
  onClearSingleColumnFilter?: () => void;
  handleClosedPopup: () => void;
  handleClearFilterCallback: () => void;
  handleFilterBySearchKeyCallback: (filterState: TFilterState) => void;
  handleValueFilterCallback: (filterState: TFilterState) => void;
};

const FilterHeaderPopupCommon = ({
  model,
  columnKey,
  isClearSingleColumnFilter,
  onClearSingleColumnFilter,
  handleClosedPopup,
  handleClearFilterCallback,
  handleFilterBySearchKeyCallback,
  handleValueFilterCallback
}: FilterPopupProps) => {
  const { columnKeyFilterCurrent, valueFilterAllColumn } = model;
  const filterValue = valueFilterAllColumn[columnKey] ?? '';
  const handleClearAllFilter = () => {
    handleClosedPopup();
    handleClearFilterCallback();
  };
  const handleOnSearchFilter = () => {
    const filterState = {
      columnKey,
      keyword: filterValue
    };
    handleFilterBySearchKeyCallback(filterState);
  };
  const handleOnchangeValue = (value: string) => {
    const columnHaveBeenFiltered = {
      columnKey,
      keyword: value
    };
    handleValueFilterCallback(columnHaveBeenFiltered);
  };
  const onClickClosedIcon = () => {
    handleClosedPopup();
  };

  const handelOnKeyDown = (e: KeyboardEvent<HTMLInputElement>): void => {
    if (e.key === 'Enter' && e.currentTarget.value) {
      handleOnSearchFilter();
    }
  };

  const handleClearSingleColumnFilter = () => {
    handleClosedPopup();
    onClearSingleColumnFilter && onClearSingleColumnFilter();
  };

  const renderSingleColumnFilter = () => {
    if (isClearSingleColumnFilter) {
      const isColumnFiltered = model.filterState?.some(
        (filter: { columnKey: string }) => filter.columnKey === columnKey
      );
      return (
        <span
          className={`clear-all ${!isColumnFiltered ? 'disabled' : ''}`}
          onClick={isColumnFiltered ? handleClearSingleColumnFilter : undefined}
          style={
            !isColumnFiltered ? { opacity: 0.5, cursor: 'not-allowed' } : {}
          }>
          Clear Column Filter
        </span>
      );
    }
  };

  return (
    <div
      className={`filter-popup is-show ${
        isEqual(columnKey, columnKeyFilterCurrent)
          ? ''
          : 'is-hide-another-popup'
      }`}>
      <div className='clear-filter-container'>
        {renderSingleColumnFilter()}
        <span className='clear-all' onClick={handleClearAllFilter}>
          Clear All Filters
        </span>
      </div>
      <input
        className='input-field'
        placeholder='Search'
        value={filterValue}
        autoFocus
        onKeyDown={e => handelOnKeyDown(e)}
        onChange={e => handleOnchangeValue(e.target.value)}
      />
      <Button
        className='btn-search'
        disabled={!filterValue}
        onClick={handleOnSearchFilter}>
        Search
      </Button>
    </div>
  );
};

export default FilterHeaderPopupCommon;
