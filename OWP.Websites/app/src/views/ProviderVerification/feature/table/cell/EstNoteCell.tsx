import { createRef, useState } from 'react';
import { TEditableCell } from '../../../type/table';
import store from '../../../app/storeNative';
import { setValue } from '../tableSlice';
import { useAppDispatch } from '../../../../EventAssignmentNew/app/hooks';
import { saveESTNote } from '../tableThunk';
import PopupEditor from '../../../../Shared/popupEditor/PopupEditor';
import { isEquals } from 'immutability-helper';

const EstNoteCell = ({
  rowIndex,
  value,
  owpMatchId,
  columnKey
}: TEditableCell) => {
  const [canEdit, setCanEdit] = useState(false);
  const popupRef = createRef<HTMLTextAreaElement>();
  const dispatch = useAppDispatch();

  const onClickOutside = () => {
    const { current } = popupRef;
    setCanEdit(false);

    if (!isEquals(value?.trim(), current?.value.trim())) {
      dispatch(
        setValue({
          owpMatchId,
          key: 'EstNote',
          value: current?.value
        })
      );

      dispatch(saveESTNote({ owpMatchId, estNote: current?.value }));
    }
  };


  const handleEdit = () => {
    const {
      providerPolicy: {
        permission: { HasWrite }
      }
    } = store.getState();
    HasWrite && setCanEdit(true);
  };

  return (
    <td onClick={handleEdit}>
      <div className='popup__editor__wrapper'>
        {canEdit && <PopupEditor
          ref={popupRef}
          value={value}
          onBlur={onClickOutside} />}
        <div className='est__note'>
          <span>{value}</span>
        </div>
      </div>
    </td>
  );
};

export default EstNoteCell;
