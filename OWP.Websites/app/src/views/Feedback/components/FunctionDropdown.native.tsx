import { useEffect, useState } from 'react';
import { BODY } from '../const.json';
import Select from 'react-select-ts';
import { queryFunction } from '../services.native';
import { SelectOption } from '../type';
import useStore from '../app/store';

const FunctionDropdown = () => {
  const updateFeedbackInfo = useStore(state => state.updateFeedbackInfo);
  const [options, setOptions] = useState([]);
  const [selected, setSelected] = useState<SelectOption | null>(null);

  const handleChange = (event: SelectOption | null) => {
    setSelected(event);
    event && updateFeedbackInfo('functionName', event.label);
  };

  const style = {
    control: base => ({
      ...base,
      border: '1px solid gray',
      boxShadow: 'none',
      '&:hover': {
        border: '1px solid gray'
      }
    })
  };

  useEffect(() => {
    (async () => {
      setOptions(await queryFunction());
    })();
  }, []);
  return (
    <div className='choose__function'>
      <p className='title' style={{ marginBottom: '3px' }}>
        {BODY.CHOOSE_FUNCTION.EN}
        <span className='required'> *</span>
      </p>
      <p className='title'>
        {BODY.CHOOSE_FUNCTION.CN}
      </p>
      <Select
        styles={style}
        data-testId='select'
        options={options}
        value={selected}
        onChange={handleChange}
        className='basic-filter-select'
      />
    </div>
  );
};

export default FunctionDropdown;
