import { TCellModel } from '../../type/type';
import AbsencePersonIcon from '../../../../assets/svg-element/AbsencePersonIcon';
import { UncontrolledTooltip, Input } from 'reactstrap';
import { isEqual } from 'lodash';
import { MATCH_STATUS } from '../../const.json';
import { LinkIcon, LinkOffIcon } from 'mdi-react';
import { useAppDispatch } from '../../app/hooks';
import ToastrWrapper from '../../../components/ToastrWrapper';
import store from '../../app/store';
import { useState } from 'react';
import { setMatchesSelected } from '../eventAssignmentTableSlice';

const { LIVE, IN_PLAY, POSTPONED } = MATCH_STATUS;

const NoCell = (props: TCellModel) => {
  const { rowIndex, dataSource } = props;
  const {
    IsShowAbsenceIcon,
    MatchNo,
    MatchStatus,
    InfoSiteMatchId,
    IsInfoSiteLinked,
    IsLiveStatusSynchronized,
    OWPMatchId,
    IsAllowedToViewDetail,
    AdminSiteId
  } = dataSource[rowIndex];

  const dispatch = useAppDispatch();

  const isSelected = store.getState().eventAssignmentTable.matchesSelected.some(
    x => x.OWPMatchId === OWPMatchId);

  const [isChecked, setIsChecked] = useState(isSelected);

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

  const handleSelectedChanged = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const { IsSaBa } = dataSource[rowIndex];
    const { matchesSelected } = store.getState().eventAssignmentTable;
    if (event?.target?.checked &&
      matchesSelected.length && matchesSelected.some(x => x.IsSaBa !== IsSaBa)) {
      ToastrWrapper.showWarning(
        <span>Please choose only one: SABA or Real matches!</span>
      );
      setIsChecked(false);
      return;
    }

    setIsChecked(event?.target?.checked);

    dispatch(
      setMatchesSelected({
        match: dataSource[rowIndex],
        isAdded: event?.target?.checked
      })
    );
  };

  const renderCheckbox = () => {
    const {
      policy: {
        entities: { detailInformationPolicy }
      }
    } = store.getState();
    if (
      !detailInformationPolicy.HasRead ||
      !IsAllowedToViewDetail ||
      !AdminSiteId
    ) {
      return null;
    }

    return (
      <Input
        type='checkbox'
        className='check-by-lst'
        checked={isChecked}
        onChange={handleSelectedChanged}
      />
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

      {renderCheckbox()}
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
