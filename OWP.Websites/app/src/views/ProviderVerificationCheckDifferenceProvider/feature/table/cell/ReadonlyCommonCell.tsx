import { TCommonCell } from '../../../type/table';
import { UncontrolledTooltip } from 'reactstrap';

const ReadonlyCommonCell = ({
  className,
  value,
  index,
  isCanToolTip
}: TCommonCell) => {

  return (
    <td className={className} id={`${className}-${index}`}>
      <span >{value}</span>
      {isCanToolTip && value && (
        <UncontrolledTooltip placement='top' target={`${className}-${index}`}>
          {value}
        </UncontrolledTooltip>
      )}
    </td>
  );
};

export default ReadonlyCommonCell;
