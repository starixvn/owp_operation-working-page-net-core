import React, { useState, MouseEvent } from 'react';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import CommonTextArea from '../../component/CommonTextArea';
import RowsExpression from '../../component/RowsExpression';
import { convertResponseToCondition } from '../../editPopup/helper/helper';
import {
  TExpressionRow,
  TOption,
  TAddExpressionKey
} from '../../editPopup/EditType';

const ConditionOfAdd = () => {
  const { round, division, remark } = useAppSelector(
    state => state.addSettingSlice
  );
  const dispatch = useAppDispatch();
  const rounds = convertResponseToCondition(round);
  const divisions = convertResponseToCondition(division);

  const handleChangeExpression = (
    selectedOption: TOption | null,
    property: 'round' | 'division',
    index: number
  ) => {
    // selectedOption &&
    //   dispatch(
    //     updateExpression({
    //       property,
    //       key: 'type',
    //       value: selectedOption.label?.toString() || '',
    //       index
    //     })
    //   );
  };

  const handleUpdateValueExpression = (
    property: 'round' | 'division',
    key: 'value',
    value: string,
    index: number
  ) => {
    // dispatch(
    //   updateExpression({
    //     property,
    //     key: key,
    //     value: value,
    //     index
    //   })
    // );
  };

  const handleAddRow = (
    event: MouseEvent<HTMLElement>,
    property: 'round' | 'division'
  ) => {
    // event.preventDefault();
    // dispatch(addRowExpressions(property));
  };

  const handleDeleteRow = (
    event: MouseEvent<HTMLElement>,
    property: 'round' | 'division',
    index: number
  ) => {
    // event.preventDefault();
    // dispatch(removeRowExpressions({ index, property }));
  };

  const handleOnChangeRemark = (
    event: React.ChangeEvent<HTMLTextAreaElement>
  ) => {
    const newValue = event.target.value;
    // remark !== newValue &&
    //  dispatch(updateObject({ property: 'remark', value: newValue }));
  };

  return (
    <div className='list'>
      <div className='title'>Condition</div>
      <div className='content'>
        <div className='left'>TV</div>
        <div className='right'>
          <label>
            <input
              className='form-check-label'
              type='checkbox'
              checked={false}
            />
            TV (check in TV list)
          </label>
          <div className='iptv'>
            <label>
              <input type='checkbox' checked={false} />
              IPTV
            </label>
          </div>
        </div>
      </div>
      <RowsExpression
        expressionList={rounds}
        title='Round'
        property='round'
        handleChangeExpression={handleChangeExpression}
        handleUpdateValueExpression={handleUpdateValueExpression}
        handleAddRow={handleAddRow}
        handleDeleteRow={handleDeleteRow}
      />
      <RowsExpression
        expressionList={divisions}
        title='Division'
        property='division'
        handleChangeExpression={handleChangeExpression}
        handleUpdateValueExpression={handleUpdateValueExpression}
        handleAddRow={handleAddRow}
        handleDeleteRow={handleDeleteRow}
      />
      <div className='content'>
        <div className='left'>Remark</div>
        <CommonTextArea value={remark} handleOnChange={handleOnChangeRemark} />
      </div>
    </div>
  );
};
export default ConditionOfAdd;
