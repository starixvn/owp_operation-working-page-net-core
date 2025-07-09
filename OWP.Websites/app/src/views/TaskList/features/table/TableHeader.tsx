import { useState } from 'react';
import DropdownIcon from '../../../../assets/svg-element/DropdownIcon';
import { TABLE_HEADER } from '../const/TableSchema.json';
import useClickOutside from '../../../Shared/customHook/useClickOutside';
import DropdownActionHeader from './DropdownActionHeader';
import { THeader } from '../../type/Header';
import TradingMenuPopup from '../../../Shared/TradingMenuPopup';
import { useAppSelector } from '../../app/hooks';
import { RootState } from '../../app/storeNative';

type colSpanType = {
  [key: string]: number;
};

const TableHeader = (props: THeader) => {
  const { hasDetailInformationPermission, firstMatchCheckedIsSaba } = props;
  const colSpanObject: colSpanType = {
    'FT Trader': 3,
    'Other Product Trader': 3,
    '1H Trader': 2,
    'default': 1
  };

  const [isShowDropdownAction, setIsDropdownAction] = useState(false);
  const ref = useClickOutside<HTMLDivElement>(() => setIsDropdownAction(false));
  const matchesSelected = useAppSelector(
    (state: RootState) => state.taskList.matchesSelected
  );

  const renderDropdown = (index: number) => {
    if (index === 0 && hasDetailInformationPermission) {
      //   return (
      //     <TradingMenuPopup
      //       firstMatchCheckedIsSaba={firstMatchCheckedIsSaba}
      //       matchesSelected={matchesSelected}
      //     />
      //   );
      return (
        <div className='dropdown-wrap'>
          <DropdownIcon
            colorDropdown='black'
            handleOnClick={() => setIsDropdownAction(true)}
          />
          {isShowDropdownAction && (
            <DropdownActionHeader
              firstMatchCheckedIsSaba={firstMatchCheckedIsSaba}
              closePopup={() => setIsDropdownAction(false)}
              refValue={ref}
            />
          )}
        </div>
      );
    }
    return null;
  };

  return (
    <tr className='header-table'>
      {TABLE_HEADER.map((header: string, index) => {
        const colSpan = colSpanObject[header] || colSpanObject.default;
        return (
          <th
            key={index}
            colSpan={colSpan}
            className={`${header} ${index === 0 ? 'first-column' : ''}`}>
            <div className='header-content'>
              {header}
              {renderDropdown(index)}
            </div>
          </th>
        );
      })}
    </tr>
  );
};

export default TableHeader;
