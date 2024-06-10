import { useState } from 'react';
import GroupPopup from '../groupPopup/GroupPopup';
import { GroupDataSource } from '../Type';

const Filter = () => {
  const [isOpenPopup, setIsOpenPopup] = useState<boolean>(false);

  return (
    <div className='filter'>
      <button
        className='button-submit'
        onClick={() => setIsOpenPopup(true)}>
        Add Group
      </button>
      {isOpenPopup && (
        <GroupPopup
          title='Add Group'
          setIsOpenPopup={setIsOpenPopup}
          model={new GroupDataSource()}
        />
      )}
    </div>
  );
};

export default Filter;