import useStore from './app/store';
import { StatusEnum } from './type';
import Feedback from './Feedback.native';
import ThankYou from './ThankYou.native';

const FeedbackSelection = () => {
  const statusForm = useStore(state => state.statusForm);

  return statusForm === StatusEnum.FEED_BACK
    ? <Feedback />
    : <ThankYou />;
};

export default FeedbackSelection;
