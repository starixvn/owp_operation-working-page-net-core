import { fireEvent, render } from '@testing-library/react';
import Search from './Search';

describe('Search Component Tests', () => {
  it('should render <Search />', () => {
    const props = {
      submit: jest.fn()
    };
    const { container } = render(<Search {...props} />);
    expect(container).toMatchSnapshot();
  });

  it('test show Close button on search text change', () => {
    const mockSubmit = jest.fn();
    const props = {
      submit: mockSubmit
    };

    const { container } = render(<Search {...props} />);
    const inputComponent = container.querySelector('.search-field');
    fireEvent.change(inputComponent, { target: { value: 'Liam' } });
    fireEvent.keyPress(inputComponent, {
      key: 'Enter',
      code: 'Enter',
      charCode: 13
    });

    expect(mockSubmit).toBeCalledTimes(1);
    expect(mockSubmit).toBeCalledWith('Liam');
  });

  it('test show Close button on Click', () => {
    const mockSubmit = jest.fn();
    const props = {
      submit: mockSubmit
    };

    const { container } = render(<Search {...props} />);
    const inputComponent = container.querySelector('.search-field');
    fireEvent.change(inputComponent, { target: { value: 'Liam' } });
    const buttonCloseComponent = container.querySelector('.icon-clear');
    expect(buttonCloseComponent).toBeInTheDocument();
    fireEvent.click(buttonCloseComponent);

    expect(mockSubmit).toBeCalledTimes(1);
    expect(mockSubmit).toBeCalledWith('');
  });
});
