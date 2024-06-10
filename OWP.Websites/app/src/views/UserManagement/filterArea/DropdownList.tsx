import MultipleSelection from '../../Shared/MultipleSelection/MultipleSelection';
import store from '../app/useStoreNative';
import renderSection from './renderSection';

const DropdownList = () => {
  const { options, filterValue, setFilterValue } = store.getState();
  const { roles, departments, categories, locationCodes } = options;

  return (
    <>
      <div className='filter-control multiselect'>
        <MultipleSelection
          label='Department'
          disabled={false}
          className='department'
          options={departments}
          value={filterValue.department}
          onChange={value => setFilterValue(value, 'department')}
        />
      </div>
      {renderSection()}
      <div className='filter-control multiselect'>
        <MultipleSelection
          label='Role'
          className='role'
          disabled={false}
          options={roles}
          value={filterValue.role}
          disableSearch
          onChange={value => setFilterValue(value, 'role')}
        />
      </div>
      <div className='filter-control multiselect'>
        <MultipleSelection
          label='Category'
          disabled={false}
          options={categories}
          disableSearch
          value={filterValue.category}
          onChange={value => setFilterValue(value, 'category')}
        />
      </div>
      <div className='filter-control multiselect'>
        <MultipleSelection
          label='Location'
          disabled={false}
          options={locationCodes}
          disableSearch
          value={filterValue.locationCode}
          onChange={value => setFilterValue(value, 'locationCode')}
        />
      </div>
    </>
  );
};

DropdownList.propTypes = {};

export default DropdownList;
