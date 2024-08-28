import { uniqueId } from 'lodash';
import { useSelector } from 'react-redux';
import ReactTooltip from 'react-tooltip';
import { updateReactIconMessage } from '../chatBoardNew/chatBoardThunkNative';
import { ReactionIcon } from '../chatBoardNew/const/index.json';
import AvatarDisplay from '../common/Avatar/AvatarDisplay';
import useStableDispatch from '../customHook/useStableDispatch';

type UseReact = {
  userName: string;
  userId: number;
  avatarLink: string;
};

type Props = {
  iconKey: string;
  userReact: UseReact[];
  messageId: number;
  ticketId: number;
  isDisableClickIcon: boolean;
};

const TableEmoji = (userReacts: UseReact[]) => {
  return (
    <div className='table-container'>
      <table className='react-table'>
        <tbody>
          {userReacts?.map(x => (
            <tr key={uniqueId()}>
              <td>
                <AvatarDisplay imageFileName={x.avatarLink} size={15} />
              </td>
              <td className='user-name-message-icon'>{x.userName}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

const IconEmoji = ({
  iconKey,
  userReact,
  messageId,
  ticketId,
  isDisableClickIcon = false
}: Props) => {
  const dispatch = useStableDispatch();
  const actorId = useSelector(state => state?.pagePolicy?.permission?.ActorId);

  const uuidKey = `custom-tooltip-${uniqueId()}`;
  const handleOnClick = (_: React.MouseEvent<HTMLDivElement, MouseEvent>) => {
    const model = {
      messageId,
      creatorId: actorId,
      iconKey,
      ticketId
    };
    !isDisableClickIcon && dispatch(updateReactIconMessage(model));
  };

  return (
    <div>
      <div
        data-tip
        data-for={uuidKey}
        onClick={handleOnClick}
        className='icon-box'>
        {ReactionIcon.find(x => x.label === iconKey)?.node}
        {userReact.length > 1 && userReact.length}
        <ReactTooltip
          clickable={true}
          id={uuidKey}
          place='top'
          effect='solid'
          className='custom-tooltip'>
          {TableEmoji(userReact)}
        </ReactTooltip>
      </div>
    </div>
  );
};

export default IconEmoji;
