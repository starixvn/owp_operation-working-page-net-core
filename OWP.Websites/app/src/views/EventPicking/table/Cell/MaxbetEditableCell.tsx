import { createRef, useEffect } from 'react';
import NumericTextbox from 'react-numeric-textbox';
import { useAppDispatch } from '../../app/hooks';
import { setValueMaxbetSpread } from '../eventPickingTableSlice/reducer';
import { handleCheckButton } from '../../helper/filterHelper';

const MaxbetEditableCell = ({
  maxBet,
  infoSiteMatchId,
  leagueName
}: {
  maxBet: string | number | undefined;
  infoSiteMatchId: number;
  leagueName: string;
}) => {
  let newValue = -999999999;
  const dispatch = useAppDispatch();
  const inputRef = createRef<HTMLInputElement>();
  const handleBlur = () => {
    const valueNumber= typeof inputRef?.current?.value==='number'
    ? (inputRef?.current?.value as number) * 1000
    : undefined;
    
    dispatch(
      setValueMaxbetSpread({
        value:  valueNumber,
        key: 'maxbetHdp',
        infoSiteMatchId
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
            infoSiteMatchId
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
          onChange={v => {
            newValue = v;
          }}
        />
        <span>K</span>
      </div>
      {!maxBet && (
        <a className='mb__check' onClick={() => handleCheckButton(leagueName)}>
          Check
        </a>
      )}
    </>
  );
};

export default MaxbetEditableCell;
