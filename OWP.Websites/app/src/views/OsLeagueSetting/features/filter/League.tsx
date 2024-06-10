import { useState } from 'react';
import { Input } from 'reactstrap';
import useStore from '../../app/store';
import { filter } from '../../osLeague.json';

const League = (): JSX.Element => {
  const [value, setValue] = useState<string>('');
  const setFilter = useStore(state => state.setFilter);

  return (
    <div className='filter-control'>
      <span className='league'>League</span>
      <Input
        type='text'
        name='league'
        value={value}
        onChange={e => setValue(e.target.value)}
        maxLength={50}
        data-testid='league'
        onBlur={() =>
          setFilter({
            key: filter.league,
            value
          })
        }
      />
    </div>
  );
};

export default League;
