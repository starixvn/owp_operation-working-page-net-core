import { useState } from 'react';
import UserGuideNative from './UserGuideNative';
import UserGuideIcon from '../../../assets/svg-element/UserGuideIcon';

const UserGuideWrapper = () => {
  const [show, setShow] = useState(false);

  return <><a className='user-guide' onClick={() => setShow(true)}>
    <UserGuideIcon />
  </a>
    {show && <UserGuideNative onShow={setShow} />}
  </>;
};

export default UserGuideWrapper;