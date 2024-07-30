import { fireEvent, render, screen } from '@testing-library/react';
import Region from './Region';
import { IFieldSelection } from '../../typing/type';
import useQueryDropdownOption from '../../hook/useQueryDropdownOption';

jest.mock('../../hook/useQueryDropdownOption');

const onChange = jest.fn();
const props: IFieldSelection = {
  value: 1,
  disabled: false,
  onChange
};

const mockUseQueryDropdownOption = useQueryDropdownOption as jest.MockedFunction<typeof useQueryDropdownOption>;


describe('<Region/> under test', () => {
  beforeEach(() => {
    jest.resetAllMocks();
  });

  it('should show loading when load options', () => {
    mockUseQueryDropdownOption.mockReturnValue({
      loading: true,
      options: []
    });
    const { container } = render(<Region {...props} />);
    expect(container.querySelector('.loading-process')).toBeInTheDocument();
  });

  it('should call onchange when change selected', () => {
    mockUseQueryDropdownOption.mockReturnValue({
      loading: false,
      options: [{ value: 1, label: 'ABC1' }, { value: 2, label: 'ABC2' }]
    });
    const { container } = render(<Region {...props} />);
    const dropdown = container.querySelector('input');

    dropdown && fireEvent.focus(dropdown);
    dropdown && fireEvent.keyDown(dropdown, { key: 'ArrowDown', code: 40 });
    fireEvent.click(screen.getByText('ABC2'));
    expect(props.onChange).toHaveBeenCalledTimes(1);
  });
});
