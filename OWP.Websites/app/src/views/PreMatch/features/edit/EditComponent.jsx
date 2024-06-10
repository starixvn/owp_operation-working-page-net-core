import { useDispatch, useSelector } from 'react-redux';
import { changeMode } from './editSlice';
import dayjs from 'dayjs';
import ToastrWrapper from './../../../components/ToastrWrapper';
const EditComponent = () => {
  const dispatch = useDispatch();
  const { filterDate } = useSelector(state => state.filter);

  const onEdit = () => {
    const validEditedDate = dayjs(new Date()).add(-3, 'days');

    if (dayjs(filterDate).isBefore(validEditedDate)) {
      return ToastrWrapper.showError(
        'You cannot edit any assignments that were created more than three days ago.'
      );
    }
    return dispatch(changeMode({ isEdit: true }));
  };
  return (
    <button className='button-submit' onClick={onEdit}>
      Edit
    </button>
  );
};

export default EditComponent;
