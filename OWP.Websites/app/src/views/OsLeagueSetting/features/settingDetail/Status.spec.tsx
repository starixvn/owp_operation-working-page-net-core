import { fireEvent, render, screen } from '@testing-library/react';
import Status from './Status';
import { IStatus } from '../../typing/type';

jest.mock('../../hook/useQueryDropdownOption');

const onChange = jest.fn();
const props: IStatus = {
  isActive: false,
  onChange
};

describe('<Status/> under test', () => {
  beforeEach(() => {
    jest.resetAllMocks();
  });

  it('should call onchange when change selected', () => {
    const { container } = render(<Status {...props} />);
    const input = container.querySelector('.MuiSwitch-input') as HTMLInputElement;
    input && fireEvent.click(input, { target: { value: true } });
    expect(props.onChange).toHaveBeenCalledTimes(1);
  });
});
