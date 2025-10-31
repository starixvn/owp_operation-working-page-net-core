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

const FilterHeaderPopupCommon = (
  {
    model,
    columnKey,
    isClearSingleColumnFilter,
    onClearSingleColumnFilter,
    handleClosedPopup,
    handleClearFilterCallback,
    handleFilterBySearchKeyCallback,
    handleValueFilterCallback
  }: FilterPopupProps
) => {
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

  const handelOnKeyDown = (
    e: KeyboardEvent<HTMLInputElement>
  ): void => {
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
      return <span className='clear-all' onClick={handleClearSingleColumnFilter}>
        Clear Filters
      </span>;
    }
  };

  return (
    <div
      className={`filter-popup is-show ${isEqual(columnKey, columnKeyFilterCurrent)
        ? ''
        : 'is-hide-another-popup'
        }`}>
      <span className='close-popup' onClick={() => onClickClosedIcon()}>
        <CloseIcon size={14} />
      </span>
      <input
        className='input-field'
        placeholder='Search'
        value={filterValue}
        autoFocus
        onKeyDown={e => handelOnKeyDown(e)}
        onChange={e => handleOnchangeValue(e.target.value)}
      />
      {renderSingleColumnFilter()}
      <span className='clear-all' onClick={handleClearAllFilter}>
        Clear All Filters
      </span>
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
