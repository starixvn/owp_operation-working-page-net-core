import Select from 'react-select-ts';
import { StatusOption } from '../Type';

type Props = {
  options: StatusOption[];
  selected: StatusOption;
  handleOnChange: (select: StatusOption | null) => void;
};

const Status = ({ options, selected, handleOnChange }: Props) => {
  return (
    <div className='form-createTicket status'>
      <label>
        Status:<span className='required'>*</span>
      </label>
      <br />
      <div className='select'>
        <Select
          classNamePrefix='input-field-select'
          options={options}
          value={selected}
          onChange={handleOnChange}
        />
      </div>
    </div>
  );
};

export default Status;
