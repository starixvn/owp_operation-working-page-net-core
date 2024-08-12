import { Input } from 'reactstrap';

const Value = (props: {
  value: string | number | undefined;
  index: number;
  hasWrite: boolean;
  property: 'round' | 'division';
  handleUpdateValueExpression: (
    property: 'round' | 'division',
    key: 'value' | 'type',
    value: string,
    index: number
  ) => void;
}) => {
  const { value, index, hasWrite, property, handleUpdateValueExpression } =
    props;
  return (
    <Input
      disabled={!hasWrite}
      type='text'
      className='input-field'
      defaultValue={value}
      maxLength={100}
      onBlur={event =>
        handleUpdateValueExpression(
          property,
          'value',
          event.target.value,
          index
        )
      }
    />
  );
};

export default Value;
