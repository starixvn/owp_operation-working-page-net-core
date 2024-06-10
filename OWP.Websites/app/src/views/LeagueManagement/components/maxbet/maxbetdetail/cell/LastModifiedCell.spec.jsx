import { useCallback } from 'react';
import { render, fireEvent, screen } from '@testing-library/react';
import LastModifiedCell from './LastModifiedCell';
import '@testing-library/jest-dom';

describe('test LastModifiedCell component', () => {
  const props = {
    lastModifiedBy: 'abc'
  };
  afterAll(() => {
    jest.clearAllMocks();
  });
  it('render correct', () => {
    const { container, getByText } = render(<LastModifiedCell {...props} />);
    expect(getByText('abc')).toBeInTheDocument();
  });
});
