import DeleteIcon from 'mdi-react/DeleteIcon';
import PlusIcon from 'mdi-react/PlusIcon';
import { useDispatch } from 'react-redux';
import MaxbetConst from '../../../../constants/MaxbetConst';
import { addRowExpressions, removeRowExpressions } from '../slice/editSettingSlice';
import { MouseEvent } from 'react';


const Action = ({ property, index, hasWrite, modelsLength }: {
  property: "round" | "division",
  index: number,
  hasWrite: boolean,
  modelsLength: number
}) => {

  const dispatch = useDispatch();

  const handleAddRow = (event: MouseEvent<HTMLElement>) => {
    event.preventDefault();
    dispatch(addRowExpressions(property));
  };

  const handleDeleteRow = (event: MouseEvent<HTMLElement>) => {
    event.preventDefault();
    dispatch(removeRowExpressions({ index, property }));
  };

  return (
    <>
      {modelsLength > 1 && hasWrite && (
        <span onClick={handleDeleteRow}>
          <DeleteIcon
            className='icon-delete'
            size={13}
          />
        </span>
      )}
      {index === (modelsLength - 1) &&
        hasWrite &&
        index < (MaxbetConst.tvList.maxLength - 1) && (
          <span onClick={handleAddRow}>
            <PlusIcon
              size={13}
              className='icon-add'
            />
          </span>
        )}
    </>
  );
};


export default Action;
