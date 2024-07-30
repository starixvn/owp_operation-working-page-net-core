import OpenInNewIcon from '@mui/icons-material/OpenInNew';
import { useSelector } from 'react-redux';
import { openNewChattingTab } from '../Utils';
import { UncontrolledTooltip } from 'reactstrap';
import { useRef } from 'react';

type Props = {
  ticketId: number;
  eventType: number;
};

const IconOpenNewTab = ({ ticketId, eventType }: Props) => {
  const ref = useRef<HTMLDivElement>(null);
  const actorId = useSelector(state => state?.pagePolicy?.permission?.ActorId);
  return (
    <>
      <span
        onClick={() => openNewChattingTab(actorId, ticketId, eventType)}
        className='icon-new-tab'
        ref={ref}>
        <OpenInNewIcon className='icon-new-tab'></OpenInNewIcon>
      </span>
      <UncontrolledTooltip placement='top' target={ref}>
        Open small ticket details
      </UncontrolledTooltip>
    </>
  );
};

export default IconOpenNewTab;
