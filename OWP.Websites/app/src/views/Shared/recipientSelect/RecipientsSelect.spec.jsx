import { useState, useEffect } from 'react';
import { fireEvent, render } from '@testing-library/react';
import RecipientsSelect from './RecipientsSelect';
import { useDispatch } from 'react-redux';
const setState = jest.fn();
const mockedDispatch = jest.fn();
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  useEffect: jest.fn()
}));
jest.mock('react-redux', () => ({
  useDispatch: jest.fn()
}));
const defaultValue = [
  {
    groupName: null,
    nickName: 'sa_nickname',
    recipientId: 8,
    recipientType: 1,
    userName: 'sa'
  }
];
const mockOnChangeErrorSubmit = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);
useState.mockImplementation(init => [init, setState]);
useEffect.mockReturnValue(setState);
describe('RecipientsSelect component', () => {
  it('should called when click on change', () => {
    const { container } = render(
      <RecipientsSelect
        onChangeErrorSubmit={mockOnChangeErrorSubmit}
        defaultValue={defaultValue}
      />
    );
    const input = container.querySelector('.MuiInputBase-input');
    fireEvent.change(input, { target: { value: 'sa' } });
    fireEvent.click(input);

    expect(setState).toBeCalledTimes(4);
    expect(mockOnChangeErrorSubmit).toBeCalledTimes(2);
  });
  it('should called when on change with value.length < 2', () => {
    const { container } = render(
      <RecipientsSelect
        onChangeErrorSubmit={mockOnChangeErrorSubmit}
        defaultValue={defaultValue}
      />
    );
    const input = container.querySelector('.MuiInputBase-input');
    fireEvent.change(input, { target: { value: 's' } });

    expect(setState).toBeCalledTimes(9);
    expect(mockOnChangeErrorSubmit).toBeCalledTimes(3);
  });
});
