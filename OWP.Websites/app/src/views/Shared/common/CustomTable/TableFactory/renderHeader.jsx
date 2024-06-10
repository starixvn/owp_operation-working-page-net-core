import OrderHeader from './OrderHeader';
import FilterHeaderUseHookNative from '../../../FilterHeader/FilterHeaderUseHookNative';
import CollapseExpandHeader from './CollapseExpandHeader';
import { uniqueId } from 'lodash';

const renderHeader = headerModel => {
  const {
    isCanSort,
    headerName,
    columnKey,
    isCanFilter,
    isCanCollapseExpand,
    isCheckBox,
    checked,
    className,
    canCheck,
    onCheckChange
  } = headerModel;

  if (isCanSort) {
    return <OrderHeader {...headerModel} />;
  }

  if (isCanCollapseExpand) {
    return <CollapseExpandHeader {...headerModel} />;
  }
  if (isCanFilter) {
    const { filterState, filterFunc, clearFilterFunc } = headerModel;
    const keyword =
      filterState.find(x => x.columnKey === columnKey)?.keyword || '';
    return (
      <FilterHeaderUseHookNative
        key={uniqueId()}
        keyword={keyword}
        filterFunc={filterFunc}
        clearFilterFunc={clearFilterFunc}
        title={headerName}
        name={columnKey}
      />
    );
  }
  if (isCheckBox) {
    return (
      <div className='custom-checkbox'>
        <span
          style={{
            display: 'block'
          }}>
          {headerName}
        </span>
        <input
          type='checkbox'
          className={className}
          disabled={!canCheck}
          checked={checked}
          onChange={onCheckChange}
        />
      </div>
    );
  }
  return headerName;
};

export default renderHeader;
