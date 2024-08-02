import { UncontrolledTooltip } from 'reactstrap';
import { v4 as uuidv4 } from 'uuid';

const CommonCell = ({ className = '', value = '' }) => {
  const uuidKey = `${className}_${uuidv4()}`;

  return (
    <td className={className}>
      <div className='row-table' id={uuidKey}>
        {value}
      </div>
      {value?.length > 0 && (
        <UncontrolledTooltip placement='left' target={uuidKey}>
          {value}
        </UncontrolledTooltip>
      )}
    </td>
  );
};

export default CommonCell;
