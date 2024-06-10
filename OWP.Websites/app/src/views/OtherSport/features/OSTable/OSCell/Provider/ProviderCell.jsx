import { Fragment } from 'react';
import PropTypes from 'prop-types';
import { TooltipButton, TooltipText } from '../../../../../Shared/common/Tooltip';
import { v4 as uuidv4 } from 'uuid';
import {
  Button,
  UncontrolledPopover,
  PopoverBody
} from 'reactstrap';
import {
  PhoneIcon
} from 'mdi-react';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};
const ProviderCell = props => {
  const { dataSource, rowIndex } = props;
  const {
    provider,
    otherProvider,
    service,
    reporter
  } = dataSource[rowIndex];

  const buttonIdPhone = `Phone${rowIndex}`;

  return (
    <div className='provider-cell'>
      {provider && (
        <TooltipText
          text={provider}
          toolTipContent={provider}
          innerClassName='provider'
          rowIndex={rowIndex}
        />
      )}
      {service && <div className='service'>{service}</div>}

      {otherProvider && (
        <TooltipButton
          rowIndex={rowIndex}
          innerClassName='other-provider'
          btnName='Other Provider'
          toolTipContent={otherProvider}
        />
      )}
      {reporter && (
        <Fragment key={`Phone${uuidv4()}${rowIndex}`}>
          <Button id={buttonIdPhone} className='button-provider'>
            <PhoneIcon size={11} /> Phone
          </Button>
          <UncontrolledPopover
            className='channel-popover'
            trigger='legacy'
            placement='right'
            target={buttonIdPhone}>
            <PopoverBody>{reporter}</PopoverBody>
          </UncontrolledPopover>
        </Fragment>
      )}
    </div>
  );
};

ProviderCell.propTypes = propTypes;
export default ProviderCell;
