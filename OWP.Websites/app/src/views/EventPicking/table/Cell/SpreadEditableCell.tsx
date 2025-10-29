import { createRef, useEffect } from 'react';
import NumericTextbox from 'react-numeric-textbox';
import { useAppDispatch } from '../../app/hooks';
import { setValueMaxbetSpread } from '../eventPickingTableSlice/reducer';
import { handleCheckButton } from '../../helper/filterHelper';

const SpreadEditableCell = ({
  spread,
  spreadKey,
  infoSiteMatchId,
  leagueName,
  minorBettypeId,
  isChildMatch,
  isShowCheckButton
}: {
  spread: number | string | undefined;
  spreadKey: 'maxbetHdp' | 'spreadOu' | 'spreadHdp';
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
    const valueNumber =
      typeof inputRef?.current?.value === 'number'
        ? (inputRef?.current?.value as number) / 100
        : undefined;
    dispatch(
      setValueMaxbetSpread({
        value: valueNumber,
        key: spreadKey,
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
            value: newValue / 100,
            key: spreadKey,
            infoSiteMatchId,
            minorBettypeId
          })
        );
        newValue = -999999999;
      }
    };
  });

  return  (
    <>
      <div className='input-value'>
        <NumericTextbox
          ref={inputRef}
          className='input-field'
          value={spread}
          min={0}
          max={99}
          decimals={2}
          autoCorrect
          format=''
          onBlur={handleBlur}
          onChange={(v: number) => {
            newValue = v;
          }}
        />
        <span>%</span>
      </div>
      {isShowCheckButton && !spread  && (
        <a className='mb__check' onClick={() => handleCheckButton(leagueName, isChildMatch)}>
          Check
        </a>
      )}
    </>
  );
};

export default SpreadEditableCell;
