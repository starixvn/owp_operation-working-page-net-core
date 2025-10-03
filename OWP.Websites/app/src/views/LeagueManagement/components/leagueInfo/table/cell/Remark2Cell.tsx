import { Fragment } from 'react';
import { UncontrolledTooltip } from 'reactstrap';
import { v4 as uuidv4 } from 'uuid';

type TProp = {
  className?: string;
  remark2?: string;
}

const Remark2Cell = ({ className, remark2 }: TProp) => {
  const uuidKey = `${className}_${uuidv4()}`;
  const remark = [remark2].filter(r => r);
  return (
    <td className={className}>
      <div className='row-table' id={uuidKey}>
        {remark.map(r => (
          <>{r}<br /></>
        ))}
      </div>
      {remark && (
        <UncontrolledTooltip placement='left' target={uuidKey}>
          {remark.map(x => (
            <Fragment key={uuidv4()}>
              <span style={{ textAlign: 'start' }}>{x}</span>
              <br />
            </Fragment>
          ))}
        </UncontrolledTooltip>
      )}
    </td>
  );
};

export default Remark2Cell;
