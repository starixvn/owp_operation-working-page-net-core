import { handleCheckButton } from '../../helper/filterHelper';

const SpreadViewCell = ({
  spread,
  leagueName,
  isChildMatch
}: {
  spread: number | string | undefined;
  leagueName: string;
  isChildMatch: boolean;
}) => {
  return (
    <div className='spread-cell'>
      {spread ? (
        <p className='spread-cell__value'>{spread}%</p>
      ) : (
        <>
          {!isChildMatch && (
            <a
              className='spread__check'
              onClick={() => handleCheckButton(leagueName)}>
              Check
            </a>
          )}
        </>
      )}
    </div>
  );
};

export default SpreadViewCell;
