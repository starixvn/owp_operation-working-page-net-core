import { TableCell } from '../../TypeNative';

type Props = TableCell & {
  isManagement?: boolean;
};

const TraderCell = ({ value, isManagement }: Props) =>
  <div className={isManagement ? 'trader-management' : 'trader'}>
    <span>
      {isManagement ? <b style={{ color: '#1e88e5' }}>{value}</b> : value}
    </span>
  </div>;

export default TraderCell;
