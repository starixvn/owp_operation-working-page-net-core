import { LEAGUE } from './filterConst.json';
import { useAppSelector } from '../app/hooks';
import { MultiSelect } from 'react-multi-select-component';
import { Option } from '../../Shared/MultipleSelection/type';
import { useEffect, useState } from 'react';
import useStableDispatch from '../../Shared/customHook/useStableDispatch';
import { setFilter } from './slice/filterSlice';

const League = () => {
  const { leagues } = useAppSelector(state => state.filter);
  const [selected, setSelected] = useState<Option[]>(leagues);

  const dispatch = useStableDispatch();
  const handleChange = (event: Option[]) => {
    dispatch(setFilter({
      key: LEAGUE,
      value: event.map(e => e.value)
    }));
    setSelected(event);
  };

  useEffect(() => {
    setSelected(leagues);
  }, [leagues]);

  return (
    <div className='multi-select-dropdown filter-league'>
      <div className='league'>
        <div className='label'>
          League
        </div>
        <MultiSelect
          onChange={handleChange}
          value={selected}
          options={leagues}
          ClearSelectedIcon={null}
          className='multi-select'
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

export default League;