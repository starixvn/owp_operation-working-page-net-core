import { useState, useEffect } from 'react';
import Select from 'react-select';
import { useSelector, useDispatch } from 'react-redux';
import PropTypes from 'prop-types';
import { updateTicketSubType } from './chatBoardThunkNative';
import ToastrWrapper from '../../components/ToastrWrapper';

export const TicketSubType = ({ options = [], policy = 'policy' }) => {
  const [selectedSubType, setSelectedSubType] = useState([]);

  const ticketDetail = useSelector(state => state.chatBoard.ticketDetail);
  const { ticketSubTypeId, ticketSubTypeName, ticketId } = ticketDetail;

  const { permission } = useSelector(state => state[policy]);
  const { HasWrite, ActorId } = permission;
  const dispatch = useDispatch();

  useEffect(() => {
    setSelectedSubType({
      value: ticketSubTypeId || 0,
      label: ticketSubTypeName || ''
    });
  }, [ticketSubTypeId, ticketSubTypeName]);

  const handleChangeSubType = async subtype => {
    if (subtype.value === ticketSubTypeId) {
      return;
    }
    const modelTicket = {
      ...ticketDetail,
      ticketSubTypeId: subtype.value,
      ticketSubTypeName: subtype.label
    };

    const requestModel = {
      ticketId,
      actorId: ActorId,
      ticketInfo: modelTicket
    };

    const result = await dispatch(updateTicketSubType(requestModel));
    if (updateTicketSubType.rejected.match(result)) {
      ToastrWrapper.showError(result.payload);
      return;
    }

    setSelectedSubType(subtype);
  };

  return (
    <Select
      styles={{ menuPortal: base => ({ ...base, zIndex: 9999 }) }}
      classNamePrefix='input-field-select'
      options={options}
      value={selectedSubType}
      defaultValue={{
        value: ticketSubTypeId,
        label: ticketSubTypeName
      }}
      onChange={value => handleChangeSubType(value)}
      isDisabled={!HasWrite}
    />
  );
};

TicketSubType.propTypes = {
  options: PropTypes.array,
  policy: PropTypes.string
};
