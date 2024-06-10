import { BODY } from '../const.json';
import AngryIcon from '../../../assets/svg-element/AngryIcon';
import SadIcon from '../../../assets/svg-element/SadIcon';
import NormalIcon from '../../../assets/svg-element/NormalIcon';
import GoodIcon from '../../../assets/svg-element/GoodIcon';
import AmazingIcon from '../../../assets/svg-element/AmazingIcon';
import useStore from '../app/store';
import { RateIconEnum } from '../type';
import AngryActiveIcon from '../../../assets/svg-element/AngryActiveIcon';
import SadActiveIcon from '../../../assets/svg-element/SadActiveIcon';
import NormalActiveIcon from '../../../assets/svg-element/NormalActiveIcon';
import GoodActiveIcon from '../../../assets/svg-element/GoodActiveIcon';
import AmazingActiveIcon from '../../../assets/svg-element/AmazingActiveIcon';

const RateFunction = () => {
  const updateFeedbackInfo = useStore(state => state.updateFeedbackInfo);
  const rate = useStore(state => state.feedBackInfo.rate);
  return (
    <div className='rate__function'>
      <p className='title' style={{ marginBottom: '3px' }}>
        {BODY.RATE_FUNCTION.EN}
        <span className='required'> *</span>
      </p>
      <p className='title'>{BODY.RATE_FUNCTION.CN}</p>
      <div className='rate__icon'>
        {rate !== RateIconEnum.ANGRY ? (
          <a onClick={() => updateFeedbackInfo('rate', RateIconEnum.ANGRY)}>
            <AngryIcon />
          </a>
        ) : (
          <a onClick={() => updateFeedbackInfo('rate', RateIconEnum.DEFAULT)}>
            <AngryActiveIcon />
          </a>
        )}
        {rate !== RateIconEnum.SAD ? (
          <a onClick={() => updateFeedbackInfo('rate', RateIconEnum.SAD)}>
            <SadIcon />
          </a>
        ) : (
          <a onClick={() => updateFeedbackInfo('rate', RateIconEnum.DEFAULT)}>
            <SadActiveIcon />
          </a>
        )}
        {rate !== RateIconEnum.NORMAL ? (
          <a onClick={() => updateFeedbackInfo('rate', RateIconEnum.NORMAL)}>
            <NormalIcon />
          </a>
        ) : (
          <a onClick={() => updateFeedbackInfo('rate', RateIconEnum.DEFAULT)}>
            <NormalActiveIcon />
          </a>
        )}
        {rate !== RateIconEnum.GOOD ? (
          <a onClick={() => updateFeedbackInfo('rate', RateIconEnum.GOOD)}>
            <GoodIcon width={17} height={17} />
          </a>
        ) : (
          <a onClick={() => updateFeedbackInfo('rate', RateIconEnum.DEFAULT)}>
            <GoodActiveIcon />
          </a>
        )}
        {rate !== RateIconEnum.AMAZING ? (
          <a onClick={() => updateFeedbackInfo('rate', RateIconEnum.AMAZING)}>
            <AmazingIcon />
          </a>
        ) : (
          <a onClick={() => updateFeedbackInfo('rate', RateIconEnum.DEFAULT)}>
            <AmazingActiveIcon />
          </a>
        )}
      </div>
    </div>
  );
};

export default RateFunction;
