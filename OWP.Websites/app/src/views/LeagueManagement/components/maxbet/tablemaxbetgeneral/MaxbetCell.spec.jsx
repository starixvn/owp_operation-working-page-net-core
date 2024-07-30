import { useCallback } from 'react';
import { render, fireEvent, screen } from '@testing-library/react';

import MaxbetCell from './MaxbetCell';
import '@testing-library/jest-dom';

describe('test MaxbetCell component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });

  it('render correct component', () => {
    const props = {
      value: 'abcdefa'
    };
    const { getByText } = render(<MaxbetCell {...props} />);

    expect(getByText('abcdefa')).toBeInTheDocument();
  });
});
