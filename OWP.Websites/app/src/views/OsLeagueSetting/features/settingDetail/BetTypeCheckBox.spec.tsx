import { fireEvent, render, screen } from '@testing-library/react';
import BetTypeCheckBox from './BetTypeCheckBox';
import useQueryDropdownOption from '../../hook/useQueryDropdownOption';
import useStore from '../../app/store';
import { IBettypeType } from '../../typing/bettypeType';

jest.mock('../../hook/useQueryDropdownOption');

const onChange = jest.fn();
const props: IBettypeType = {
  value: ['1', '2'],
  disabled: false,
  onChange
};

const mockUseQueryDropdownOption = useQueryDropdownOption as jest.MockedFunction<typeof useQueryDropdownOption>;


describe('<BetDelay/> under test', () => {
  beforeEach(() => {
    jest.resetAllMocks();
  });

  it('should show loading when load options', () => {
    mockUseQueryDropdownOption.mockReturnValue({
      loading: true,
      options: []
    });
    const { container } = render(<BetTypeCheckBox {...props} />);
    expect(container.querySelector('.loading-process')).toBeInTheDocument();
  });

  it('should show error when has error', () => {
    useStore.setState({
      errorMessage: {
        leagueName: null,
        offerLines: null,
        maxbetHDP: null,
        maxbetOU: null,
        betTypes: 'This field is required',
        valid: false,
        betDelay: null
      }
    });
    mockUseQueryDropdownOption.mockReturnValue({
      loading: false,
      options: [{ value: 1, label: 'ABC1' }, { value: 2, label: 'ABC2' }]
    });
    const { getByText } = render(<BetTypeCheckBox {...props} />);
    expect(getByText('This field is required')).toBeInTheDocument();
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
    const { getByTestId } = render(<BetTypeCheckBox {...props} />);
    fireEvent.click(getByTestId('checkbox-0'), { target: { value: true } })
    expect(props.onChange).toHaveBeenCalledTimes(1);
  });
});
