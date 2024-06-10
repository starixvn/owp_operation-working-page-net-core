import { fireEvent, render, screen, waitFor } from '@testing-library/react';
import MessageBox from './MessageBox';
import AuthorizeService from '../Authorize/Service';

jest.mock('../Authorize/Service');

describe('MessageBox Component Tests', () => {
  it('should render <MessageBox />', () => {
    const policy = { HasRead: true, HasWrite: false };
    AuthorizeService.authorize.mockResolvedValue(policy);
    const component = render(<MessageBox />);
    waitFor(() => {
      expect(component).toMatchSnapshot();
    });
  });

  it('show only tab Assignee if not have permisson Read', () => {
    const policy = { HasRead: false, HasWrite: false };
    AuthorizeService.authorize.mockResolvedValue(policy);
    const { container } = render(<MessageBox />);
    const navItemComponent = container.querySelector('.message-box');
    expect(navItemComponent).toBeInTheDocument();
  });
  it('test toggle tab click', async () => {
    const policy = { HasRead: true, HasWrite: false };
    AuthorizeService.authorize.mockResolvedValue(policy);
    const { container } = render(<MessageBox />);
    await screen.findByText('My Section Message');
    fireEvent.click(screen.getByText('My Message'));
    const activeNavLink = container.querySelector(`[class="active nav-link"]`);
    const innerHTML = activeNavLink.innerHTML;
    const navItemComponent = container.querySelectorAll('.nav-item');
    expect(navItemComponent).toHaveLength(3);
    expect(innerHTML).toEqual('My Message');
  });
});
