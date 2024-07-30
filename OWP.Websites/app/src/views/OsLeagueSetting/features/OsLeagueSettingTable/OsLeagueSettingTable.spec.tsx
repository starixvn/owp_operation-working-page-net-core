import { render, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import OsLeagueSettingTable from './OsLeagueSettingTable';

const { ResizeObserver } = window;

describe('OsLeagueSettingTable under test', () => {
  beforeEach(() => {
    //@ts-ignore
    delete window.ResizeObserver;
    window.ResizeObserver = jest.fn().mockImplementation(() => ({
      observe: jest.fn(),
      unobserve: jest.fn(),
      disconnect: jest.fn()
    }));
  });
    afterEach(() => {
    window.ResizeObserver = ResizeObserver;
    jest.restoreAllMocks();
  });
  it('render table correct', () => {
    const { getByText } = render(<OsLeagueSettingTable />);
    expect(getByText('#')).toBeInTheDocument();
  });

  it('render window resize', async () => {
    let aResizeEventListenerWasAddedToWindow = false;

    const originalMethod = window.addEventListener;
    const spy = jest.spyOn(window, 'addEventListener');
    spy.mockImplementation((...args) => {
      originalMethod(...args);
      const [eventType] = args;
      if (eventType === 'resize') aResizeEventListenerWasAddedToWindow = true;
    });

    render(<OsLeagueSettingTable />);
    await waitFor(() =>
      expect(aResizeEventListenerWasAddedToWindow).toBeTruthy()
    );
  });
});
