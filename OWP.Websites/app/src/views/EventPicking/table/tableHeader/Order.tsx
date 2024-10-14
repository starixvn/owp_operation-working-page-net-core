import { useState } from 'react';
import DropdownIcon from '../../../../assets/svg-element/DropdownIcon';
import useClickOutside from '../../../Shared/customHook/useClickOutside';
import store from '../../app/store';
import { PERIOD } from '../../filterArea/filterConst.json';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { setOrder } from '../eventPickingTableSlice/reducer';
import { TOrderDirection, TOrderKey } from '../../types/dataTableType';

const Order = () => {
  const dispatch = useAppDispatch();
  const order = useAppSelector(s => s.eventPickingTable.order);
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
      setOrder({
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
              onClick={() => handleChange('1m', order?.direction)}>
              <span
                className={`sort-icon ${
                  order?.key === '1m' && order?.direction
                }`}></span>
              <label>1m</label>
            </li>
          )}
          {PERIOD.VALUE.THREE_MONTHS in (period as object) && (
            <li
              className='three__month item'
              onClick={() => handleChange('3m', order?.direction)}>
              <span
                className={`sort-icon ${
                  order?.key === '3m' && order?.direction
                }`}></span>
              <label>3m</label>
            </li>
          )}
          {PERIOD.VALUE.SIX_MONTHS in (period as object) && (
            <li
              className='six__month item'
              onClick={() => handleChange('6m', order?.direction)}>
              <span
                className={`sort-icon ${
                  order?.key === '6m' && order?.direction
                }`}></span>
              <label>6m</label>
            </li>
          )}
          {PERIOD.VALUE.TWELVE_MONTH in (period as object) && (
            <li
              className='twelve__month item'
              onClick={() => handleChange('12m', order?.direction)}>
              <span
                className={`sort-icon ${
                  order?.key === '12m' && order?.direction
                }`}></span>
              <label>12m</label>
            </li>
          )}
        </div>
      )}
    </span>
  );
};

export default Order;
