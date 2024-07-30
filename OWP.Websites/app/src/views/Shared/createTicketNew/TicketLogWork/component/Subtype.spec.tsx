import { render, fireEvent } from '@testing-library/react';
import Subtype from './Subtype';
import { SubTypeOption } from '../Type';

describe('Subtype', () => {
  const options: SubTypeOption[] = [
    { value: 'pending', label: 'Pending' },
    { value: 'completed', label: 'Completed' },
    { value: 'cancelled', label: 'Cancelled' }
  ];
  const selected = { value: 'pending', label: 'Pending' };
  const handleChangeSubType = jest.fn();

  it('should call handleOnChange when selecting a status', () => {
    const { container, getByText } = render(
      <Subtype
        options={options}
        selected={selected}
        handleChangeSubType={handleChangeSubType}
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

    expect(handleChangeSubType).toHaveBeenCalledTimes(1);
  });
});
