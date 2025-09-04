import { Fragment } from 'react';
import { UncontrolledTooltip } from 'reactstrap';
import { v4 as uuidv4 } from 'uuid';

type TProp = {
  className?: string;
  value?: string;
}

const CommentCell = ({ className, value }: TProp) => {
  const uuidKey = `${className}_${uuidv4()}`;
  const comments = (value || '').split('\n');

  return (
    <td className={className}>
      <div className='row-table' id={uuidKey}>
        {value}
      </div>
      {value && (
        <UncontrolledTooltip placement='left' target={uuidKey}>
          {comments.map(x => (
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

export default CommentCell;
