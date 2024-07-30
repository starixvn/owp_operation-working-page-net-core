import { useRef } from 'react';
import { TableCell } from '../../TypeNative';
import { UncontrolledTooltip } from 'reactstrap';
const MainBetTypeCell = ({ value }: TableCell) => {
  const ref = useRef<HTMLDivElement>(null);
  return (
    <>
      <div className='main-bet-type' ref={ref}>
        <span>{value}</span>
      </div>
      {value && (
        <UncontrolledTooltip placement='right' target={ref}>
          {value}
        </UncontrolledTooltip>
      )}
    </>
  );
};

export default MainBetTypeCell;
