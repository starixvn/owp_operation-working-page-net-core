import PhoneIcon from '@mui/icons-material/Phone';
import FormatListBulletedIcon from '@mui/icons-material/FormatListBulleted';
import {
  UncontrolledTooltip,
  UncontrolledPopover,
  PopoverBody
} from 'reactstrap';
import { CellProps } from '../../../type/TTaskListTable';

const ProviderCell = (props: CellProps): JSX.Element => {
  const { rowIndex, dataSource } = props;
  const { Provider, WatchBet, Phone, OtherProvider } = dataSource;

  return (
    <>
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
            <span className='other-provider' id={`other-provider-${rowIndex}`}>
              <FormatListBulletedIcon />
            </span>
            <UncontrolledTooltip target={`other-provider-${rowIndex}`}>
              Other Provider List
            </UncontrolledTooltip>
            <UncontrolledPopover
              className='channel-popover'
              trigger='legacy'
              placement='right'
              target={`other-provider-${rowIndex}`}>
              <PopoverBody>{OtherProvider}</PopoverBody>
            </UncontrolledPopover>
          </>
        )}
      </div>
    </>
  );
};

export default ProviderCell;
