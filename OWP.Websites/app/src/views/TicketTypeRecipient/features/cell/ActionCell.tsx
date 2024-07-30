import { PencilIcon } from 'mdi-react';

type Props = {
  onOpenPopup: () => void
};

const ActionCell = (props: Props) => {
  return (
    <>
      <div className='action-cell'>
        <span className='icon-edit' onClick={() => props.onOpenPopup()}>
          <PencilIcon size={12} />
        </span>
      </div>
    </>
  );
};

export default ActionCell;
