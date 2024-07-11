
import { useAppDispatch } from '../../app/hooks';
import { useSelector } from 'react-redux';
import store from '../../app/store';
import MultipleFilterHeader from './MultipleFilterHeader';
import { fireEvent, render } from '@testing-library/react';


jest.mock('../../app/hooks',()=>({
  useAppDispatch: jest.fn(),
  useAppSelector: jest.fn(),
}));

jest.mock('../../app/store');

store.getState=()=>({
  eventPickingTable:{
    multipleFilter:{
      KO_Time:{
        options:[{
          value: 123,
          label: 'VN'
        }],
        selected:[{
          value: 123,
          label: 'VN'
        }],
        isFilter: true
      },
      MB:{
        options:[{
          value: 123,
          label: 'VN'
        }],
        selected:[{
          value: 123,
          label: 'VN'
        }],
        isFilter: true
      }
    }
  }
});

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
    const { container, getByText } = render(<MultipleFilterHeader columnKey='MB'/>);
   
    const element=container.getElementsByClassName('icon-filter')[0];
    fireEvent.click(element)
    fireEvent.click(getByText(/Search/));
    expect(useAppDispatch).toBeCalled();
  });

  it('should call clear filter', ()=>{
    const { container, getByText } = render(<MultipleFilterHeader columnKey='MB'/>);
   
    const element=container.getElementsByClassName('icon-filter')[0];
    fireEvent.click(element)
    fireEvent.click(getByText(/Clear All Filters/));
    expect(useAppDispatch).toBeCalled();
  });
});