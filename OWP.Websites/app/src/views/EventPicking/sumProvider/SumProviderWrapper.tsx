import { SigmaIcon } from 'mdi-react';
import { useState } from 'react';
import SumProviderTable from './SumProviderTable';
import { Tooltip } from 'reactstrap';

const SumProviderWrapper = () => {
  const [show, setShow] = useState(false);

  return (<>
    <a className='sum-provider' onClick={() => setShow(!show)}>
      <SigmaIcon color='rgba(0, 0, 0, 0.75)' size={18} />
    </a>
    <Tooltip
      isOpen={show}
      className='tooltip-explain'
      placement='bottom'
      target='icon-info'>
      <SumProviderTable />
    </Tooltip>
  </>);
};

export default SumProviderWrapper;