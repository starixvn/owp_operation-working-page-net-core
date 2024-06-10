import { useState } from 'react';
import PropTypes from 'prop-types';
import TreeSelect from 'rc-tree-select';
import 'rc-tree-select/assets/index.css';
const propTypes = {
  onSelectedChange: PropTypes.func.isRequired,
  options: PropTypes.array.isRequired,
  defaultValue: PropTypes.array.isRequired
};

const TicketTypeFilter = props => {
  const { onSelectedChange, options, defaultValue } = props;
  const [valueOption, setValueOption] = useState([...defaultValue]);
  const treeDataSimpleMode = {
    id: 'key'
  };

  const handleOnChange = value => {
    setValueOption(value);
    onSelectedChange(value);
  };

  return (
    <TreeSelect
      treeData={options}
      value={valueOption}
      treeCheckable
      maxTagCount={1}
      treeDefaultExpandAll
      onChange={handleOnChange}
      treeDataSimpleMode={treeDataSimpleMode}
      treeNodeLabelProp='showTitle'
    />
  );
};

TicketTypeFilter.propTypes = propTypes;

export default TicketTypeFilter;
