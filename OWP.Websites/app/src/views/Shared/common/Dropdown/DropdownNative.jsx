import PropTypes from 'prop-types';
import {
  UncontrolledButtonDropdown,
  DropdownMenu,
  DropdownItem,
  DropdownToggle
} from 'reactstrap';

const propTypes = {
  items: PropTypes.array.isRequired
};

const DropdownNative = props => {
  const { items } = props;
  const renderItems = () => {
    return items.map(item => {
      return (
        <DropdownItem
          key={item.title}
          onClick={item.handler}>
          {item.title}
        </DropdownItem>
      );
    });
  };
  return (
    <UncontrolledButtonDropdown>
      <DropdownToggle
        caret
        className='button-export'>
        Export
      </DropdownToggle>
      <DropdownMenu>
        {renderItems()}
      </DropdownMenu>
    </UncontrolledButtonDropdown>
  );
};

DropdownNative.propTypes = propTypes;

export default DropdownNative;
