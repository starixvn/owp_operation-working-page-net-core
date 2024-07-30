import Select from 'react-select-ts';
import { SubTypeOption } from '../Type';
type Props = {
  options: SubTypeOption[];
  handleChangeSubType: (value: SubTypeOption | null) => Promise<void>;
  selected: SubTypeOption | undefined;
};

const Subtype = ({ options, handleChangeSubType, selected }: Props) => {
  return (
    <div className='form-createTicket subtype'>
      <div className='left-form'>
        <label className='label' htmlFor='subject'>
          SubType:<span className='required'>*</span>
        </label>
      </div>
      <div className='right-form'>
        <div className='subtype-input'>
          <Select
            classNamePrefix='input-field-select'
            options={options}
            value={selected}
            onChange={handleChangeSubType}
          />
        </div>
      </div>
    </div>
  );
};

export default Subtype;
