import { THANK_YOU, HEADER, NEXT_STEP } from './const.json';
import ThankImage from '../../assets/img/feedback-thank.png';

const {
  TITLE
} = HEADER;

const ThankYou = () => <div className='feed__back'>
  <div className='header'>
    <p className='title'>{TITLE}</p>
    <span className='description' style={{ width: '100%' }}>{THANK_YOU.EN}</span>
    <p className='description'>{THANK_YOU.CN}</p>
  </div>
  <div className='body'>
    <span className='next__step' style={{ width: '100%' }} dangerouslySetInnerHTML={{ __html: NEXT_STEP.EN }} />
    <p className='next__step' dangerouslySetInnerHTML={{ __html: NEXT_STEP.CN }} />
    <div className='next__step__img'>
      <img className='image' src={ThankImage} alt="next step" />
    </div>
  </div>
</div>;

export default ThankYou;
