import { handleCheckButton } from '../../helper/filterHelper';

const SpreadViewCell = ({
  spread,
  leagueName
}: {
  spread: number | string | undefined;
  leagueName: string;
}) => {
  return (
    <div className='spread-cell'>
      {spread ? (
        <p className='spread-cell__value'>{spread}%</p>
      ) : (
        <a
          className='spread__check'
          onClick={() => handleCheckButton(leagueName)}>
          Check
        </a>
      )}
    </div>
  );
};

export default SpreadViewCell;
