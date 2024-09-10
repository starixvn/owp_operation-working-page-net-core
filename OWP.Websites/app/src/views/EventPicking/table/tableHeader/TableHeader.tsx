import {
  TABLE_HEADER,
  TABLE_HEADER_TURN_OVER_MARGIN
} from '../TableSchema.json';
import FilterHeader from './FilterHeader';
import CheckboxHeader from './CheckboxHeader';
import store from '../../app/store';
import MultipleFilterHeader from './MultipleFilterHeader';
import { TColumnKey } from '../../types/dataTableType';
import { VIEW } from '../../mode/mode.json';
import DropdownIcon from '../../../../assets/svg-element/DropdownIcon';
import { useState } from 'react';
import useClickOutside from '../../../Shared/customHook/useClickOutside';
import DropdownActionHeader from './DropdownActionHeader';
import CheckAllHeader from './CheckAllHeader';

const TableHeader = () => {
  const {
    turnOverMarginPolicy: {
      permission: { HasRead }
    },
    mode: { mode }
  } = store.getState();
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
              {item?.isFilter && mode === VIEW && (
                <FilterHeader columnKey={item.columnKey} />
              )}
              {item?.isFilterMultiple && mode === VIEW && (
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
              {item?.isHaveDropdown && (
                <div className='dropdown-wrap'>
                  <DropdownIcon
                    colorDropdown='black'
                    handleOnClick={() => setIsDropdownAction(true)}
                  />
                  {isShowDropdownAction && (
                    <DropdownActionHeader refValue={ref} />
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
