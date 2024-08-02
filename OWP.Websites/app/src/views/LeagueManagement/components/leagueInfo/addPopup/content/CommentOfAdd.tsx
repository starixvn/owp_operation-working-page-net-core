import { useAppDispatch, useAppSelector } from '../../app/hooks';
import CommonTextArea from '../../component/CommonTextArea';
import { updateObjectAddPopup } from '../slice/addSettingSlice';

const CommentOfAdd = () => {
  const dispatch = useAppDispatch();
  const { comment } = useAppSelector(state => state.addSettingSlice);

  const handleOnChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
    const newValue = event.target.value;
    dispatch(updateObjectAddPopup({ property: 'comment', value: newValue }));
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

export default CommentOfAdd;
