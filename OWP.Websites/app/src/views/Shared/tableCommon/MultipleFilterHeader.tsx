import { FilterIcon } from 'mdi-react';
import { useState } from 'react';
import { MultipleFilterHeaderProps } from './typeCommon';
import MultipleFilterHeaderPopupCommon from './MultipleFilterHeaderPopupCommon';
import useClickOutside from '../customHook/useClickOutside';
import { Option } from '../MultipleSelection/type';

const MultipleFilterHeader = ({
  columnKey,
  options,
  selected,
  isFilter,
  isClearSingleColumnFilter,
  onFilter,
  setOptions,
  setSelected,
  setIsFilter,
  onClear,
  onClearSingleColumnFilter
}: MultipleFilterHeaderProps) => {
  const [isOpenFilter, setIsOpenFilter] = useState(false);
  const filterRef = useClickOutside<HTMLDivElement>(() =>
    setIsOpenFilter(false)
  );

  const handleFilter = (selectedOptions: Option[]) => {
    setIsOpenFilter(false);
    onFilter(selectedOptions);
  };

  const handleClearFilter = () => {
    setIsOpenFilter(false);
    onClear();
  };

  const handleSelfClearFilter = () => {
    setIsOpenFilter(false);
    onClearSingleColumnFilter && onClearSingleColumnFilter();
  };
  return (
    <div className='filter-header'>
      <span
        className={`icon-filter ${(setIsFilter ? setIsFilter() : isFilter) ? 'is-filter' : ''
          }`}
        onClick={() => setIsOpenFilter(true)}>
        <FilterIcon size={14} />
      </span>
      {isOpenFilter && (
        <div ref={filterRef}>
          <MultipleFilterHeaderPopupCommon
            isClearSingleColumnFilter={isClearSingleColumnFilter}
            columnKey={columnKey}
            options={setOptions ? setOptions() : options}
            selected={setSelected ? setSelected() : selected}
            isFilter={setIsFilter ? setIsFilter() : isFilter}
            onClosedPopup={() => setIsOpenFilter(false)}
            onFilter={handleFilter}
            onClear={handleClearFilter}
            onClearSingleColumnFilter={handleSelfClearFilter}
          />
        </div>
      )}
    </div>
  );
};

export default MultipleFilterHeader;
