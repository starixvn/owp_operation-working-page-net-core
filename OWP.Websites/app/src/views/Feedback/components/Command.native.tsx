import { useState } from 'react';
import Loading from '../../components/Loading';
import useStore from '../app/store';

const Command = () => {
  const [loading, setLoading] = useState(false);
  const submit = useStore(state => state.postFeedback);

  const handleSubmit = () => {
    setLoading(true);
    submit();
  };
  return <>
    {loading && <Loading />}<div className='command'>
      <button onClick={handleSubmit} className='submit'>Submit</button>
    </div></>;
};

export default Command;