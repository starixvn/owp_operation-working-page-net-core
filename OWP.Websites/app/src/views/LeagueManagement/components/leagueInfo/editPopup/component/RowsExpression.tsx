import { uniqueId } from 'lodash';
import { TExpressionRow } from '../EditType';
import Action from './Action';
import Condition from './Condition';
import Value from './Value';

interface RowExpressionProps {
  expressionList: Array<TExpressionRow>;
  title?: string;
  property: "round" | "division";
}

const RowsExpression = ({ expressionList, title, property }: RowExpressionProps) => {
  return (
    <div className='content'>
      {title && <div className='left'>{title}</div>}
      <div className='right'>
        {expressionList.map((expression, index) =>
        (
          <div className='expression__row' key={`${uniqueId()}`}>
            <Condition
              property={property}
              condition={expression.condition}
              index={index}
              hasWrite />
            <Value value={expression.value}
              index={index}
              property={property}
              hasWrite />
            <Action property={property}
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
