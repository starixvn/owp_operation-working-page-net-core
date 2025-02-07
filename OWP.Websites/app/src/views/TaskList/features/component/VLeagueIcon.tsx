import { useRef } from 'react';
import { PesOrFiFa } from '../../type/TTaskListTable';
import { UncontrolledTooltip } from 'reactstrap';

const VLeagueIcon = (props: PesOrFiFa) => {
  const { IsVietNamVLeague } = props;

  const ref = useRef(null);
  return (
    <>
      {IsVietNamVLeague && (
        <>
          <i
            className={'icon-match icon-vn-vleague'}
            original-title='VLeague'
            ref={ref}></i>
          <UncontrolledTooltip placement='bottom' target={ref}>
            VLeague
          </UncontrolledTooltip>
        </>
      )}
    </>
  );
};

export default VLeagueIcon;
