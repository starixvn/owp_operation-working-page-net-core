
import store from '../app/store';

import { useAppSelector } from '../app/hooks';
import useStableDispatch from '../../Shared/customHook/useStableDispatch';
import SumProviderTable from './SumProviderTable';
import { render } from '@testing-library/react';

jest.mock('../../Shared/customHook/useStableDispatch',);

jest.mock('../app/hooks',()=>({
  useAppSelector: jest.fn(),
}));

jest.mock('react-redux', () => ({
  useDispatch: jest.fn(),
  useSelector: jest.fn(),
  createSelectorHook: jest.fn(),
}));

(useStableDispatch as jest.Mock).mockReturnValue(jest.fn());

useAppSelector.mockImplementation(fn=>fn({
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
  },
  sumProvider:{
    sumTable:[{
      countDate: new Date(),
      count: 123,
      periodCount: 456
    }]
  }
}))

store.getState=()=>({
  filter:{
    param:{
      date: new Date('2024-7-11')
    }
  }
});


describe('Sum provider under test', () => {
  beforeEach(()=>{
    jest.clearAllMocks();
  });

  it('should render component', ()=>{
    const { container } = render(<SumProviderTable/>);
    expect(container.querySelector('.sum-provider-table')).toBeInTheDocument();
  });
});