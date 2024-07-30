import { Input } from 'reactstrap';

type Props = {
  text: string;
  id: string;
  value: boolean;
  handleClick: () => void;
};

const Checkbox = ({ text, id, handleClick, value = false }: Props) => {
  return (
    <div className='checkbox'>
      <Input
        id={id}
        type='checkbox'
        className='form-check-input'
        onClick={handleClick}
        defaultChecked={value}
      />
      <label className='form-check-label' htmlFor={id}>
        {text}
      </label>
    </div>
  );
};

export default Checkbox;
