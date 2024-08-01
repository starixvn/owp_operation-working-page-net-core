import { Input } from 'reactstrap';

const Value = (props: {
  value: string | number | undefined;
  index: number;
  hasWrite: boolean;
  property: 'round' | 'division';
  handleUpdateValueExpression: (
    property: 'round' | 'division',
    key: 'value',
    value: string,
    index: number
  ) => void;
}) => {
  const { value, index, hasWrite, property, handleUpdateValueExpression } =
    props;

  const handleUpdateValueExpressionCallBack = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const newValue = event.target.value;
    handleUpdateValueExpression(property, 'value', newValue, index);
  };

  return (
    <Input
      disabled={!hasWrite}
      type='text'
      className='input-field'
      defaultValue={value}
      maxLength={100}
      onBlur={handleUpdateValueExpressionCallBack}
    />
  );
};

export default Value;
