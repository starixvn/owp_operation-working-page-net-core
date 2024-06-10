import { AssignmentCell } from '../../TypeNative';

type Props = AssignmentCell;

const TimeLineCell = ({
  text,
  color,
  textColor,
  borderLeft
}: Props & { borderLeft: string }) => {
  return (
    <div
      className='item'
      style={{
        backgroundColor: color,
        flex: 1,
        borderLeft
      }}>
      <span
        style={{
          color: textColor,
          whiteSpace: 'nowrap'
        }}>
        {text}
      </span>
    </div>
  );
};

export default TimeLineCell;
