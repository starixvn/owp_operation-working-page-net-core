import { activeIcon, inactiveIcon } from '../../TableSchema.json';

type StatusProps = {
  isActive: boolean;
};

const StatusIcon = (props: StatusProps): JSX.Element => {
  const { isActive } = props;
  return (
    <>
      {isActive ? (
        <svg
          width='16'
          height='16'
          viewBox='0 0 16 16'
          fill='none'
          xmlns='http://www.w3.org/2000/svg'>
          <path d={activeIcon} fill='#46b46e' />
        </svg>
      ) : (
        <svg
          width='16'
          height='16'
          viewBox='0 0 16 16'
          fill='none'
          xmlns='http://www.w3.org/2000/svg'>
          <path d={inactiveIcon} fill='#AB383B' />
        </svg>
      )}
    </>
  );
};

export default StatusIcon;
