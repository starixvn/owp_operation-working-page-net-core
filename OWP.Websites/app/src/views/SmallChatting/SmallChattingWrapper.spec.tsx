import React from 'react';
import { render } from '@testing-library/react';
import { Provider } from 'react-redux';
import SmallChattingWrapper from './SmallChattingWrapper';
import store from './app/store';

jest.mock('./SignalR/useNotificationWrapper', () => () => (
  <div>Notification</div>
));
jest.mock('./SignalR/useSignalRWrapper', () => () => <div>SignalR</div>);

test('renders SmallChattingWrapper with all components', () => {
  const { getByText } = render(
    <Provider store={store}>
      <SmallChattingWrapper />
    </Provider>
  );

  expect(getByText('Notification')).toBeInTheDocument();
  expect(getByText('SignalR')).toBeInTheDocument();
});
