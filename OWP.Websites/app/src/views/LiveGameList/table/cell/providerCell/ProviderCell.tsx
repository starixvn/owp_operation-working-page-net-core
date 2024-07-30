import { useRef } from 'react';
import { CellProps } from '../../../type/TableType';
import PhoneIcon from '@mui/icons-material/Phone';
import FormatListBulletedIcon from '@mui/icons-material/FormatListBulleted';
import {
  UncontrolledTooltip,
  UncontrolledPopover,
  PopoverBody
} from 'reactstrap';

const ProviderCell = (props: CellProps): JSX.Element => {
  const ref = useRef(null);
  const { rowIndex, dataSource } = props;
  const { Provider, WatchBet, Phone, OtherProvider } = dataSource[rowIndex];

  return (
    <div className='provider-cell'>
      {Provider && <span className='provider'>{Provider}</span>}
      {WatchBet && <span className='watch-bet'>{WatchBet}</span>}
      {Phone && (
        <span className='phone'>
          <PhoneIcon />
          {Phone}
        </span>
      )}
      {OtherProvider && (
        <>
          <span className='other-provider' ref={ref}>
            <FormatListBulletedIcon />
          </span>
          <UncontrolledTooltip target={ref}>
            Other Provider List
          </UncontrolledTooltip>
          <UncontrolledPopover
            className='channel-popover'
            trigger='legacy'
            placement='right'
            target={ref}>
            <PopoverBody>{OtherProvider}</PopoverBody>
          </UncontrolledPopover>
        </>
      )}
    </div>
  );
};

export default ProviderCell;
