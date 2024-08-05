import { uniqueId } from 'lodash';
import { TExpressionRow } from './Type';
import Action from './Action';
import Condition from './Condition';
import Value from './Value';
import { MouseEvent } from 'react';

interface RowExpressionProps {
  expressionList: Array<TExpressionRow>;
  title?: string;
  property: 'round' | 'division';
  handleUpdateValueExpression: (
    property: 'round' | 'division',
    key: 'value' | 'type',
    value: string,
    index: number
  ) => void;
  handleAddRow: (
    event: MouseEvent<HTMLElement>,
    property: 'round' | 'division'
  ) => void;
  handleDeleteRow: (
    event: MouseEvent<HTMLElement>,
    property: 'round' | 'division',
    index: number
  ) => void;
}

const RowsExpression = ({
  expressionList,
  title,
  property,
  handleUpdateValueExpression,
  handleAddRow,
  handleDeleteRow
}: RowExpressionProps) => {
  return (
    <div className='content'>
      {title && <div className='left'>{title}</div>}
      <div className='right'>
        {expressionList.map((expression, index) => (
          <div className='expression__row' key={`${uniqueId()}`}>
            <Condition
              condition={expression.condition}
              index={index}
              hasWrite
              property={property}
              handleUpdateValueExpression={handleUpdateValueExpression}
            />
            <Value
              value={expression.value}
              index={index}
              property={property}
              hasWrite
              handleUpdateValueExpression={handleUpdateValueExpression}
            />
            <Action
              handleAddRow={handleAddRow}
              handleDeleteRow={handleDeleteRow}
              property={property}
              index={index}
              hasWrite
              modelsLength={expressionList.length}
            />
          </div>
        ))}
      </div>
    </div>
  );
};

export default RowsExpression;
