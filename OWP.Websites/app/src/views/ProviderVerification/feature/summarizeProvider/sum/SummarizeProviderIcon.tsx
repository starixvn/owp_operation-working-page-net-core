import { useEffect, useRef, useState } from 'react';
import { Tooltip } from 'reactstrap';
import SummarizeProviderTable from './SummarizeProviderTable';
import { InfoOutlineIcon } from 'mdi-react';

const SummarizeProviderIcon = () => {
  const [tooltipOpen, setTooltipOpen] = useState(false);
  const ref = useRef<HTMLAnchorElement | null>(null);
  const toggle = () => {
    setTooltipOpen(!tooltipOpen);
  };

  useEffect(() => {
    const handleOutsideClick = (event: MouseEvent) => {
      if (ref.current && !ref.current.contains(event.target as Node)) {
        setTooltipOpen(false);
      }
    };
    const abort = new AbortController();
    window.addEventListener('click', handleOutsideClick, abort);
    return () => {
      abort.abort();
    };
  }, []);

  return (
    <>
      <a ref={ref} className='sum-provider' onClick={toggle} id='icon-summarize'>
        <InfoOutlineIcon color='black' />
      </a>
      <Tooltip
        isOpen={tooltipOpen}
        className='tooltip-explain'
        placement='bottom'
        target='icon-summarize'>
        <SummarizeProviderTable />
      </Tooltip>
    </>
  );
};

export default SummarizeProviderIcon;
