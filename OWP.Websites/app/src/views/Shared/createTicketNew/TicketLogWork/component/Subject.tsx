import { TextField } from '@mui/material';

type Props = {
  subjectValue: string;
  errorValue: string;
  handleOnChangeSubject: (value: string) => void;
};

const Subject = ({
  subjectValue,
  errorValue,
  handleOnChangeSubject
}: Props) => {
  return (
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
          helperText={errorValue}
          error={Boolean(errorValue)}
          margin='normal'
          onChange={event => handleOnChangeSubject(event.target.value)}
          variant='standard'
          value={subjectValue}
        />
      </div>
    </div>
  );
};

export default Subject;
