import { Fragment } from 'react';
import PropTypes from 'prop-types';
import { UncontrolledTooltip } from 'reactstrap';

const propTypes = {
  innerClassName: PropTypes.string.isRequired,
  rowIndex: PropTypes.number.isRequired,
  toolTipContent: PropTypes.oneOfType([
    PropTypes.string,
    PropTypes.object
  ]),
  text: PropTypes.oneOfType([
    PropTypes.string,
    PropTypes.object
  ]),
  placement: PropTypes.string
};

const TooltipText = props => {
  const {
    innerClassName,
    rowIndex,
    toolTipContent,
    text,
    placement = 'right'
  } = props;

  return (
    <Fragment key={`${innerClassName}${rowIndex}`}>
      <div id={`${innerClassName}${rowIndex}`} className={`${innerClassName}`}>
        {text}
      </div>
      <UncontrolledTooltip
        className='tooltip-text'
        placement={placement}
        target={`${innerClassName}${rowIndex}`}>
        {toolTipContent}
      </UncontrolledTooltip>
    </Fragment>
  );
};

TooltipText.propTypes = propTypes;
export default TooltipText;
