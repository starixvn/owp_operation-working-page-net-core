import { InputItem } from '../../component/InputItem';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { TAddSetting } from '../../component/Type';
import { updateObjectAddPopup } from '../slice/addSettingSlice';

const MBOfAdd = () => {
  const { maxbetWD_HDP, wD_NoOfHDP, maxbetWK_HDP, wK_NoOfHDP } = useAppSelector(
    state => state.addSettingSlice
  );
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
      <div className='title'>MB</div>
      <div className='content-column-2'>
        <InputItem
          label='Weekday HDP (K)'
          value={maxbetWD_HDP}
          handleOnChange={event => handleOnChange(event, 'maxbetWD_HDP')}
        />
        <InputItem
          label='Weekday NO. HDP'
          value={wD_NoOfHDP}
          handleOnChange={event => handleOnChange(event, 'wD_NoOfHDP')}
        />
      </div>
      <div className='content-column-2'>
        <InputItem
          label='Weekend HDP (K)'
          value={maxbetWK_HDP}
          handleOnChange={event => handleOnChange(event, 'maxbetWK_HDP')}
        />
        <InputItem
          label='Weekend NO. HDP'
          value={wK_NoOfHDP}
          handleOnChange={event => handleOnChange(event, 'wK_NoOfHDP')}
        />
      </div>
    </div>
  );
};

export default MBOfAdd;
