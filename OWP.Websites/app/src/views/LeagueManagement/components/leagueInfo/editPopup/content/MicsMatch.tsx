import { InputItem } from '../../component/InputItem';
import { useAppDispatch } from '../../app/hooks';
import { updateObject } from '../slice/editSettingSlice';
import { TEditSetting } from '../../component/Type';

const MicsMatch = (props: { lstNote: string; remark2: string; remark2WK: string, remark2WD: string }) => {
  const { lstNote, remark2, remark2WD, remark2WK } = props;
  const dispatch = useAppDispatch();
  const handleOnChange = (
    event: React.ChangeEvent<HTMLInputElement>,
    value: string,
    inputKey: keyof TEditSetting
  ) => {
    const newValue = event.target.value;
    newValue !== value &&
      dispatch(updateObject({ property: inputKey, value: newValue }));
  };
  return (
    <div className='list'>
      <div className='title'>Display in Misc match file</div>
      <div className='content-column'>
        <InputItem
          label='LST Note (Show under League)'
          value={lstNote}
          handleOnChange={event => handleOnChange(event, lstNote, 'lstNote')}
        />
      </div>
      <div className='content-column'>
        <InputItem
          label='Remark 2 (Show under Match)'
          value={`${remark2}${remark2WD}${remark2WK}`}
          handleOnChange={event => handleOnChange(event, remark2, 'remark2')}
        />
      </div>
    </div>
  );
};

export default MicsMatch;
