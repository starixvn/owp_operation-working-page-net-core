import DeletedLeagueTable from './DeletedLeagueTable';
import { useSelector } from 'react-redux';

const DeletedLeagueDayShift = () => {
  const model = useSelector(state => state.deletedLeague);

  return <DeletedLeagueTable leagueDeleted={model.leagueDeleted} />;
};

export default DeletedLeagueDayShift;
