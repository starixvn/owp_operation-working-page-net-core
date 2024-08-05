import React, { useState, MouseEvent } from 'react';
import { TExpressionRow } from '../../component/Type';
import CommonTextArea from '../../component/CommonTextArea';
import RowsExpression from '../../component/RowsExpression';
import { convertResponseToCondition } from '../../helper/helper';
import { useAppDispatch } from '../../app/hooks';
import {
  updateObject,
  updateExpression,
  addRowExpressions,
  removeRowExpressions
} from '../slice/editSettingSlice';
import { TConditionSource } from '../../table/TableType';

const Condition = (props: { condition: TConditionSource }) => {
  const { round, division, hasTv, hasIpTv, remark } = props.condition;
  const [checkedTV, setCheckedTV] = useState<boolean>(hasTv);
  const [checkedIPTV, setCheckedIPTV] = useState(hasIpTv);
  const dispatch = useAppDispatch();

  const rounds = convertResponseToCondition(round);
  const divisions = convertResponseToCondition(division);

  const handleOnChangeHasTV = (event: React.ChangeEvent<HTMLInputElement>) => {
    dispatch(
      updateObject({ property: 'hasTv', value: event.currentTarget.checked })
    );
    setCheckedTV(event.currentTarget.checked);
  };

  const handleOnChangeHasIPTV = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    dispatch(
      updateObject({ property: 'hasIpTv', value: event.currentTarget.checked })
    );
    setCheckedIPTV(event.currentTarget.checked);
  };

  const handleUpdateValueExpression = (
    property: 'round' | 'division',
    key: 'value' | 'type',
    value: string,
    index: number
  ) => {
    dispatch(
      updateExpression({
        property,
        key: key,
        value: value,
        index
      })
    );
  };

  const handleAddRow = (
    event: MouseEvent<HTMLElement>,
    property: 'round' | 'division'
  ) => {
    event.preventDefault();
    dispatch(addRowExpressions(property));
  };

  const handleDeleteRow = (
    event: MouseEvent<HTMLElement>,
    property: 'round' | 'division',
    index: number
  ) => {
    event.preventDefault();
    dispatch(removeRowExpressions({ index, property }));
  };

  const renderRoundList = (roundLists: TExpressionRow[]) => (
    <RowsExpression
      expressionList={roundLists}
      title='Round'
      property='round'
      handleUpdateValueExpression={handleUpdateValueExpression}
      handleAddRow={handleAddRow}
      handleDeleteRow={handleDeleteRow}
    />
  );

  const renderDivisionList = (divisionLists: TExpressionRow[]) => (
    <RowsExpression
      expressionList={divisionLists}
      title='Division'
      property='division'
      handleUpdateValueExpression={handleUpdateValueExpression}
      handleAddRow={handleAddRow}
      handleDeleteRow={handleDeleteRow}
    />
  );

  const renderRemark = (remarkLists: string) => {
    const handleOnChangeRemark = (
      event: React.ChangeEvent<HTMLTextAreaElement>
    ) => {
      const newValue = event.target.value;
      remarkLists !== newValue &&
        dispatch(updateObject({ property: 'remark', value: newValue }));
    };
    return (
      <>
        <div className='content'>
          <div className='left'>Remark</div>
          <CommonTextArea
            value={remarkLists}
            handleOnChange={handleOnChangeRemark}
          />
        </div>
      </>
    );
  };

  const renderTV = () => (
    <>
      <div className='content'>
        <div className='left'>TV</div>
        <div className='right'>
          <label>
            <input
              className='form-check-label'
              type='checkbox'
              checked={checkedTV}
              onChange={handleOnChangeHasTV}
            />
            TV (check in TV list)
          </label>
          <div className='iptv'>
            <label>
              <input
                type='checkbox'
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
