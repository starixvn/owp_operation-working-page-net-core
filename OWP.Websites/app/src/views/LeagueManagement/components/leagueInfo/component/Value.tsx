import { Input } from 'reactstrap';

const Value = (props: {
  value: string | number | undefined;
  index: number;
  hasWrite: boolean;
  property: 'round' | 'division';
  hint?: string;
  handleUpdateValueExpression: (
    property: 'round' | 'division',
    key: 'value' | 'type',
    value: string,
    index: number
  ) => void;
}) => {
  const { value, index, hasWrite, property, handleUpdateValueExpression, hint } =
    props;
  return (
    <Input
      disabled={!hasWrite}
      type='text'
      className='input-field'
      defaultValue={value}
      maxLength={100}
      placeholder={hint}
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
