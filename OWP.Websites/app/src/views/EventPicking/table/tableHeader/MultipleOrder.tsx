import { useState } from 'react';
import DropdownIcon from '../../../../assets/svg-element/DropdownIcon';
import useClickOutside from '../../../Shared/customHook/useClickOutside';
import store from '../../app/store';
import { PERIOD } from '../../filterArea/filterConst.json';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { setMultipleOrder } from '../eventPickingTableSlice/reducer';
import { TOrderDirection, TOrderKey } from '../../types/dataTableType';

const MultipleOrder = () => {
  const dispatch = useAppDispatch();
  const { one_m, three_m, six_m, twelve_m } = useAppSelector(
    s => s.eventPickingTable.multipleOrder
  );
  const [isShowMultipleFilter, setIsShowMultipleFilter] = useState(false);
  const {
    filter: {
      param: { period }
    }
  } = store.getState();

  const ref = useClickOutside<HTMLDivElement>(() =>
    setIsShowMultipleFilter(false)
  );

  const handleChange = (key: TOrderKey, direction?: TOrderDirection) => {
    dispatch(
      setMultipleOrder({
        key,
        direction: direction === 'asc' ? 'desc' : 'asc'
      })
    );
  };

  return (
    <span className='multiple-order'>
      <DropdownIcon
        colorDropdown='black'
        handleOnClick={() => setIsShowMultipleFilter(!isShowMultipleFilter)}
      />
      {isShowMultipleFilter && (
        <div className='dropdown-action is-show' ref={ref}>
          {PERIOD.VALUE.ONE_MONTH in (period as object) && (
            <li
              className='one__month item'
              onClick={() => handleChange('one_m', one_m?.direction)}>
              <span className={`sort-icon ${one_m?.direction}`}></span>
              <label>1m</label>
            </li>
          )}
          {PERIOD.VALUE.THREE_MONTHS in (period as object) && (
            <li
              className='three__month item'
              onClick={() => handleChange('three_m', three_m?.direction)}>
              <span className={`sort-icon ${three_m?.direction}`}></span>
              <label>3m</label>
            </li>
          )}
          {PERIOD.VALUE.SIX_MONTHS in (period as object) && (
            <li
              className='six__month item'
              onClick={() => handleChange('six_m', six_m?.direction)}>
              <span className={`sort-icon ${six_m?.direction}`}></span>
              <label>6m</label>
            </li>
          )}
          {PERIOD.VALUE.TWELVE_MONTH in (period as object) && (
            <li
              className='twelve__month item'
              onClick={() => handleChange('twelve_m', twelve_m?.direction)}>
              <span className={`sort-icon ${twelve_m?.direction}`}></span>
              <label>12m</label>
            </li>
          )}
        </div>
      )}
    </span>
  );
};

export default MultipleOrder;
