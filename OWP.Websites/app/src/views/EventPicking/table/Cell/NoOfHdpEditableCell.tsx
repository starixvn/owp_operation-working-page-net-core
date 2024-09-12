import { useEffect, useRef } from 'react';
import { useAppDispatch } from '../../app/hooks';
import { setValueNoOfHdp } from '../eventPickingTableSlice/reducer';
import { uniqueId } from 'lodash';
import { handleCheckButton } from '../../helper/filterHelper';

const NoOfHdpEditableCell = ({
  value,
  infoSiteMatchId,
  leagueName,
  minorBettypeId,
  isChildMatch
}: {
  value: string | null | undefined;
  infoSiteMatchId: number;
  leagueName: string;
  minorBettypeId: number;
  isChildMatch: boolean;
}) => {
  let newValue = null;
  const dispatch = useAppDispatch();
  const inputRef = useRef<HTMLInputElement>(null);
  const handleBlur = () => {
    dispatch(
      setValueNoOfHdp({
        value: inputRef?.current?.value,
        infoSiteMatchId,
        minorBettypeId
      })
    );
  };

  useEffect(() => {
    return () => {
      if (newValue !== null) {
        dispatch(
          setValueNoOfHdp({
            value: newValue,
            infoSiteMatchId,
            minorBettypeId
          })
        );
        newValue = null;
      }
    };
  });
  return (
    !isChildMatch && (
      <div key={uniqueId()} className='input-value'>
        <input
          ref={inputRef}
          type='text'
          name='noOfHdp'
          className='no__hdp input-field'
          defaultValue={value ?? ''}
          maxLength={2000}
          onBlur={handleBlur}
          onChange={v => {
            newValue = v.target?.value;
          }}
        />
        {!value && !minorBettypeId && (
          <a
            className='mb__check'
            onClick={() => handleCheckButton(leagueName)}>
            Check
          </a>
        )}
      </div>
    )
  );
};

export default NoOfHdpEditableCell;
