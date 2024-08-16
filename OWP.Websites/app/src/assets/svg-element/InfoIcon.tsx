import { UncontrolledTooltip } from 'reactstrap';

const InfoIcon = () => {
  return (
    <>
      <svg
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
          fill='black'
        />
      </svg>
      <UncontrolledTooltip
        className='tooltip-explain'
        placement='right'
        target='icon-info'>
        <p>Explanation:</p>
        <ul>
          <li>
            Filter match:
            <ul>
              <li>Gray: A match belongs to Black list</li>
              <li>White: Other cases</li>
            </ul>
          </li>
          <li>Orange: Special match</li>
        </ul>
      </UncontrolledTooltip>
    </>
  );
};

export default InfoIcon;
