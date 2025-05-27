import React from 'react';
import { render } from '@testing-library/react';
import { Provider } from 'react-redux';
import ProviderVerificationNative from './ProviderVerificationNative';
import store from './app/storeNative';

jest.mock('./ProviderVerificationAppNative', () => {
  return function MockProviderVerificationAppNative() {
    return <div data-testid="mock-app">Provider Verification App</div>;
  };
});

jest.mock('./feature/signalR/useSignalRWrapper', () => {
  return function mockUseSignalRWrapper() {
    return <div data-testid="mock-signalr">SignalR Wrapper</div>;
  };
});

describe('ProviderVerificationNative', () => {
  it('renders without crashing', () => {
    const { container } = render(
      <Provider store={store}>
        <ProviderVerificationNative />
      </Provider>
    );
    expect(container).toBeTruthy();
  });

  it('renders ProviderVerificationAppNative component', () => {
    const { getByTestId } = render(
      <Provider store={store}>
        <ProviderVerificationNative />
      </Provider>
    );
    expect(getByTestId('mock-app')).toBeInTheDocument();
  });

  it('renders SignalR wrapper', () => {
    const { getByTestId } = render(
      <Provider store={store}>
        <ProviderVerificationNative />
      </Provider>
    );
    expect(getByTestId('mock-signalr')).toBeInTheDocument();
  });

}); 