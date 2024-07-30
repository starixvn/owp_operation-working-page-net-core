import { PencilIcon } from 'mdi-react';
import { useState } from 'react';
import GroupPopup from '../../groupPopup/GroupPopup';
import { GroupDataSource } from '../../Type';

const ActionCell = (data: GroupDataSource) => {
  const [isOpenPopup, setIsOpenPopup] = useState<boolean>(false);

  return (
    <>
      <div className='action-cell'>
        <span className='icon-edit' onClick={() => setIsOpenPopup(true)}>
          <PencilIcon size={12} />
        </span>
      </div>
      {isOpenPopup && (
        <GroupPopup
          title='Edit Group'
          setIsOpenPopup={setIsOpenPopup}
          model={data}
        />
      )}
    </>
  );
};

export default ActionCell;
