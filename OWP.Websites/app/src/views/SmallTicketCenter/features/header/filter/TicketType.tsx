import TreeSelect from 'rc-tree-select';
import 'rc-tree-select/assets/index.css';
import store from '../../../app/storeNative';
import { useState } from 'react';
import { useAppDispatch } from '../../../app/hooksNative';
import { setFilterParam } from './filterSlice';

const TicketType = () => {
  const dispatch= useAppDispatch();
  
  const {ticketTypeOptions, ticketTypeSelected }= store.getState().filter.entities.multipleSelect;
  const [selected, setSelected] = useState(ticketTypeSelected);
  const treeDataSimpleMode = {
    id: 'key'
  };

  const handleOnChange = (value: Array<string>) => {
    setSelected(value);
    dispatch(setFilterParam({
    key: 'ticketTypeId',
     value: value.join(',')
    }));
  };

 
  return (
    <div className='ticket-type-dropdown'>
    <div className='show-by-filter-control'>
      <div className='label'>Ticket Type - Subtype</div>
      <TreeSelect
        treeData={ticketTypeOptions}
        value={selected}
        treeCheckable
        maxTagCount={1}
        treeDefaultExpandAll
        onChange={handleOnChange}
        treeDataSimpleMode={treeDataSimpleMode}
        treeNodeLabelProp='showTitle'
    />
    </div>
  </div>
  );
};



export default TicketType;