import { InputItem } from '../../component/InputItem';
import { useAppDispatch } from '../../app/hooks';
import { updateObject } from '../slice/editSettingSlice';

const FtSpread = ({ spread }: { spread: string }) => {
  const dispatch = useAppDispatch();
  const handleOnChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = event.target.value;
    newValue !== spread &&
      dispatch(updateObject({ property: 'spread', value: newValue }));
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

export default FtSpread;
