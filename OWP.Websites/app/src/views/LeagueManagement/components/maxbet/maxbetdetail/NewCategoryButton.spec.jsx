import { useCallback } from 'react';
import Router from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import NewCategoryButton from './NewCategoryButton';
import '@testing-library/jest-dom';
import Service from '../../../Service';

jest.mock('react-redux', () => ({
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);

jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useParams: jest.fn()
}));
jest.spyOn(Router, 'useParams').mockReturnValue({ id: 1 });

jest.mock('../../../Service');

describe('test MaxbetDetail component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });

  it('should render NewCategoryButton component', () => {
    const { getByText } = render(<NewCategoryButton />);
    const button = getByText('New Category');
    expect(button).toBeInTheDocument();
  });

  it('should not handle click', () => {
    Service.addNewLeague.mockReturnValue({});
    const { getByText } = render(<NewCategoryButton />);
    const button = getByText('New Category');
    fireEvent.click(button);
    expect(mockDispatch).toBeCalledTimes(0);
  });
});
