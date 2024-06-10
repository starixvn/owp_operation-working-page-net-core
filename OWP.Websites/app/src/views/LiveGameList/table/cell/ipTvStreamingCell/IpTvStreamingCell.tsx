import { useRef } from 'react';
import { CellProps, Television } from '../../../type/TableType';
import {
  UncontrolledTooltip,
  UncontrolledPopover,
  PopoverBody
} from 'reactstrap';

const IpTvStreamingCell = (props: CellProps): JSX.Element => {
  const ref = useRef(null);
  const { rowIndex, dataSource } = props;
  const { Televisions, Channel } = dataSource[rowIndex];
  const buildTextTv = (television: Television, index: number) => {
    return index === Televisions.length - 1
      ? television.TVCode
      : `${television.TVCode}, `;
  };

  return (
    <div className='tv-cell'>
      {Televisions.length > 0 &&
        Televisions.map((television: Television, index) => {
          return (
            <span
              key={`${television}-${index}`}
              style={{ 'color': `${television.Color}` }}>
              {buildTextTv(television, index)}
            </span>
          );
        })}
      {Channel && (
        <>
          <span className='icon-channel has-info' ref={ref}></span>
          <UncontrolledTooltip target={ref}>Channel</UncontrolledTooltip>
          <UncontrolledPopover
            className='channel-popover'
            trigger='legacy'
            placement='right'
            target={ref}>
            <PopoverBody>{Channel}</PopoverBody>
          </UncontrolledPopover>
        </>
      )}
    </div>
  );
};

export default IpTvStreamingCell;
