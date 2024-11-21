import { Provider } from 'react-redux';
import store from './app/store';
import EventPickingManagementNative from './EventPickingManagementNative';

const EventPickingAppNative = () => {
    return (
        <Provider store={store}>
            <EventPickingManagementNative />
        </Provider>
    );
};
export default EventPickingAppNative;
