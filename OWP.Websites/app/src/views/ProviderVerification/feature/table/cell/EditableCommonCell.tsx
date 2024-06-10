import { useRef, useState } from 'react';
import { TEditableCell } from '../../../type/table';
import store from '../../../app/storeNative';
import {
  UncontrolledTooltip
} from 'reactstrap';

const EditableCommonCell = ({ rowIndex, className, value, onBlur, columnKey }: TEditableCell) => {
  const [canEdit, setCanEdit] = useState(false);
  const inputRef = useRef<HTMLInputElement>(null);
  const handleEdit = () => {
    const {
      providerPolicy: {
        permission: { HasWrite }
      }
    } = store.getState();
    HasWrite && setCanEdit(true);
  };
  const handleBlur = () => {
    setTimeout(() => {
      setCanEdit(false);
    }, 90);

    onBlur(inputRef.current?.value);
  };

  const id = `${columnKey}${rowIndex}`;
  return (
    <td className={className} onClick={handleEdit}>
      {!canEdit && <>
        <span id={id}>{value}</span>
        <UncontrolledTooltip placement='top' target={id}>
          {value}
        </UncontrolledTooltip>
      </>}
      {canEdit && (
        <input
          type='text'
          className='input-field'
          defaultValue={value}
          ref={inputRef}
          autoFocus
          onBlur={handleBlur}></input>
      )}
    </td>
  );
};

export default EditableCommonCell;
