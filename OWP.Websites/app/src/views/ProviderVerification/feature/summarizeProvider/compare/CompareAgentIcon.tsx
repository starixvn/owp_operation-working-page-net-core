import { useState } from 'react';
import CompareAgentPopup from './CompareAgentPopup';

const CompareAgent = () => {
  const [isOpenPopup, setIsOpenPopup] = useState(false);

  return (
    <>
      <svg onClick={() => setIsOpenPopup(true)}
        className='compare-icon'
        width="30" height="28" viewBox="0 0 37 32" fill="none" xmlns="http://www.w3.org/2000/svg">
        <path d="M3.0835 5.39551H33.9168M3.0835 17.7288H11.5627M3.0835 30.0622H11.5627" stroke="black" strokeWidth="2" strokeLinecap="round" />
        <path d="M24.2816 26.2077C26.0193 26.2077 27.6858 25.5174 28.9146 24.2886C30.1434 23.0599 30.8337 21.3933 30.8337 19.6556C30.8337 17.9179 30.1434 16.2513 28.9146 15.0226C27.6858 13.7938 26.0193 13.1035 24.2816 13.1035C22.5439 13.1035 20.8773 13.7938 19.6486 15.0226C18.4198 16.2513 17.7295 17.9179 17.7295 19.6556C17.7295 21.3933 18.4198 23.0599 19.6486 24.2886C20.8773 25.5174 22.5439 26.2077 24.2816 26.2077Z" stroke="black" strokeWidth="2" />
        <path d="M28.521 24.666L33.9168 30.1004" stroke="black" strokeWidth="2" strokeLinecap="round" />
      </svg>
      {isOpenPopup &&
        <CompareAgentPopup setIsOpenPopup={setIsOpenPopup} />
      }
    </>
  );
};

export default CompareAgent;