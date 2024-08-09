import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';
import { TPermission } from '../../types/dataTableType';
import { buildColumnValueByColumnName } from '../EventPickingTableHelper';

type CheckboxHeaderProps = {
  classNameProps: string | undefined;
  columnKey: string;
};

const CheckboxHeader = ({ classNameProps, columnKey }: CheckboxHeaderProps) => {
  const model = useSelector((state: RootState) => state.eventPickingTable);
  const policy = useSelector((state: RootState) => state.pagePolicy);
  const permission = policy.permission as TPermission;
  const { isNoOfferAll, isConfirmAll } = model;

  const columnValue = buildColumnValueByColumnName(
    columnKey,
    isNoOfferAll,
    isConfirmAll
  );

  return (
    <div className='custom-checkbox'>
      <input
        type='checkbox'
        className={classNameProps}
        disabled={!permission.HasWrite}
        checked={columnValue?.checked}
        onChange={columnValue?.onCheckChange}
      />
    </div>
  );
};
export default CheckboxHeader;
