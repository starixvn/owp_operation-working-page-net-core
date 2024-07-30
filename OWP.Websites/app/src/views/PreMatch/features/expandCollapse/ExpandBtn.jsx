import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { expandAllLeague } from '../league/shiftSlice/shiftSlice';

const ExpandBtn = () => {
  const stableDispatch = useStableDispatch();
  const handelClick = () => {
    stableDispatch(expandAllLeague());
  };
  return (
    <div className='box-btn'>
      <button className='btn btn-outline-primary' onClick={handelClick}>
        Expand
      </button>
    </div>
  );
};

export default ExpandBtn;
