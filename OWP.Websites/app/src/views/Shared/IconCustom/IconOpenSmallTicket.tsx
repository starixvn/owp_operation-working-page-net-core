import OpenInNewIcon from '@mui/icons-material/OpenInNew';
import { openNewSmallTicket } from '../Utils';
import { UncontrolledTooltip } from 'reactstrap';
import { useRef } from 'react';
const IconOpenSmallTicket = () => {
  const ref = useRef<HTMLDivElement>(null);

  return (
    <>
      <span
        onClick={openNewSmallTicket}
        ref={ref}
        className='icon-small-ticket-tab'>
        <OpenInNewIcon className='icon-small-ticket-tab'></OpenInNewIcon>
      </span>
      <UncontrolledTooltip placement='top' target={ref}>
        Open small ticket list
      </UncontrolledTooltip>
    </>
  );
};

export default IconOpenSmallTicket;
