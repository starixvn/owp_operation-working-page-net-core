import { UncontrolledTooltip } from 'reactstrap';
import { v4 as uuidv4 } from 'uuid';

type TProp = {
  className?: string;
  value?: string | number | boolean;
}
const CommonCell = ({ className, value }: TProp) => {
  const uuidKey = `${className}_${uuidv4()}`;

  return (
    <td className={className}>
      <div className='row-table' id={uuidKey}>
        {value}
      </div>
      {value && (
        <UncontrolledTooltip placement='left' target={uuidKey}>
          {value}
        </UncontrolledTooltip>
      )}
    </td>
  );
};

export default CommonCell;
