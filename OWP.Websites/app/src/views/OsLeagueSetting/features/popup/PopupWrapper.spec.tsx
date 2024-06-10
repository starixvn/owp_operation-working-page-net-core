import { render, act, fireEvent, screen } from '@testing-library/react';
import PopupWrapper from './PopupWrapper';
import { popup } from '../../osLeague.json';
import useQueryDropdownOption from '../../hook/useQueryDropdownOption';
import useStore from '../../app/store';

jest.mock('../../../Shared/Service/Service');
jest.mock('../../hook/useQueryDropdownOption');


const mockUseQueryDropdownOption = useQueryDropdownOption as jest.MockedFunction<typeof useQueryDropdownOption>;

describe('<Category/> under test', () => {
  afterEach(() => {
    jest.resetAllMocks();
  });
  beforeEach(() => {
    mockUseQueryDropdownOption.mockReturnValue({
      loading: true,
      options: []
    });

  });

  it('should render popup add', () => {
    useStore.setState({
      popupType: popup.type.add as 'add'
    });
    const { getByText } = render(<PopupWrapper />);
    expect(getByText('Add Setting')).toBeInTheDocument();
  });
});
