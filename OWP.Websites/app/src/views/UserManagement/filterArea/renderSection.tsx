import store from '../app/useStoreNative';
import MultipleSelection from '../../Shared/MultipleSelection/MultipleSelection';

const renderSection = () => {
  const { permissions, currentSection, options, filterValue, setFilterValue } = store.getState();
  const { sections } = options;

  const { userManagementAllSections } = permissions;
  return userManagementAllSections.isRead
    ? <div className='filter-control multiselect'>
      <MultipleSelection
        label='Section'
        className='section'
        disabled={false}
        options={sections}
        value={filterValue.section}
        disableSearch
        onChange={value => setFilterValue(value, 'section')}
      />
    </div>
    : <div className='filter-control'>
      <div className='label'>Section</div>
      <input
        type='text'
        name='username'
        className='input-field'
        disabled
        value={currentSection?.value}
      />
    </div>;
};

export default renderSection;