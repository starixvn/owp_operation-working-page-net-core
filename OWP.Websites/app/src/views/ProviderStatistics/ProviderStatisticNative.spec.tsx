import React from 'react';
import { render } from '@testing-library/react';
import { Provider } from 'react-redux';
import ProviderStatisticNative from './ProviderStatisticNative';
import store from './app/storeNative';

jest.mock('./features/filter/Filter', () => {
  return function MockFilter() {
    return <div data-testid="mock-filter">Filter Component</div>;
  };
});

describe('ProviderStatisticNative', () => {
  it('renders without crashing', () => {
    const { container } = render(
      <Provider store={store}>
        <ProviderStatisticNative />
      </Provider>
    );
    expect(container).toBeTruthy();
  });

  it('renders with correct structure', () => {
    const { getByTestId, container } = render(
      <Provider store={store}>
        <ProviderStatisticNative />
      </Provider>
    );

    expect(container.querySelector('.body-content')).toBeInTheDocument();

    expect(container.querySelector('.provider-statistics-body')).toBeInTheDocument();

    expect(getByTestId('mock-filter')).toBeInTheDocument();
  });
}); 