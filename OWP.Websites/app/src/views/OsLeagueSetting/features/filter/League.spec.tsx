import { render, act, fireEvent, screen } from '@testing-library/react';
import League from './League';
import useStore from '../../app/store';

const setFilter = jest.fn();

describe('<Category/> under test', () => {
  afterEach(() => {
    jest.resetAllMocks();
  });
  beforeEach(() => {
    useStore.setState({ setFilter });
  });

  it('should call setFilter when blur', () => {
    render(<League />);
    const input = screen.getByTestId('league')
    fireEvent.change(input, {
      target: {
        value: 'test'
      }
    });
    fireEvent.focusOut(input);
    expect(setFilter).toBeCalledTimes(1);
  });
});
