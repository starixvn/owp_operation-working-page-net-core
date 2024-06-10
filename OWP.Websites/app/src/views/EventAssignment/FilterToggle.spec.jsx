import FilterToggle from './FilterToggle';
import { fireEvent, render } from '@testing-library/react';

describe('FilterToggle', () => {
  const onSelected = jest.fn();

  const defaultProps = {
    onSelected,
    selectedDate: new Date(),
    policy: {
      UserId: 8,
      Username: 'sa',
      FullBetTypes: {
        HasRead: true
      }
    },
    dailyWorkingShiftPolicy: {
      HasRead: true,
      HasWrite: true
    }
  };
  let windowSpy;

  beforeEach(() => {
    windowSpy = jest.spyOn(global, 'window', 'get');
  });

  afterEach(() => {
    windowSpy.mockRestore();
  });

  it('should redirect to Daily WS when click on button Working Shift', () => {
    window.open = jest.fn().mockReturnValue(window);
    window.focus = jest.fn();

    const { container } = render(<FilterToggle {...defaultProps} />);
    fireEvent.click(container.querySelector('.button-working-shift'));
    expect(window.focus).toBeCalledTimes(1);
  });

  it('should render toggle column when component load', () => {
    const { container } = render(<FilterToggle {...defaultProps} />);
    expect(container.firstChild).toMatchSnapshot()
  });
});
