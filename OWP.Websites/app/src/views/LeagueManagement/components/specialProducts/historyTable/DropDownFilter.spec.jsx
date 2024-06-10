import { useCallback } from 'react';
import { useSelector, useDispatch } from 'react-redux';

import { render, fireEvent, screen } from '@testing-library/react';
import DropDownFilter from './DropDownFilter';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);

describe('test DropDownFilter component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });
  const props = {
    productId: 1
  };
  it('render correct loading component', () => {
    const { container } = render(<DropDownFilter {...props} />);

    const select = container.querySelector('#react-select-2-input');
    expect(select).toBeInTheDocument();

    fireEvent.change(select, {
      target: { value: 'Today' }
    });

    expect(mockDispatch).toBeCalled();
  });
});
