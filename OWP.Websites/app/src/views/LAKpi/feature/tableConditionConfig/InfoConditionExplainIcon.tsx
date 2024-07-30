import { useEffect, useRef, useState } from 'react';
import { Tooltip } from 'reactstrap';
import TableConditionConfig from './TableConditionConfig';

const InfoConditionExplainIcon = () => {
  const [tooltipOpen, setTooltipOpen] = useState(false);
  const ref = useRef<SVGSVGElement | null>(null);
  const toggle = () => {
    setTooltipOpen(!tooltipOpen);
  };

  useEffect(() => {
    const handleOutsideClick = (event: MouseEvent) => {
      if (ref.current && !ref.current.contains(event.target as Node)) {
        setTooltipOpen(false);
      }
    };
    window.addEventListener('click', handleOutsideClick);
    return () => {
      window.removeEventListener('click', handleOutsideClick);
    };
  }, []);

  return (
    <>
      <svg
        ref={ref}
        onClick={toggle}
        id='icon-info'
        width='24'
        height='24'
        viewBox='0 0 24 24'
        fill='none'
        xmlns='http://www.w3.org/2000/svg'>
        <path
          d='M12 2C6.489 2 2 6.489 2 12C2 17.511 6.489 22 12 22C17.511 22 22 17.511 22
          12C22 6.489 17.511 2 12 2ZM12 4C16.4301 4 20 7.56988 20 12C20 16.4301 16.4301
          20 12 20C7.56988 20 4 16.4301 4 12C4 7.56988 7.56988 4 12 4ZM11 7V9H13V7H11ZM11
          11V17H13V11H11Z'
          fill='#39f'
        />
      </svg>
      <Tooltip
        isOpen={tooltipOpen}
        className='tooltip-explain'
        placement='bottom'
        target='icon-info'>
        <TableConditionConfig />
      </Tooltip>
    </>
  );
};

export default InfoConditionExplainIcon;
