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
  onFilter,
  setOptions,
  setSelected,
  setIsFilter,
  onClear
}: MultipleFilterHeaderProps) => {
  const [isOpenFilter, setIsOpenFilter] = useState(false);
  const filterRef = useClickOutside<HTMLDivElement>(() =>
    setIsOpenFilter(false)
  );

  const handleFilter = (selected: Option[]) => {
    setIsOpenFilter(false);
    onFilter(selected);
  };

  const handleClearFilter = () => {
    setIsOpenFilter(false);
    onClear();
  };
  return (
    <div className='filter-header'>
      <span
        className={`icon-filter ${
          (setIsFilter ? setIsFilter() : isFilter) ? 'is-filter' : ''
        }`}
        onClick={() => setIsOpenFilter(true)}>
        <FilterIcon size={14} />
      </span>
      {isOpenFilter && (
        <div ref={filterRef}>
          <MultipleFilterHeaderPopupCommon
            columnKey={columnKey}
            options={setOptions ? setOptions() : options}
            selected={setSelected ? setSelected() : selected}
            onClosedPopup={() => setIsOpenFilter(false)}
            onFilter={handleFilter}
            onClear={handleClearFilter}
          />
        </div>
      )}
    </div>
  );
};

export default MultipleFilterHeader;
