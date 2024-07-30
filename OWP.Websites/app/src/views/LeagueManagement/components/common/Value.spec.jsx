import { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { render, fireEvent } from '@testing-library/react';
import Value from './Value';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);

describe('Value under test', () => {
  const props = {
    item: {
      expression: {}
    },
    index: 1,
    hasWrite: true,
    actionType: {}
  };
  it('render correct Value component', () => {
    const { container } = render(<Value {...props} />);
    const input = container.querySelector('.input-field');
    expect(input).toBeInTheDocument();
    fireEvent.blur(input);
    expect(mockDispatch).toBeCalled();
  });
});
