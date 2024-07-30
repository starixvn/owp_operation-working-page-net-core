
import { InputItem } from '../component/InputItem';

const MicsMatch = (props: { lstNote: string, remark2: string }) => {
  const { lstNote, remark2 } = props;

  return (
    <div className='list'>
      <div className='title'>Display in Misc match file</div>
      <div className='content-column'>
        <InputItem
          label='LST Note (Show under League)'
          inputKey={'lstNote'}
          value={lstNote}
        />
      </div>
      <div className='content-column'>
        <InputItem
          label='Remark 2 (Show under Match)'
          inputKey={'remark2'}
          value={remark2}
        />
      </div>
    </div>
  );
};

export default MicsMatch;