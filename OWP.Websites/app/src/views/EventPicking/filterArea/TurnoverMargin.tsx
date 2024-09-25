import { useState } from 'react';
import { Option } from '../../Shared/MultipleSelection/type';
import { TurnoverMarginSelection } from '../types/filterType';
import useStableDispatch from '../../Shared/customHook/useStableDispatch';
import { setPeriod } from './slice/filterSlice';
import { SELECT_TURNOVER_OPTIONS } from './const';
import { MultiSelect } from 'react-multi-select-component';

const TurnoverMargin = () => {
  const dispatch = useStableDispatch();
  const [selected, setSelected] = useState<Option[]>([
    SELECT_TURNOVER_OPTIONS[0]
  ]);

  const handleChange = (value: TurnoverMarginSelection[]) => {
    const period = value.reduce(
      (sl, item) => ({
        ...sl,
        [item.key]: {
          fromDate: item.fromDate,
          toDate: item.toDate
        }
      }),
      {}
    );

    dispatch(setPeriod(period));
    setSelected(value);
  };

  return (
    <div className='multi-select-dropdown'>
      <div className={`Turnover__Margin--Period`}>
        <div className='label'>Turnover & Margin Period</div>
        <MultiSelect
          onChange={handleChange}
          value={selected}
          options={SELECT_TURNOVER_OPTIONS}
          ClearSelectedIcon={null}
          className='multi-select'
          disableSearch
          hasSelectAll
          labelledBy='Select...'
          overrideStrings={{
            selectAll: '-- Select All --',
            allItemsAreSelected: 'All'
          }}
        />
      </div>
    </div>
  );
};

export default TurnoverMargin;
