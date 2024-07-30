
import { useAppDispatch } from '../../app/hooks';
import { useSelector } from 'react-redux';
import FilterHeader from './FilterHeader';
import { fireEvent, render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';

jest.mock('../../app/hooks',()=>({
  useAppDispatch: jest.fn(),
  useAppSelector: jest.fn(),
}));

jest.mock('react-redux', () => ({
  useDispatch: jest.fn(),
  useSelector: jest.fn(),
  createSelectorHook: jest.fn(),
}));

(useAppDispatch as jest.Mock).mockReturnValue(jest.fn());

useSelector.mockImplementation(fn=>fn({
  eventPickingTable:{
    isNoOfferAll: true, 
    isConfirmAll: true,
    filterState:[],
    valueFilterAllColumn:{
      League:'League',
    }
  },
  pagePolicy:{
    permission:{
      HasWrite: true
    }
  }
}))

describe('Checkbox under test', () => {
  beforeEach(()=>{
    jest.clearAllMocks();
  });

  it('should call filter', ()=>{
    const { container, getByText } = render(<FilterHeader columnKey='League'/>);
    const element=container.getElementsByClassName('icon-filter ')[0];
    fireEvent.click(element);
    fireEvent.click(getByText(/Search/));
    expect(useAppDispatch).toBeCalled();
  });

  it('should call clear filter', ()=>{
    const { container, getByText } = render(<FilterHeader columnKey='League'/>);
    const element=container.getElementsByClassName('icon-filter ')[0];
    fireEvent.click(element);
    fireEvent.click(getByText(/Clear All Filters/));
    expect(useAppDispatch).toBeCalled();
  });

  it('should call filter by click enter', ()=>{
    const { container, getByText } = render(<FilterHeader columnKey='League'/>);
    const element=container.getElementsByClassName('icon-filter ')[0];
    fireEvent.click(element);
    userEvent.keyboard("{enter}");
    expect(useAppDispatch).toBeCalled();
  });
});