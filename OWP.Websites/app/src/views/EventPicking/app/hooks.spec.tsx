import { useDispatch } from 'react-redux';
import {useAppDispatch} from './hooks';

jest.mock('react-redux');

useDispatch.mockReturnValue({});

describe('hooks under test', () => {
  it('should call useDispatch', ()=>{
    useAppDispatch();
    expect(useDispatch).toBeCalled();
  })
})