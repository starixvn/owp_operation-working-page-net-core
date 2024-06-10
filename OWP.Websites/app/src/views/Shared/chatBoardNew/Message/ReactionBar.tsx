import { ReactionBarSelector } from '@charkour/react-reactions';
import useStableDispatch from '../../customHook/useStableDispatch';
import { ReactionIcon } from '../const/index.json';
import { updateReactIconMessage } from '../chatBoardThunkNative';

type ReactionBarProps = {
  isOpenReactionIcon: boolean;
  setIsOpenReactionIcon: (isOpen: boolean) => void;
  messageId: number;
  creatorId: number;
  ticketId: number;
};

const ReactionBar = ({
  isOpenReactionIcon,
  setIsOpenReactionIcon,
  messageId,
  creatorId,
  ticketId
}: ReactionBarProps) => {
  const dispatch = useStableDispatch();

  const handleOnSelectReactionIcon = (key: string) => {
    setIsOpenReactionIcon(false);
    const model = {
      messageId,
      creatorId,
      iconKey: key,
      ticketId
    };
    dispatch(updateReactIconMessage(model));
  };

  return (
    <>
      {isOpenReactionIcon && (
        <div className='list-icon-reaction'>
          <ReactionBarSelector
            iconSize={15}
            reactions={ReactionIcon}
            onSelect={handleOnSelectReactionIcon}
            style={{ margin: 0 }}
          />
        </div>
      )}
    </>
  );
};

export default ReactionBar;
