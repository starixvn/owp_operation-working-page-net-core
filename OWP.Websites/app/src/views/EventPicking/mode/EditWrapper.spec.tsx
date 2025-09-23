
import store from '../app/store';

import { useAppDispatch, useAppSelector } from '../app/hooks';
import EditWrapper from './EditWrapper';
import { fireEvent, render, screen } from '@testing-library/react';


jest.mock('../app/hooks', () => ({
  useAppDispatch: jest.fn(),
  useAppSelector: jest.fn()
}));

jest.mock('react-redux');

(useAppDispatch as jest.Mock).mockReturnValue(jest.fn());

store.getState = () => ({
  filter: {
    param: {
      date: new Date('2024-7-11')
    }
  },
  pagePolicy: {
    permission: {
      HasWrite: true
    }
  }
});

const onSetMode = jest.fn();
describe('Edit under test', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should render edit component', () => {

    useAppSelector.mockImplementation(fn => fn({
      mode: {
        mode: 'VIEW'
      }
    }));
    const { getByText } = render(<EditWrapper />);
    expect(getByText('Edit MB')).toBeInTheDocument()

  });
  it('should render view component', () => {

    useAppSelector.mockImplementation(fn => fn({
      mode: {
        mode: 'EDIT'
      }
    }));
    const { getByText } = render(<EditWrapper />);
    expect(getByText('Save')).toBeInTheDocument()

  });
});