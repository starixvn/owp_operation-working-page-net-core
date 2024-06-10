import { Fragment } from 'react';
import PropTypes from 'prop-types';
import { UncontrolledPopover, PopoverBody } from 'reactstrap';

const propTypes = {
  innerClassName: PropTypes.string.isRequired,
  rowIndex: PropTypes.number.isRequired,
  toolTipContent: PropTypes.string.isRequired,
  btnName: PropTypes.string.isRequired,
  placement: PropTypes.string
};

const TooltipButton = props => {
  const {
    innerClassName,
    rowIndex,
    toolTipContent,
    btnName,
    placement = 'right'
  } = props;

  return (
    <Fragment key={`${innerClassName}${rowIndex}`}>
      <button
        id={`${innerClassName}${rowIndex}`}
        className={`button-${innerClassName}`}>
        {btnName}
      </button>
      <UncontrolledPopover
        className='tooltip-button-popover'
        trigger='legacy'
        placement={placement}
        target={`${innerClassName}${rowIndex}`}>
        <PopoverBody>{toolTipContent}</PopoverBody>
      </UncontrolledPopover>
    </Fragment>
  );
};

TooltipButton.propTypes = propTypes;
export default TooltipButton;
