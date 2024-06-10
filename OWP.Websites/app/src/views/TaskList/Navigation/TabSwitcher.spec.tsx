import { render, screen, fireEvent } from '@testing-library/react';
import TabSwitcher from './TabSwitcher';
import { getHeaderDateTime } from '../helper/tasklistHelper';
import { Provider } from 'react-redux';
import store from '../app/storeNative';

jest.mock('../helper/tasklistHelper', () => ({
  getHeaderDateTime: jest.fn()
}));

describe('TabSwitcher', () => {
  beforeEach(() => {
    const today = '2023-05-04';
    const yesterday = '2023-05-03';
    (getHeaderDateTime as unknown as jest.Mock).mockReturnValue({
      today,
      yesterday
    });
  });
  it('should render tab headers with correct dates', () => {
    render(
      <Provider store={store}>
        <TabSwitcher />
      </Provider>
    );
    const todayTabHeader = screen.getByText('Today (May 04)');
    const yesterdayTabHeader = screen.getByText('Yesterday (May 03)');
    expect(todayTabHeader).toBeInTheDocument();
    expect(yesterdayTabHeader).toBeInTheDocument();
  });

  it('should switch tabs when clicking on headers', () => {
    render(
      <Provider store={store}>
        <TabSwitcher />
      </Provider>
    );
    const todayTabHeader = screen.getByText('Today (May 04)');
    const yesterdayTabHeader = screen.getByText('Yesterday (May 03)');
    fireEvent.click(yesterdayTabHeader);
    expect(todayTabHeader).not.toHaveClass('active');
    expect(yesterdayTabHeader).toHaveClass('active');
  });
});
