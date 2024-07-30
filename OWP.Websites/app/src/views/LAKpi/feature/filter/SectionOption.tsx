import useStore from '../../app/store';
import { MultiSelect } from 'react-multi-select-component';
const SectionOption = () => {
  const sectionOption = useStore(state => state.sectionOption);
  const sectionSelected = useStore(state => state.sectionSelected);
  const setSectionSelected = useStore(state => state.setSectionSelected);

  return (
    <div className='multi-select-dropdown'>
      <div className='label'>Section</div>
      <MultiSelect
        labelledBy='Select section'
        className='status'
        options={sectionOption}
        value={sectionSelected}
        disableSearch
        ClearSelectedIcon={null}
        onChange={setSectionSelected}
      />
    </div>
  );
};

export default SectionOption;
