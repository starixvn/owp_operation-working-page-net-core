import { Provider } from 'react-redux';
import { OWPPage } from '../Shared/common/OWP/OPWPage';
import ProfilePicture from './features/profile/ProfilePicture';
import QuickMessage from './features/quickMessage/QuickMessage';
import store from './app/store';
//import store from './store/store';

const AccountSetting = () => {
  return (
    <Provider store={store}>
      <OWPPage className='account-setting'>
        <ProfilePicture />
        <QuickMessage />
      </OWPPage>
    </Provider>
  );
};

export default AccountSetting;
