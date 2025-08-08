import React, { MouseEvent } from 'react';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import CommonTextArea from '../../component/CommonTextArea';
import RowsExpression from '../../component/RowsExpression';
import { convertResponseToCondition } from '../../helper/helper';
import {
  addRowExpressionsAddPopup,
  removeRowExpressionsAddPopup,
  updateExpressionAddPopup,
  updateObjectAddPopup
} from '../slice/addSettingSlice';
import MaxbetConst from '../../../../constants/MaxbetConst';

const ConditionOfAdd = () => {
  const { round, division, remark, hasIpTv, hasTv } = useAppSelector(
    state => state.addSettingSlice
  );
  const dispatch = useAppDispatch();
  const rounds = convertResponseToCondition(round);
  const divisions = convertResponseToCondition(division);

  const handleUpdateValueExpression = (
    property: 'round' | 'division',
    key: 'value' | 'type',
    value: string,
    index: number
  ) => {
    dispatch(
      updateExpressionAddPopup({
        property,
        key,
        value,
        index
      })
    );
  };

  const handleAddRow = (
    event: MouseEvent<HTMLElement>,
    property: 'round' | 'division'
  ) => {
    event.preventDefault();
    dispatch(addRowExpressionsAddPopup(property));
  };

  const handleDeleteRow = (
    event: MouseEvent<HTMLElement>,
    property: 'round' | 'division',
    index: number
  ) => {
    event.preventDefault();
    dispatch(removeRowExpressionsAddPopup({ index, property }));
  };

  const handleOnChangeRemark = (
    event: React.ChangeEvent<HTMLTextAreaElement>
  ) => {
    const newValue = event.target.value;
    dispatch(updateObjectAddPopup({ property: 'remark', value: newValue }));
  };

  const handleOnChangeCheckBox = (
    event: React.ChangeEvent<HTMLInputElement>,
    property: 'hasTv' | 'hasIpTv'
  ) => {
    dispatch(
      updateObjectAddPopup({ property, value: event.currentTarget.checked })
    );
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
              checked={hasTv}
              onChange={event => handleOnChangeCheckBox(event, 'hasTv')}
            />
            TV (check in TV list)
          </label>
          <div className='iptv'>
            <label>
              <input
                type='checkbox'
                checked={hasIpTv}
                onChange={event => handleOnChangeCheckBox(event, 'hasIpTv')}
              />
              IPTV
            </label>
          </div>
        </div>
      </div>
      <RowsExpression
        expressionList={rounds}
        option={MaxbetConst.expressionOption}
        title='Round'
        property='round'
        handleUpdateValueExpression={handleUpdateValueExpression}
        handleAddRow={handleAddRow}
        handleDeleteRow={handleDeleteRow}
      />
      <RowsExpression
        expressionList={divisions}
        option={MaxbetConst.divisionExpressionOption}
        title='Division'
        property='division'
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
