import { Fragment } from 'react';
import { Button, UncontrolledPopover, PopoverBody } from 'reactstrap';
import { TCellModel } from '../../type/type';

const IPTVCell = (props: TCellModel) => {
  const { dataSource, rowIndex } = props;
  const { Televisions, Channel, WatchBet } = dataSource[rowIndex];
  return (
    <div className='tv-cell'>
      {Televisions.length > 0 && (
        <div>
          {Televisions.map((t, index) => (
            <span key={`${t.TVCode}-${index}`} style={{ color: t.Color }}>
              {t.TVCode}
            </span>
          )).reduce((prev, curr) => [prev, ', ', curr])}
        </div>
      )}

      {Channel && (
        <Fragment key={`Channel${rowIndex}`}>
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
        <Fragment key={`WatchBet${rowIndex}`}>
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
};

export default IPTVCell;
