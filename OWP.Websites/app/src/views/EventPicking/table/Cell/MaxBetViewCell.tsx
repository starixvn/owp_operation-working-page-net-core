import { handleCheckButton } from "../../helper/filterHelper";

const MaxBetViewCell = ({
  maxBet,
  leagueName
}: {
  maxBet: string | undefined;
  leagueName: string;
}) => {
  return (
    <div className='max-bet-cell'>
      {maxBet ? (
        <p className='max-bet-cell__value'>{maxBet}</p>
      ) : (
        <a className='mb__check' onClick={() => handleCheckButton(leagueName)}>
          Check
        </a>
      )}
    </div>
  );
};

export default MaxBetViewCell;
