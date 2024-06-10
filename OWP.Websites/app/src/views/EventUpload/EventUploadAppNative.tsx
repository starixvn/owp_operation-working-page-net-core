import { Provider } from 'react-redux';
import store from './app/storeNative';
import EventUploadContent from './features/components/EventUploadContent';

const EventUploadAppNative = () => {
  return (
    <div className='event-upload-container'>
      <Provider store={store}>
        <EventUploadContent />
      </Provider>
    </div>
  );
};

export default EventUploadAppNative;