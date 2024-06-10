import { PureComponent, Fragment } from 'react';
import {
  Button,
  UncontrolledPopover,
  PopoverBody
} from 'reactstrap';
import { v4 as uuidv4 } from 'uuid';
import PropTypes from 'prop-types';


class TVCell extends PureComponent {
  render() {
    const { data, rowIndex } = this.props;
    const { Televisions, Channel, WatchBet } = data[rowIndex];
    return (
      <div className='tv-cell'>
        {Televisions.length > 0 && (
          <div>
            {Televisions.map(t => (
              <span key={`${uuidv4()}`} style={{ color: t.Color }}>
                {t.TVCode}
              </span>
            )).reduce((prev, curr) => [prev, ', ', curr])}
          </div>
        )}

        {Channel && (
          <Fragment key={`Channel${uuidv4()}${rowIndex}`}>
            <Button id={`Channel${rowIndex}`} className='button-channel'>
              IPTV/Channel
            </Button>
            <UncontrolledPopover
              className='channel-popover'
              trigger='legacy'
              placement='right'
              target={`Channel${rowIndex}`}>
              <PopoverBody>{Channel}</PopoverBody>
            </UncontrolledPopover>
          </Fragment>
        )}
        {WatchBet && (
          <Fragment key={`WatchBet${uuidv4()}${rowIndex}`}>
            <Button id={`WatchBet${rowIndex}`} className='button-watchbet'>
              Streaming
            </Button>
            <UncontrolledPopover
              className='watchbet-popover'
              trigger='legacy'
              placement='right'
              target={`WatchBet${rowIndex}`}>
              <PopoverBody>{WatchBet}</PopoverBody>
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

TVCell.propTypes = propTypesBase;

export default TVCell;
