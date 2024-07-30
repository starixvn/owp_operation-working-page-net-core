import { TextField } from '@mui/material';
import { useState } from 'react';
import { get } from '../../../Service/Service';
import { urlGetMatchInfo } from '../../const/const';
import { formatTime } from '../../../DateTime/dateTimeHelper';
import { MatchInfo } from '../Type';
import { isValidMatchId } from '../LogWorkHelper';

type Props = {
  setMatchId: (value: string) => void;
  errorInvalid: string;
  setErrorInvalid: (value: string) => void;
  matchInfo: MatchInfo | null;
  setMatchInfo: (value: MatchInfo | null) => void;
};

const EventSubject = ({
  setMatchId,
  errorInvalid,
  setErrorInvalid,
  matchInfo,
  setMatchInfo
}: Props) => {
  const [value, setValue] = useState('');

  const handleOnBlur = async () => {
    const validAdminSideId = isValidMatchId(parseInt(value));
    if (validAdminSideId) {
      const response = await get({ adminSiteId: value, url: urlGetMatchInfo });
      setMatchInfo(response?.data);
      response?.data ? setErrorInvalid('') : setErrorInvalid('Invalid MatchId');
      response?.data ? setMatchId(value) : setMatchId('');
      return;
    }
    setErrorInvalid('Invalid MatchId');
    setMatchId('');
    setMatchInfo(null);
  };

  const handleOnChange = (text: string) => {
    setValue(text);
    setMatchInfo(null);
    setErrorInvalid('');
  };
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
          helperText={errorInvalid}
          error={Boolean(errorInvalid)}
          margin='normal'
          value={value}
          onChange={event => handleOnChange(event.target.value)}
          onBlur={handleOnBlur}
          variant='standard'
          label={
            matchInfo && (
              <div className='selected-event'>
                <span>{matchInfo.leagueName}</span>
                <span className='league-event-selected event-subject'>
                  {matchInfo.homeName} vs {matchInfo.awayName} (
                  {formatTime(matchInfo?.koTime, 'minute', 'lll')} )
                </span>
              </div>
            )
          }
        />
      </div>
    </div>
  );
};

export default EventSubject;
