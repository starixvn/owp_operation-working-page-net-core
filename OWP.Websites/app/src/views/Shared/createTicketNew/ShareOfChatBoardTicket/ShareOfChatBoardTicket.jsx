import PropTypes from 'prop-types';
import { useEffect, useRef, useState } from 'react';
import { useSelector } from 'react-redux';
import Select from 'react-select';
import Comment from './Comment';
import { getValueRef } from '../../Utils';
import useStableDispatch from '../../customHook/useStableDispatch';
import RecipientsSelect from '../../recipientSelect/RecipientsSelect';
import Attachments from '../Attachments/Attachments';
import {
  filterFeedbackSubtype,
  getRecipientDefaultBySubType,
  getSubTypeByType
} from '../Helper/HelperCreateTicket';
import {
  changeErrorRecipient,
  setSelectedRecipients,
  setSubTypeSelected
} from '../createTicketBoardSliceNew';

const propTypes = {
  handleOnSubmit: PropTypes.func,
  handleOnClosePopup: PropTypes.func,
  selectedFiles: PropTypes.any,
  setSelectedFiles: PropTypes.func,
  ticketType: PropTypes.any,
  selectedRecipients: PropTypes.any,
  defaultRecipients: PropTypes.any
};

const ShareOfChatBoardTicket = props => {
  const {
    selectedFiles,
    setSelectedFiles,
    handleOnSubmit,
    handleOnClosePopup,
    ticketType,
    selectedRecipients,
    defaultRecipients
  } = props;

  const commentRef = useRef();

  const handleSubmit = () => {
    handleOnSubmit(getValueRef(commentRef));
  };
  const dispatch = useStableDispatch();
  const [subTypeOption, setSubTypeOption] = useState([]);
  const { errorRecipient, subTypeSelected } = useSelector(
    state => state.createTicketBoard
  );

  useEffect(() => {
    (async () => {
      const subType = await getSubTypeByType(ticketType);
      let option = subType.map(({ subTypeId: value, subTypeName: label }) => ({
        value,
        label
      }));
      option = filterFeedbackSubtype(ticketType, option);
      setSubTypeOption([...option, { value: 0, label: '' }]);
    })();
  }, [ticketType]);

  const handleChangeSubType = async select => {
    dispatch(setSubTypeSelected(select));
    const recipient = await getRecipientDefaultBySubType(select.value);
    dispatch(setSelectedRecipients(recipient.concat(defaultRecipients)));
  };

  return (
    <>
      <div className='form-createTicket'>
        <div className='left-form'>
          <label className='label' htmlFor='subject'>
            SubType:
          </label>
        </div>
        <div className='right-form'>
          <div className='subtype-input'>
            <Select
              classNamePrefix='input-field-select'
              placeholder=''
              value={subTypeSelected}
              onChange={select => handleChangeSubType(select)}
              options={subTypeOption}
            />
          </div>
        </div>
      </div>
      <div className='form-createTicket'>
        <div className='left-form'>
          <label className='label' htmlFor='subject'>
            Recipient:<span className='required'>*</span>
          </label>
        </div>
        <div className='right-form'>
          <RecipientsSelect
            ticketType={ticketType}
            defaultValue={selectedRecipients || []}
            onChange={value => dispatch(setSelectedRecipients(value))}
            errorSubmit={errorRecipient}
            onChangeErrorSubmit={value => dispatch(changeErrorRecipient(value))}
          />
        </div>
      </div>
      <div className='form-createTicket'>
        <div className='left-form'>
          <div className='label'>Comment:</div>
        </div>

        <Comment ref={commentRef} />
      </div>
      <Attachments
        selectedFiles={selectedFiles}
        setSelectedFiles={setSelectedFiles}
      />
      <div className='btn-group-add-new'>
        <button className='button-close' onClick={handleOnClosePopup}>
          Cancel
        </button>
        <button className='button-submit' onClick={handleSubmit}>
          Submit
        </button>
      </div>
    </>
  );
};
ShareOfChatBoardTicket.propTypes = propTypes;
export default ShareOfChatBoardTicket;
