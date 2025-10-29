import {
  TABLE_HEADER,
  TABLE_HEADER_TURN_OVER_MARGIN
} from '../TableSchema.json';
import FilterHeader from './FilterHeader';
import CheckboxHeader from './CheckboxHeader';
import store from '../../app/store';
import MultipleFilterHeader from './MultipleFilterHeader';
import { TColumnKey } from '../../types/dataTableType';
import DropdownIcon from '../../../../assets/svg-element/DropdownIcon';
import { useState } from 'react';
import useClickOutside from '../../../Shared/customHook/useClickOutside';
import DropdownActionHeader from './DropdownActionHeader';
import CheckAllHeader from './CheckAllHeader';
import Order from './Order';
import { useAppSelector } from '../../app/hooks';

const TableHeader = () => {
  const {
    pagePolicy: {
      permission: { HasWrite }
    },
    turnOverMarginPolicy: {
      permission: { HasRead }
    }
  } = store.getState();
  const { mode } = useAppSelector(s => s.mode);
  const [isShowDropdownAction, setIsDropdownAction] = useState(false);

  const ref = useClickOutside<HTMLDivElement>(() => setIsDropdownAction(false));

  return (
    <tr>
      {{
        true: TABLE_HEADER_TURN_OVER_MARGIN,
        false: TABLE_HEADER
      }[`${HasRead}`].map((item, index) => {
        return (
          <th colSpan={item?.colSpan} key={index}>
            <div className='header-table'>
              <span>{item.headerName}</span>
              {item?.isOrder && <Order columnKey={item.columnKey} />}
              {item?.isFilter && !item?.isCheckAll && (
                <FilterHeader columnKey={item.columnKey} />
              )}
              {item?.isFilterMultiple && (
                <MultipleFilterHeader
                  columnKey={item.columnKey as TColumnKey}
                />
              )}
            </div>
            <div className='group-multiple-select'>
              {item?.isCheckBox && (
                <CheckboxHeader
                  classNameProps={item?.className}
                  columnKey={item.columnKey}
                />
              )}
              {item?.isCheckAll && (
                <CheckAllHeader
                  classNameProps={item?.className}
                  columnKey={item.columnKey}
                />
              )}
              {item?.isFilter && item?.isCheckAll && (
                <FilterHeader columnKey={item.columnKey} />
              )}

              {item?.isHaveDropdown && HasWrite && (
                <div className='dropdown-wrap'>
                  <DropdownIcon
                    colorDropdown='black'
                    handleOnClick={() => setIsDropdownAction(true)}
                  />
                  {isShowDropdownAction && (
                    <DropdownActionHeader refValue={ref} mode={mode} />
                  )}
                </div>
              )}
            </div>
          </th>
        );
      })}
    </tr>
  );
};

export default TableHeader;
