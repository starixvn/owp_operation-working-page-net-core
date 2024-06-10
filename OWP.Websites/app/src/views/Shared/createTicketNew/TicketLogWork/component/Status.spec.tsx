import { render, screen, fireEvent } from '@testing-library/react';
import Status from './Status';
import { StatusOption } from '../Type';

describe('Status', () => {
  const options: StatusOption[] = [
    { value: 'pending', label: 'Pending' },
    { value: 'completed', label: 'Completed' },
    { value: 'cancelled', label: 'Cancelled' }
  ];
  const selected = { value: 'pending', label: 'Pending' };
  const handleOnChange = jest.fn();

  it('should call handleOnChange when selecting a status', () => {
    const { container, getByText } = render(
      <Status
        options={options}
        selected={selected}
        handleOnChange={handleOnChange}
      />
    );

    const selectElement = container.querySelector('input');
    if (selectElement) {
      fireEvent.focus(selectElement);
      fireEvent.keyDown(selectElement, {
        key: 'ArrowDown',
        code: 40
      });

      fireEvent.click(getByText('Completed'));
    }

    expect(handleOnChange).toHaveBeenCalledTimes(1);
  });
});
