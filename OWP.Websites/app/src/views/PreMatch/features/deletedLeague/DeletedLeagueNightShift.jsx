import DeletedLeagueTable from './DeletedLeagueTable';
import { useSelector } from 'react-redux';

const DeletedLeagueNightShift = () => {
  const model = useSelector(state => state.deletedLeague);

  return <DeletedLeagueTable leagueDeleted={model.leagueDeleted} />;
};

export default DeletedLeagueNightShift;
