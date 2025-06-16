import OpenInNewIcon from '@mui/icons-material/OpenInNew';
import { useSelector } from 'react-redux';
import { openNewChattingTab } from '../Utils';
import { UncontrolledTooltip } from 'reactstrap';
import { useRef } from 'react';
import { getServerUrl } from '../RcChat/Service';

type Props = {
  ticketId: number;
  eventType: number;
  discussionName: string;
  authToken: string;
  isRC: boolean;
};

const openDiscussion = async (discussionName: string, authToken: string) => {
  const width = window.innerWidth / 2;
  const height = window.innerHeight / 2;
  const left = (window.innerWidth - width) / 2;
  const top = (window.innerHeight - height) / 2;

  const baseUrl = `${await getServerUrl()
    }/group/${discussionName}`;
  const params = new URLSearchParams({
    layout: 'embedded'
  });
  if (authToken) {
    params.set('resumeToken', authToken);
  }

  const chatUrl = `${baseUrl}?${params.toString()}`;

  window.open(
    chatUrl,
    '_blank',
    `toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=${width},height=${height},top=${top},left=${left}`
  );
};

const IconOpenNewTab = ({
  ticketId,
  eventType,
  discussionName,
  authToken,
  isRC
}: Props) => {
  const ref = useRef<HTMLDivElement>(null);
  const actorId = useSelector(state => state?.pagePolicy?.permission?.ActorId);
  return (
    <>
      <span
        onClick={() =>
          isRC
            ? openDiscussion(discussionName, authToken)
            : openNewChattingTab(actorId, ticketId, eventType)
        }
        className='icon-new-tab'
        ref={ref}>
        <OpenInNewIcon className='icon-new-tab'></OpenInNewIcon>
      </span>
      <UncontrolledTooltip placement='top' target={ref}>
        {isRC ? 'Open discussion' : 'Open small ticket details'}
      </UncontrolledTooltip>
    </>
  );
};

export default IconOpenNewTab;
