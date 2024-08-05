import { InputItem } from '../../component/InputItem';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { TAddSetting } from '../../component/Type';
import { updateObjectAddPopup } from '../slice/addSettingSlice';

const MiscMatchOfAdd = () => {
  const { lstNote, remark2 } = useAppSelector(state => state.addSettingSlice);
  const dispatch = useAppDispatch();
  const handleOnChange = (
    event: React.ChangeEvent<HTMLInputElement>,
    inputKey: keyof TAddSetting
  ) => {
    const newValue = event.target.value;
    dispatch(updateObjectAddPopup({ property: inputKey, value: newValue }));
  };
  return (
    <div className='list'>
      <div className='title'>Display in Misc match file</div>
      <div className='content-column'>
        <InputItem
          label='LST Note (Show under League)'
          value={lstNote}
          handleOnChange={event => handleOnChange(event, 'lstNote')}
        />
      </div>
      <div className='content-column'>
        <InputItem
          label='Remark 2 (Show under Match)'
          value={remark2}
          handleOnChange={event => handleOnChange(event, 'remark2')}
        />
      </div>
    </div>
  );
};

export default MiscMatchOfAdd;
