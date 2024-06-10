import { fireEvent, render } from '@testing-library/react';
import FilterHeader from './FilterHeader';

const onSubmit = jest.fn();
const onTextChange = jest.fn();
const onClear = jest.fn();
const defaultProps = {
  onSubmit: onSubmit,
  onTextChange: onTextChange,
  onClear: onClear,
  name: 'HDP',
  title: 'HDP',
  keyword: '',
  columnName: ''
};
const props = { name: 'HDP', title: 'HDP' };

describe('FilterHeader', () => {
  beforeEach(() => {
    jest.useFakeTimers('legacy');
  });
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render when component load', () => {
    const { container } = render(<FilterHeader {...defaultProps} />);
    const titleHeader = container.querySelector('.title-header');
    expect(titleHeader).toBeInTheDocument();
  });

  it('should render textbox when click filter icon', () => {
    const { container } = render(
      <FilterHeader {...defaultProps} {...{ keyword: 'abc' }} />
    );
    const spanFilter = container.querySelector('.icon-filter');
    expect(spanFilter).toBeInTheDocument();
    expect(container.querySelector('.filter-popup')).toBeNull;

    fireEvent.click(spanFilter);
    expect(container.querySelector('.filter-popup')).toBeInTheDocument;
    const inputSearch = container.querySelector('.input-field');
    expect(inputSearch).toBeInTheDocument;
  });

  it('should not render popup when click close button', () => {
    const { container } = render(<FilterHeader {...defaultProps} {...props} />);
    fireEvent.click(container.querySelector('.icon-filter'));

    const closeButton = container.querySelector('.close-popup');
    expect(closeButton).toBeInTheDocument();
    fireEvent.click(closeButton);
    expect(container.querySelector('.filter-popup')).toBeNull;
  });

  it('should render value, when textbox change', () => {
    const { container } = render(<FilterHeader {...defaultProps} {...props} />);
    fireEvent.click(container.querySelector('.icon-filter'));
    const inputSearch = container.querySelector('.input-field');
    fireEvent.change(inputSearch, { target: { value: 'new value' } });
    expect(inputSearch.value).toBe('new value');
  });

  it('should call onSubmit, when key press Enter', () => {
    const { container } = render(<FilterHeader {...defaultProps} {...props} />);
    fireEvent.click(container.querySelector('.icon-filter'));
    const inputSearch = container.querySelector('.input-field');
    fireEvent.change(inputSearch, { target: { value: 'value' } });
    fireEvent.keyPress(inputSearch, { key: 'Enter', code: 13, charCode: 13 });
    expect(onSubmit).toHaveBeenCalledTimes(1);
  });

  it('should call onClear, when button clear click', () => {
    const { container } = render(<FilterHeader {...defaultProps} {...props} />);
    fireEvent.click(container.querySelector('.icon-filter'));
    fireEvent.click(container.querySelector('.clear-all'));
    expect(onClear).toHaveBeenCalledTimes(1);
  });

  it('should call onSubmit when click search button', () => {
    const { container } = render(<FilterHeader {...defaultProps} {...props} />);
    fireEvent.click(container.querySelector('.icon-filter'));
    const inputSearch = container.querySelector('.input-field');
    fireEvent.change(inputSearch, { target: { value: 'new value' } });
    fireEvent.click(container.querySelector('.btn-search'));
    expect(onSubmit).toHaveBeenCalledTimes(1);
  });
});
