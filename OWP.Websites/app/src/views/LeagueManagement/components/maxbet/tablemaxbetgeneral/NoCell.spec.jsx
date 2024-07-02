import { useCallback } from 'react';
import { render, fireEvent, screen } from '@testing-library/react';

import NoCell from './NoCell';
import '@testing-library/jest-dom';

describe('test NoCell component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });

  it('render correct component', () => {
    const props = {
      rowIndex: 1
    };
    const { getByText } = render(<NoCell {...props} />);

    expect(getByText('2')).toBeInTheDocument();
  });
});
