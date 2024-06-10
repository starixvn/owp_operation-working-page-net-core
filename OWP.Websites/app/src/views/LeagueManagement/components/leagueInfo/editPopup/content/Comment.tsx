
import CommonTextArea from '../component/CommonTextArea';

const Comment = ({ comment }: { comment: string }) => {
  return (
    <div className='list'>
      <div className='title'>Comment</div>
      <div className='content-column-2'>
        <CommonTextArea
          value={comment}
          inputKey={'comment'}
          rows={6}
        />
      </div>
    </div >
  );
};

export default Comment;