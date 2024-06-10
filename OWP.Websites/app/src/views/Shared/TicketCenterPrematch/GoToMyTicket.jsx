import { useEffect, useState } from 'react';
import ToastrWrapper from '../../components/ToastrWrapper';
import { getMyTicketPermission, buildGoToMyTicketLink } from './helper/helper';

const GoToMyTicket = () => {
  const [permission, setPermission] = useState(null);
  useEffect(() => {
    const getPermission = async () => {
      const ticketPermission = await getMyTicketPermission();
      setPermission(ticketPermission);
    };

    getPermission();
  }, []);

  const handleClick = () => {
    permission?.HasRead
      ? window.open(buildGoToMyTicketLink())
      : ToastrWrapper.showInfo(
          "You don't have permission to see your tickets!"
        );
  };

  return (
    <span className='myTicket' onClick={handleClick} style={style}>
      Go to My Tickets
    </span>
  );
};

export default GoToMyTicket;

const style = {
  color: '#1e88e5',
  fontSize: '14px',
  cursor: 'pointer'
};
