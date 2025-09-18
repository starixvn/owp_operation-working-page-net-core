import { useState, useEffect } from 'react';
import { Option } from '../../Shared/MultipleSelection/type';
import { TurnoverMarginSelection } from '../types/filterType';
import useStableDispatch from '../../Shared/customHook/useStableDispatch';
import { setPeriod } from './slice/filterSlice';
import { SELECT_TURNOVER_OPTIONS } from './const';
import Select from 'react-select';


const TurnoverMargin = () => {
  const dispatch = useStableDispatch();
  const [selected, setSelected] = useState<Option | null>(
    SELECT_TURNOVER_OPTIONS[1]
  );

  useEffect(() => {
    const defaultOption = SELECT_TURNOVER_OPTIONS[1] as TurnoverMarginSelection;
    const period = {
      [defaultOption.key]: {
        fromDate: defaultOption.fromDate,
        toDate: defaultOption.toDate
      }
    };
    dispatch(setPeriod(period));
  }, []);

  const handleChange = (selectedOption: Option | null) => {
    if (selectedOption) {
      const value = selectedOption as TurnoverMarginSelection;
      const period = {
        [value.key]: {
          fromDate: value.fromDate,
          toDate: value.toDate
        }
      };

      dispatch(setPeriod(period));
      setSelected(selectedOption);
    }
  };

  return (
    <div className='multi-select-dropdown'>
      <div className={`Turnover__Margin--Period`}>
        <div className='label'>Turnover & Margin Period</div>
        <Select
          value={selected}
          className='basic-multi-select'
          classNamePrefix='select'
          options={SELECT_TURNOVER_OPTIONS}
          onChange={handleChange}
        />
      </div>
    </div>
  );
};

export default TurnoverMargin;
