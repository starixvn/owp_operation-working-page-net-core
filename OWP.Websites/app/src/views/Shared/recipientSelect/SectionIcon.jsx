import {
  iconSectionLeft,
  iconSectionRight,
  iconSectionCenter
} from './const.json';
const SectionIcon = () => {
  return (
    <svg
      className='groups-suggestion'
      width='24'
      height='18'
      viewBox='0 0 24 18'
      fill='none'
      xmlns='http://www.w3.org/2000/svg'>
      <path d={iconSectionLeft} fill='#459be7' />
      <path d={iconSectionCenter} fill='#459be7' />
      <path d={iconSectionRight} fill='#459be7' />
    </svg>
  );
};

export default SectionIcon;
