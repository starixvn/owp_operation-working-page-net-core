import { InputItem } from '../../component/InputItem';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { updateObjectAddPopup } from '../slice/addSettingSlice';

const FtSpreadOffAdd = () => {
  const dispatch = useAppDispatch();
  const { spread } = useAppSelector(state => state.addSettingSlice);
  const handleOnChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = event.target.value;
    dispatch(updateObjectAddPopup({ property: 'spread', value: newValue }));
  };
  return (
    <div className='list'>
      <div className='title'>FT Spread</div>
      <div className='content-column-2'>
        <InputItem
          label='HDP-OU(%)'
          value={spread}
          handleOnChange={handleOnChange}
        />
      </div>
    </div>
  );
};

export default FtSpreadOffAdd;
