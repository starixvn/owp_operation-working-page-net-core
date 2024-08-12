import { useAppDispatch } from '../../app/hooks';
import CommonTextArea from '../../component/CommonTextArea';
import { updateObject } from '../slice/editSettingSlice';

const Comment = ({ comment }: { comment: string }) => {
  const dispatch = useAppDispatch();

  const handleOnChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
    const newValue = event.target.value;
    comment !== newValue &&
      dispatch(updateObject({ property: 'comment', value: newValue }));
  };

  return (
    <div className='list'>
      <div className='title'>Comment</div>
      <div className='content-column-2'>
        <CommonTextArea
          value={comment}
          rows={6}
          handleOnChange={handleOnChange}
        />
      </div>
    </div>
  );
};

export default Comment;
