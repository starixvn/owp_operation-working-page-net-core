import Category from './Category';
import Command from './Command';
import League from './League';
import Round from './Round';
import Status from './Status';

const FilterManagementNative = () => {
  return (
    <div className='filter'>
      <League />
      <Status />
      <Category />
      <Round />
      <Command />
    </div>
  );
};


export default FilterManagementNative;