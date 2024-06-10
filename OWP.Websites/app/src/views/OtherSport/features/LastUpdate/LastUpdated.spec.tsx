import { render } from '@testing-library/react';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import '@testing-library/jest-dom';
import LastUpdated from './LastUpdated';

jest.mock('../../app/hooks');

const mockedDispatch = jest.fn();
const setUp = () => {
  (useAppDispatch as jest.Mock).mockReturnValue(mockedDispatch);
};

describe('test for LastUpdated component', () => {
  beforeEach(() => {
    setUp();
    jest.clearAllMocks();
    jest.resetModules();
  });

  it('render correct loading', () => {
    (useAppSelector as jest.Mock).mockImplementation(selectorFn =>
      selectorFn({
        lastUpdate: {
          loading: 'pending',
          entity: {
            date: '',
            username: ''
          }
        }
      })
    );
    const { container } = render(<LastUpdated />);

    expect(container.querySelector('.loading-process')).toBeInTheDocument();
  });

  it('render correct with date and username', () => {
    (useAppSelector as jest.Mock).mockImplementation(selectorFn =>
      selectorFn({
        lastUpdate: {
          loading: 'succeeded',
          entity: {
            date: '2021-01-01T01:00:00',
            username: 'sa'
          }
        }
      })
    );
    const { container } = render(<LastUpdated />);

    expect(container.querySelector('.last-update')).toBeInTheDocument();
    expect(container.querySelector('.last-update')?.textContent).toEqual(
      'Last Updated at January 01, 2021 01:00 AM (GMT+8) by sa'
    );
  });
});
