import {
  render,
  waitForElementToBeRemoved,
  waitFor
} from '@testing-library/react';
import Notification from './Notification';
import AuthorizeService from '../Authorize/Service';
import { FunctionKeys } from '../Shared/Constants';
import { setImmediate } from 'timers';

jest.mock('../Authorize/Service');

function flushPromises() {
  return new Promise(resolve => setImmediate(resolve));
}

describe('Render Notification', () => {
  const policy = { UserId: 8, Username: 'sa', HasRead: true, HasWrite: true };
  beforeEach(() => {
    AuthorizeService.authorize.mockResolvedValue(policy);
  });
  afterEach(() => {
    jest.resetAllMocks();
  });
  it('Notification component should render loading process', async () => {
    const { container } = render(<Notification />);
    const icon = container.querySelector(`[class="icon-loading"]`);
    expect(icon).toBeInTheDocument();
    await waitFor(() =>
      expect(AuthorizeService.authorize).toHaveBeenCalledTimes(1)
    );
  });

  it('Notification component should render <List/>', async () => {
    const { container } = render(<Notification />);
    const icon = container.querySelector(`[class="icon-loading"]`);
    await waitForElementToBeRemoved(icon);
    const item = container.querySelector('.notification-setting');
    expect(item).toBeInTheDocument();
  });
});
