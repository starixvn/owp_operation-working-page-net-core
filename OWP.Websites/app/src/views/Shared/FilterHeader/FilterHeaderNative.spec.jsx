import { fireEvent, render, screen } from '@testing-library/react';
import FilterHeaderNative from './FilterHeaderNative';
const onTextChange = jest.fn();
const onClear = jest.fn();
const defaultProps = {
  onTextChange: onTextChange,
  onClear: onClear,
  name: 'HDP',
  title: 'HDP',
  keyword: 'hi'
};
const mockOnSubmit = jest.fn();
describe('FilterHeaderNative', () => {
  it('should submit when keypress Enter or submit click', async () => {
    jest.resetAllMocks();
    const props = {
      name: 'HDP',
      title: 'HDP',
      onSubmit: mockOnSubmit
    };
    const component = render(
      <FilterHeaderNative {...defaultProps} {...props} />
    );
    const spanFilter = component.container.querySelector('.icon-filter');
    fireEvent.click(spanFilter);
    await screen.findByText('Search');

    fireEvent.click(screen.queryByText('Search'));

    expect(
      component.container.querySelector('.title-header')
    ).toBeInTheDocument();
    expect(mockOnSubmit).toHaveBeenCalledTimes(1);
  });

  it('should render textbox when click filter icon', async () => {
    jest.resetAllMocks();
    const props = {
      name: 'HDP',
      title: 'HDP',
      onSubmit: mockOnSubmit
    };
    const { container } = render(
      <FilterHeaderNative {...defaultProps} {...props} />
    );

    const spanFilter = container.querySelector('.icon-filter');

    fireEvent.click(spanFilter);
    await screen.findByText('Search');

    expect(spanFilter).toBeInTheDocument();
    expect(screen.queryByText('Search')).toBeInTheDocument();
  });

  it('should render hide textbox when click close button', async () => {
    jest.resetAllMocks();
    const props = {
      name: 'HDP',
      title: 'HDP',
      onSubmit: mockOnSubmit
    };

    const { container } = render(
      <FilterHeaderNative {...defaultProps} {...props} />
    );

    const spanFilter = container.querySelector('.icon-filter');
    fireEvent.click(spanFilter);
    await screen.findByText('Search');
    const closeButton = container.querySelector('.close-popup');
    fireEvent.click(closeButton);

    expect(screen.queryByText('Search')).toBeNull();
  });

  it('should render value when textbox change', async () => {
    jest.resetAllMocks();
    const props = {
      name: 'HDP',
      title: 'HDP',
      onSubmit: mockOnSubmit
    };
    const component = render(
      <FilterHeaderNative {...defaultProps} {...props} />
    );

    const spanFilter = component.container.querySelector('.icon-filter');
    fireEvent.click(spanFilter);
    await screen.findByText('Search');
    const inputSearch = component.container.querySelector('.input-field');
    const event = {
      target: { value: 'the-value' }
    };

    fireEvent.change(inputSearch, event);

    expect(inputSearch.value).toEqual('the-value');
  });
});
