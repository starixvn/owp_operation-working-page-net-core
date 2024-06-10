import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { collapseAllLeague } from '../league/shiftSlice/shiftSlice';

const CollapseBtn = () => {
  const stableDispatch = useStableDispatch();
  const handelClick = () => {
    stableDispatch(collapseAllLeague());
  };
  return (
    <div className='box-btn'>
      <button className='btn btn-outline-primary' onClick={handelClick}>
        Collapse
      </button>
    </div>
  );
};

export default CollapseBtn;
