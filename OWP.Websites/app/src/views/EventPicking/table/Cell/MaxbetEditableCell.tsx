import { createRef, useEffect } from 'react';
import NumericTextbox from 'react-numeric-textbox';
import { useAppDispatch } from '../../app/hooks';
import { setValueMaxbetSpread } from '../eventPickingTableSlice/reducer';
import { handleCheckButton } from '../../helper/filterHelper';
import ToastrWrapper from '../../../components/ToastrWrapper';

const MaxbetEditableCell = ({
  maxBet,
  infoSiteMatchId,
  minorBettypeId,
  leagueName,
  isChildMatch,
  isShowCheckButton
}: {
  maxBet: string | number | undefined;
  infoSiteMatchId: number;
  leagueName: string;
  minorBettypeId: number;
  isChildMatch: boolean;
  isShowCheckButton: boolean;
}) => {
  let newValue = -999999999;
  const dispatch = useAppDispatch();
  const inputRef = createRef<HTMLInputElement>();
  const handleBlur = () => {
    const valueInput = Number(inputRef?.current?.value);
    if (!isNaN(valueInput) && valueInput >= 100) {
      ToastrWrapper.showWarning(<span>Please enter a MaxBet value that is less than 3 digits!</span>);
    }

    const valueNumber =
      typeof inputRef?.current?.value === 'number'
        ? (inputRef?.current?.value as number) * 1000
        : undefined;

    dispatch(
      setValueMaxbetSpread({
        value: valueNumber,
        key: 'maxbetHdp',
        infoSiteMatchId,
        minorBettypeId
      })
    );
  };

  useEffect(() => {
    return () => {
      if (newValue !== -999999999) {
        dispatch(
          setValueMaxbetSpread({
            value: newValue * 1000,
            key: 'maxbetHdp',
            infoSiteMatchId,
            minorBettypeId
          })
        );
        newValue = -999999999;
      }
    };
  });

  return (
      <>
        <div className='input-value'>
          <NumericTextbox
            ref={inputRef}
            className='input-field'
            value={maxBet ?? undefined}
            min={0}
            max={999999}
            decimals={3}
            autoCorrect
            format=''
            name='maxbet'
            onBlur={handleBlur}
            onChange={(v: number) => {
              newValue = v;
            }}
          />
          <span>K</span>
        </div>
        {isShowCheckButton && !maxBet && !minorBettypeId && (
          <a
            className='mb__check'
            onClick={() => handleCheckButton(leagueName, isChildMatch)}>
            Check
          </a>
      )}
    </>
  );
};

export default MaxbetEditableCell;
