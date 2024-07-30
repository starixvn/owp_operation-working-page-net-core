import { TCellModel } from '../../type/type';
import AbsencePersonIcon from '../../../../assets/svg-element/AbsencePersonIcon';
import { UncontrolledTooltip } from 'reactstrap';
import { isEqual } from 'lodash';
import { MATCH_STATUS } from '../../const.json';
import { LinkIcon, LinkOffIcon } from 'mdi-react';

const { LIVE, IN_PLAY, POSTPONED } = MATCH_STATUS;

const NoCell = (props: TCellModel) => {
  const { rowIndex, dataSource } = props;
  const {
    IsShowAbsenceIcon,
    MatchNo,
    MatchStatus,
    InfoSiteMatchId,
    IsInfoSiteLinked,
    IsLiveStatusSynchronized
  } = dataSource[rowIndex];

  const renderInfoSiteLinked = () => {
    if (
      IsLiveStatusSynchronized &&
      [IN_PLAY, LIVE, POSTPONED].some(status => isEqual(MatchStatus, status))
    ) {
      return <span className='badge-label match-status'>LIVE</span>;
    }

    return IsInfoSiteLinked ? (
      <LinkIcon color='#00920A' size={15} />
    ) : (
      <LinkOffIcon color='#FF0000' size={15} />
    );
  };

  return (
    <div className='match-no'>
      {IsShowAbsenceIcon && <AbsencePersonIcon />}
      <span className='match-number'>{MatchNo}</span>
      <span id={`InfoSite${rowIndex}`} className='icon-validInfoSiteId'>
        {renderInfoSiteLinked()}
      </span>
      <UncontrolledTooltip placement='right' target={`InfoSite${rowIndex}`}>
        InfositeId: {InfoSiteMatchId}
      </UncontrolledTooltip>

      {!isEqual(MatchStatus, LIVE) && (
        <>
          <span
            id={`MatchStatus${rowIndex}`}
            className={`icon-status icon-${MatchStatus.toLowerCase()}`}
          />
          <UncontrolledTooltip
            placement='right'
            target={`MatchStatus${rowIndex}`}>
            {MatchStatus}
          </UncontrolledTooltip>
        </>
      )}
    </div>
  );
};

export default NoCell;
