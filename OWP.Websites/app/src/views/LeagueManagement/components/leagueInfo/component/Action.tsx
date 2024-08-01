import DeleteIcon from 'mdi-react/DeleteIcon';
import PlusIcon from 'mdi-react/PlusIcon';
import MaxbetConst from '../../../constants/MaxbetConst';
import { MouseEvent } from 'react';

const Action = ({
  property,
  index,
  hasWrite,
  modelsLength,
  handleAddRow,
  handleDeleteRow
}: {
  property: 'round' | 'division';
  index: number;
  hasWrite: boolean;
  modelsLength: number;
  handleAddRow: (
    event: MouseEvent<HTMLElement>,
    property: 'round' | 'division'
  ) => void;
  handleDeleteRow: (
    event: MouseEvent<HTMLElement>,
    property: 'round' | 'division',
    index: number
  ) => void;
}) => {
  const handleAddRowCallBack = (event: MouseEvent<HTMLElement>) => {
    handleAddRow(event, property);
  };

  const handleDeleteRowCallBack = (event: MouseEvent<HTMLElement>) => {
    handleDeleteRow(event, property, index);
  };

  return (
    <>
      {modelsLength > 1 && hasWrite && (
        <span onClick={handleDeleteRowCallBack}>
          <DeleteIcon className='icon-delete' size={13} />
        </span>
      )}
      {index === modelsLength - 1 &&
        hasWrite &&
        index < MaxbetConst.tvList.maxLength - 1 && (
          <span onClick={handleAddRowCallBack}>
            <PlusIcon size={13} className='icon-add' />
          </span>
        )}
    </>
  );
};

export default Action;
