import { useState } from 'react';
import { FilterIcon } from 'mdi-react';
import FilterHeaderPopupCommon from './FilterHeaderPopupCommon';
import { TFilterState } from './typeCommon';
import useClickOutside from '../customHook/useClickOutside';
type FilterHeaderProps = {
  columnKey: string;
  model: any; //NOSONAR
  isClearSingleColumnFilter?: boolean;
  onClearSingleColumnFilter?: () => void;
  handleClickFilterCallback: (value: string) => void;
  handleClearFilterCallback: () => void;
  handleFilterBySearchKeyCallback: (filterState: TFilterState) => void;
  handleValueFilterCallback: (filterState: TFilterState) => void;
};

const FilterHeaderCommon = ({
  columnKey,
  model,
  isClearSingleColumnFilter,
  onClearSingleColumnFilter,
  handleClickFilterCallback,
  handleClearFilterCallback,
  handleFilterBySearchKeyCallback,
  handleValueFilterCallback
}: FilterHeaderProps) => {

  const { filterState } = model;
  const [isOpenFilter, setIsOpenFilter] = useState(false);

  const handleClickFilter = (value: string) => {
    handleClickFilterCallback(value);
    setIsOpenFilter(true);
  };

  const className = filterState
    .some((x: { columnKey: string }) => x.columnKey === columnKey)
    ? 'is-filter'
    : '';

  const handleClosedPopup = () => {
    setIsOpenFilter(false);
  };

  const filterRef = useClickOutside<HTMLDivElement>(() => setIsOpenFilter(false));



  return (
    <div className='filter-header'>
      <span
        className={`icon-filter ${className}`}
        onClick={() => handleClickFilter(columnKey)}>
        <FilterIcon size={14} />
      </span>
      {isOpenFilter && (
        <div ref={filterRef}>
          <FilterHeaderPopupCommon
            model={model}
            isClearSingleColumnFilter={isClearSingleColumnFilter}
            onClearSingleColumnFilter={onClearSingleColumnFilter}
            handleClosedPopup={handleClosedPopup}
            columnKey={columnKey}
            handleClearFilterCallback={handleClearFilterCallback}
            handleFilterBySearchKeyCallback={handleFilterBySearchKeyCallback}
            handleValueFilterCallback={handleValueFilterCallback}
          />
        </div>
      )}
    </div>
  );
};

export default FilterHeaderCommon;
