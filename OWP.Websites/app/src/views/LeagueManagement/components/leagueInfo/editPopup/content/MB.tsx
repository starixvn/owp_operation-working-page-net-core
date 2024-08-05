import { InputItem } from '../../component/InputItem';
import { useAppDispatch } from '../../app/hooks';
import { updateObject } from '../slice/editSettingSlice';
import { TEditSetting } from '../../component/Type';

const MB = (props: {
  maxbetWD_HDP: string;
  wD_NoOfHDP: string;
  maxbetWK_HDP: string;
  wK_NoOfHDP: string;
}) => {
  const { maxbetWD_HDP, wD_NoOfHDP, maxbetWK_HDP, wK_NoOfHDP } = props;
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
      <div className='title'>MB</div>
      <div className='content-column-2'>
        <InputItem
          label='Weekday HDP (K)'
          value={maxbetWD_HDP}
          handleOnChange={event =>
            handleOnChange(event, maxbetWD_HDP, 'maxbetWD_HDP')
          }
        />
        <InputItem
          label='Weekday NO. HDP'
          value={wD_NoOfHDP}
          handleOnChange={event =>
            handleOnChange(event, wD_NoOfHDP, 'wD_NoOfHDP')
          }
        />
      </div>
      <div className='content-column-2'>
        <InputItem
          label='Weekend HDP (K)'
          value={maxbetWK_HDP}
          handleOnChange={event =>
            handleOnChange(event, maxbetWK_HDP, 'maxbetWK_HDP')
          }
        />
        <InputItem
          label='Weekend NO. HDP'
          value={wK_NoOfHDP}
          handleOnChange={event =>
            handleOnChange(event, wK_NoOfHDP, 'wK_NoOfHDP')
          }
        />
      </div>
    </div>
  );
};

export default MB;
