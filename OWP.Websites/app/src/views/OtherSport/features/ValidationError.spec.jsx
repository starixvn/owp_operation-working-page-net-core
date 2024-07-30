import ValidationError from './ValidationError';
import CloseCircleIcon from 'mdi-react/CloseCircleIcon';
import { render } from '@testing-library/react';
import '@testing-library/jest-dom';

const props = {
  validationResults: [
    {
      row: 1,
      message: 'KOtime invalid'
    }
  ]
};

describe('ValidationError under test', () => {
  it('should match snapshot', () => {
    const { container } = render(<ValidationError {...props} />);
    expect(container).toMatchSnapshot();
  });

  it('should render error info', () => {
    const { container } = render(<ValidationError {...props} />);
    const error = container.querySelectorAll('.error-info');
    expect(error.length).toEqual(1);
  });

  it('should render table error', () => {
    const { container } = render(<ValidationError {...props} />);
    const table = container.querySelectorAll('.table');
    expect(table.length).toEqual(1);
  });
});
