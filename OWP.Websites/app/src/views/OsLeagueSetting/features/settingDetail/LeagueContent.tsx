import Region from './Region';
import Category from './Category';
import { ILeagueContent } from '../../typing/type';

const LeagueContent = (props: ILeagueContent) => {
  return (
    <div>
      <h4 className='league-info'>League Information</h4>
      <div className='setting-row'>
        <Region {...props.region} />
        <Category {...props.category} />
        {props.children}
      </div>
    </div>
  );
};

export default LeagueContent;
