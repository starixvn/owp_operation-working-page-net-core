import { useState, Suspense } from 'react';
import Loading from '../../../../components/Loading';
import { IPropsTeamInfo } from '../../../type';
import LazyTeamInfo from '../../lazyExport/LazyTeamInfo';

const TeamInfoWrapper = ({ owpLeagueId, leagueName }: IPropsTeamInfo): JSX.Element => {
  const [visibleTeamInfo, setVisibleTeamInfo] = useState<boolean>(false);
  return (
    <>
      <button onClick={() => setVisibleTeamInfo(true)} className="btn-show">
        <span className="icon-eye" />
      </button>
      {visibleTeamInfo &&
        <Suspense fallback={<Loading />}>
          <LazyTeamInfo
            onClose={() => setVisibleTeamInfo(false)}
            owpLeagueId={owpLeagueId}
            leagueName={leagueName} />
        </Suspense>}
    </>
  );
};


export default TeamInfoWrapper;