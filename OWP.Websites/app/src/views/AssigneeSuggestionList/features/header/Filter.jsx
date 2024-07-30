import Select from 'react-select';
import { OVERT_TIME } from '../../const/OptionsConst';
import { shallow } from 'zustand/shallow';
import useStore from '../../app/useStoreNative';

const Filter = () => {
  const filterByOtValue = useStore(state => state.filterByOtValue, shallow);

  return (
    <>
      <div className='overtime__label'>Overtime</div>
      <div className='overtime-dropdown'>
        <div className='overtime-filter-control'>
          <Select
            data-testId='select'
            options={OVERT_TIME.OPTIONS}
            defaultValue={OVERT_TIME.OPTIONS[0]}
            onChange={({ value }) => filterByOtValue(value)}
            className='basic-filter-select'
          />
        </div>
      </div>
    </>
  );
};

export default Filter;
