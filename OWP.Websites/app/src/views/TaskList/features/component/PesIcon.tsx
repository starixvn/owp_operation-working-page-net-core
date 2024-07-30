import { useRef } from 'react';
import { PesOrFiFa } from '../../type/TTaskListTable';
import { UncontrolledTooltip } from 'reactstrap';

const PesIcon = (props: PesOrFiFa) => {
  const { IsPES23, IsPESOther } = props;

  const ref = useRef(null);
  return (
    <>
      {(IsPESOther || IsPES23) && (
        <>
          <i
            className={`icon-match ${IsPES23 ? 'icon-pes23' : 'icon-pes'}`}
            original-title='PES'
            ref={ref}></i>
          <UncontrolledTooltip placement='bottom' target={ref}>
            PES
          </UncontrolledTooltip>
        </>
      )}
    </>
  );
};

export default PesIcon;
