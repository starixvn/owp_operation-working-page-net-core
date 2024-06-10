import { PureComponent, Fragment } from 'react';
import {
  Button,
  UncontrolledPopover,
  PopoverBody
} from 'reactstrap';
import { v4 as uuidv4 } from 'uuid';
import {
  PhoneIcon
} from 'mdi-react';
import PropTypes from 'prop-types';

class ProviderCell extends PureComponent {
  render() {
    const { data, rowIndex } = this.props;
    const { OtherProvider, Phone } = data[rowIndex];
    const buttonIdOtherProvider = `OtherProvider${rowIndex}`;
    const buttonIdPhone = `Phone${rowIndex}`;
    return (
      <div className='provider-cell'>
        {data[rowIndex].Provider && (
          <p className='runner'>{data[rowIndex].Provider}</p>
        )}
        {OtherProvider && (
          <Fragment key={`OtherProvider${uuidv4()}${rowIndex}`}>
            <Button id={buttonIdOtherProvider} className='button-provider'>
              Other Provider
            </Button>
            <UncontrolledPopover
              className='channel-popover'
              trigger='legacy'
              placement='right'
              target={buttonIdOtherProvider}>
              <PopoverBody>{OtherProvider}</PopoverBody>
            </UncontrolledPopover>
          </Fragment>
        )}
        {Phone && (
          <Fragment key={`Phone${uuidv4()}${rowIndex}`}>
            <Button id={buttonIdPhone} className='button-provider'>
              <PhoneIcon size={11} /> Phone
            </Button>
            <UncontrolledPopover
              className='channel-popover'
              trigger='legacy'
              placement='right'
              target={buttonIdPhone}>
              <PopoverBody>{Phone}</PopoverBody>
            </UncontrolledPopover>
          </Fragment>
        )}
      </div>
    );
  }
}

const propTypesBase = {
  data: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

ProviderCell.propTypes = propTypesBase;

export default ProviderCell;
