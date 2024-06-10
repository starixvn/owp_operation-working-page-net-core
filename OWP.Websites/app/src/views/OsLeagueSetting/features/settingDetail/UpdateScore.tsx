import { ChangeEvent, useState } from 'react';
import { IUpdateScore } from '../../typing/type';
import constant from '../../osLeague.json';
import ErrorMessage from './ErrorMessage';
import useStore from '../../app/store';

const UpdateScore = ({
  isIptvOnly,
  isManualUpdateScore,
  onChange,
  disabled
}: IUpdateScore) => {
  const [stateCheck, setState] = useState({
    isIptvOnly,
    isManualUpdateScore
  });
  const errorMessage = useStore(state => state.errorMessage.iptv);

  const handleChange = (key: string, event: ChangeEvent<HTMLInputElement>) => {
    const { checked } = event.target;
    setState({
      ...stateCheck,
      [key]: checked
    });
    onChange && onChange(key, checked);
  };

  return (
    <div className='update-score-content'>
      <label className='color-transparent'>Remark</label>
      <div>
        <input
          type='checkbox'
          id='update-score'
          data-testid='update-score-testid'
          checked={stateCheck.isManualUpdateScore}
          onChange={event => handleChange(constant.isManualUpdateScore, event)}
          disabled={disabled}
        />
        <label className='checkbox-label' htmlFor='update-score'>
          Manual Update Score
        </label>
      </div>
      <div>
        <input
          type='checkbox'
          id='iptv-only'
          data-testid='iptv-only-testid'
          defaultChecked={stateCheck.isIptvOnly}
          disabled={disabled}
          onChange={event => handleChange(constant.isIptvOnly, event)}
        />
        <label className='checkbox-label' htmlFor='iptv-only'>
          IPTV Only
        </label>{' '}
        <ErrorMessage message={errorMessage}></ErrorMessage>
      </div>
    </div>
  );
};

export default UpdateScore;
