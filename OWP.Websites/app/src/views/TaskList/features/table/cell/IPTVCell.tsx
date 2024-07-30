import { CellProps } from '../../../type/TTaskListTable';
import {
  UncontrolledTooltip,
  UncontrolledPopover,
  PopoverBody
} from 'reactstrap';

const IPTVCell = (props: CellProps) => {
  const { Televisions, Channel } = props.dataSource;
  const { rowIndex } = props;

  const renderChannel = () =>
    Channel && (
      <>
        <span
          className='icon-channel has-info'
          id={`channel-${rowIndex}`}></span>
        <UncontrolledTooltip target={`channel-${rowIndex}`}>
          {Channel}
        </UncontrolledTooltip>
        <UncontrolledPopover
          className='channel-popover'
          trigger='legacy'
          placement='right'
          target={`channel-${rowIndex}`}>
          <PopoverBody>{Channel}</PopoverBody>
        </UncontrolledPopover>
      </>
    );

  const renderTv = () =>
    Televisions.length > 0 &&
    Televisions.map((televisions, index) => {
      return (
        <span
          key={`${televisions}-${index}`}
          style={{ 'color': `${televisions.Color}` }}>
          {televisions.TVCode},
        </span>
      );
    });

  return (
    <>
      <div className='tv-cell'>
        {renderTv()}
        {renderChannel()}
      </div>
    </>
  );
};

export default IPTVCell;
