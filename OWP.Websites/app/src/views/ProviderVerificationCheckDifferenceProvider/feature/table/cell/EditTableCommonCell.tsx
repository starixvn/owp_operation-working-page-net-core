import { useRef, useState } from 'react';
import { TEditTableCell } from '../../../type/table';
import store from '../../../app/storeNative';
import {
  UncontrolledTooltip
} from 'reactstrap';

const EditTableCommonCell = ({ rowIndex, className, value, columnKey }: TEditTableCell) => {
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

export default EditTableCommonCell;
