import { TableCell } from '../../TypeNative';
import OTPostShift from '../../../../../assets/svg-element/OTPostShift';
import OTPreShift from '../../../../../assets/svg-element/OTPreShift';

type Props = TableCell & {
  shiftCode?: string;
  otPreShift?: boolean;
  otPostShift?: boolean;
};

const ShiftCell = ({ value, shiftCode, otPreShift, otPostShift }: Props) => {

  return (
    <div className='shift-cell'>
      <span>
        {value} {shiftCode && <span>({shiftCode})</span>}
        {otPreShift && <OTPreShift />}
        {otPostShift && <OTPostShift />}
      </span>
    </div>
  );
};

export default ShiftCell;
