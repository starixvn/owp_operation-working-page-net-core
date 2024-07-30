import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';
import { TPermission } from '../../types/dataTableType';
import { buildColumnValueByColumnName } from '../EventPickingTableHelper';

import { render, fireEvent } from '@testing-library/react';
import CheckboxHeader from './CheckboxHeader';

jest.mock('../EventPickingTableHelper');

jest.mock('react-redux', () => ({
  useDispatch: jest.fn(),
  useSelector: jest.fn(),
  createSelectorHook: jest.fn(),
}));

useSelector.mockImplementation(fn=>fn({
  
  eventPickingTable:{
    isNoOfferAll: true, 
    isConfirmAll: true
  },
  pagePolicy:{
    permission:{
      HasWrite: true
    }
  }
}))
const onCheckChange= jest.fn();
buildColumnValueByColumnName.mockReturnValue({
  onCheckChange,
  checked: true
});
describe('Checkbox under test', () => {
  it('should call onchange', ()=>{
    const { container } = render(<CheckboxHeader classNameProps='check-box' columnKey='Checked'/>);
    
    const checkbox = container.querySelector('.check-box');
    fireEvent.click(checkbox, { target: { checked: false } });
    expect(onCheckChange).toBeCalled();
  });
});