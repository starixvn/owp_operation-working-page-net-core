import React, { useState } from 'react';
import { TExpressionRow } from '../EditType';
import CommonTextArea from '../component/CommonTextArea';
import RowsExpression from '../component/RowsExpression';
import { convertResponseToCondition } from '../helper/helper';
import { useAppDispatch } from '../../app/hooks';
import { updateObject } from '../slice/editSettingSlice';
import { TConditionSource } from '../../table/TableType';

const Condition = (props: {
  condition: TConditionSource
}) => {
  const { round, division, hasTv, hasIpTv, remark } = props.condition;
  const [checkedTV, setCheckedTV] = useState<boolean>(hasTv);
  const [checkedIPTV, setCheckedIPTV] = useState(hasIpTv);
  const dispatch = useAppDispatch();

  const rounds = convertResponseToCondition(round);
  const divisions = convertResponseToCondition(division);

  const handleOnChangeHasTV = (event: React.ChangeEvent<HTMLInputElement>) => {
    dispatch(updateObject({ property: 'hasTv', value: event.currentTarget.checked }));
    setCheckedTV(event.currentTarget.checked);
  };

  const handleOnChangeHasIPTV = (event: React.ChangeEvent<HTMLInputElement>) => {
    dispatch(updateObject({ property: 'hasIpTv', value: event.currentTarget.checked }));
    setCheckedIPTV(event.currentTarget.checked);
  };

  const renderRoundList = (rounds: TExpressionRow[]) => (
    <RowsExpression
      expressionList={rounds}
      title='Round'
      property='round'
    />
  );

  const renderDivisionList = (divisions: TExpressionRow[]) => (
    <RowsExpression
      expressionList={divisions}
      title='Division'
      property='division'
    />
  );

  const renderRemark = (remark: string) => (
    <>
      <div className='content'>
        <div className='left'>Remark</div>
        <CommonTextArea
          value={remark}
          inputKey='remark'
        />
      </div>
    </>
  );

  const renderTV = () => (
    <>
      <div className='content'>
        <div className='left'>TV</div>
        <div className='right'>
          <label>
            <input
              className='form-check-label'
              type='checkbox'
              defaultChecked={checkedTV}
              checked={checkedTV}
              onChange={handleOnChangeHasTV}
            />
            TV (check in TV list)
          </label>
          <div className='iptv'>
            <label>
              <input
                type='checkbox'
                defaultChecked={checkedIPTV}
                checked={checkedIPTV}
                onChange={handleOnChangeHasIPTV}
              />
              IPTV
            </label>
          </div>
        </div>
      </div>
    </>
  );


  return (
    <div className='list'>
      <div className='title'>Condition</div>
      {renderTV()}
      {renderRoundList(rounds)}
      {renderDivisionList(divisions)}
      {renderRemark(remark)}
    </div>
  );
};

export default Condition;