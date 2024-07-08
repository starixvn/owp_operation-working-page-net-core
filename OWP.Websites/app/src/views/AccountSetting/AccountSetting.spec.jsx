import { render } from '@testing-library/react';
import React from 'react';
import AccountSetting from './AccountSetting';

describe('AccountSetting Component', () => {
  test('renders without crashing and has className "account-setting-page"', () => {
    const { container } = render(<AccountSetting />);

    const accountSettingElement = container.querySelector(
      '.account-setting-page'
    );
    expect(accountSettingElement).toBeInTheDocument();
  });
});
