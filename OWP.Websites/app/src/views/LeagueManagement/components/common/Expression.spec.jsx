import { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { render, fireEvent } from '@testing-library/react';
import Expression from './Expression';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);

describe('Expression under test', () => {
  const props = {
    item: {
      expression: {}
    },
    index: 1,
    hasWrite: true,
    actionType: {}
  };
  it('render correct Expression component', () => {
    const { container } = render(<Expression {...props} />);
    expect(container.querySelector('.basic-multi-select')).toBeInTheDocument();
    fireEvent.blur(container.querySelector('#react-select-2-input'));
    expect(mockDispatch).toBeCalled();
  });
});
