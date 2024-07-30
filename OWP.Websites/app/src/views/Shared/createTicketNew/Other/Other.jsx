import TextField from '@mui/material/TextField';
import { useSelector } from 'react-redux';
import useStableDispatch from '../../customHook/useStableDispatch';
import { otherSubject, changeErrorOther } from '../createTicketBoardSliceNew';

const Other = () => {
  const dispatch = useStableDispatch();
  const { errorOther, errorTextField, typeTicketText, valueOther } =
    useSelector(state => state.createTicketBoard);
  const handleOnChangeSubject = event => {
    dispatch(otherSubject(event.target.value));
    dispatch(changeErrorOther(false));
  };
  return (
    <>
      <div className='header-popup'>
        <h1> Type: {typeTicketText}</h1>
      </div>
      <div className='form-createTicket'>
        <div className='left-form'>
          <label
            className='label'
            style={{ marginTop: '10px' }}
            htmlFor='subject'>
            Subject:<span className='required'>*</span>
          </label>
        </div>
        <div className='right-form'>
          <TextField
            id='subject'
            helperText={errorOther ? errorTextField : ''}
            error={errorOther}
            margin='normal'
            onChange={handleOnChangeSubject}
            variant='standard'
            value={valueOther}
          />
        </div>
      </div>
    </>
  );
};
export default Other;
