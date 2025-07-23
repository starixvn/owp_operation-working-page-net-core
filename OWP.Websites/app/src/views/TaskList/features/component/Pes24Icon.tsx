import { useRef } from 'react';
import { PesOrFiFa } from '../../type/TTaskListTable';
import { UncontrolledTooltip } from 'reactstrap';

const Pes24Icon = (props: PesOrFiFa) => {
  const { IsPES24 } = props;

  const ref = useRef(null);
  return (
    <>
      {IsPES24 && (
        <>
          <i
            className={'icon-match icon-pes24'}
            original-title='Pes24'
            ref={ref}></i>
          <UncontrolledTooltip placement='bottom' target={ref}>
            PES 24
          </UncontrolledTooltip>
        </>
      )}
    </>
  );
};

export default Pes24Icon;
