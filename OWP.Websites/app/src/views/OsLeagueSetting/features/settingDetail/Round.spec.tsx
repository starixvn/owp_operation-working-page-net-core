import { fireEvent, render, screen } from '@testing-library/react';
import Round from './Round';
import { IFieldMultipleSelection } from '../../typing/type';
import useQueryDropdownOption from '../../hook/useQueryDropdownOption';
import useStore from '../../app/store';

jest.mock('../../hook/useQueryDropdownOption');

const onChange = jest.fn();
const props: IFieldMultipleSelection = {
  value: ['1', '2'],
  disabled: false,
  onChange
};

const mockUseQueryDropdownOption = useQueryDropdownOption as jest.MockedFunction<typeof useQueryDropdownOption>;


describe('<Round/> under test', () => {
  beforeEach(() => {
    jest.resetAllMocks();
  });

  it('should show loading when load options', () => {
    mockUseQueryDropdownOption.mockReturnValue({
      loading: true,
      options: []
    });
    const { container } = render(<Round {...props} />);
    expect(container.querySelector('.loading-process')).toBeInTheDocument();
  });

  it('should call onchange when change selected', () => {
    useStore.setState({
      errorMessage: {
        leagueName: null,
        offerLines: null,
        maxbetHDP: null,
        maxbetOU: null,
        betTypes: null,
        valid: true,
        betDelay: null
      }
    });
    mockUseQueryDropdownOption.mockReturnValue({
      loading: false,
      options: [{ value: 1, label: 'ABC1' }, { value: 2, label: 'ABC2' }]
    });
    const { container } = render(<Round {...props} />);
    const arrowIcon = container.querySelector('.dropdown-heading-dropdown-arrow') as Element;
    fireEvent.click(arrowIcon);
    fireEvent.click(screen.getByText('ABC1'));
    fireEvent.click(arrowIcon);
    expect(props.onChange).toHaveBeenCalledTimes(1);
  });
});
