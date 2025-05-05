import { UncontrolledTooltip, Input } from 'reactstrap';
import { CellProps } from '../../../type/TTaskListTable';
import AbsencePersonIcon from '../../../../../assets/svg-element/AbsencePersonIcon';
import useStableDispatch from '../../../../Shared/customHook/useStableDispatch';
import { useAppSelector } from '../../../app/hooks';
import { setMatchesSelected } from '../taskListSlice';
import { useState } from 'react';
import ToastrWrapper from '../../../../components/ToastrWrapper';

const NoCell = ({
  hasDetailInformationPermission,
  rowIndex,
  dataSource,
  firstMatchCheckedIsSaba
}: CellProps) => {
  const {
    MatchNo,
    MatchStatus,
    IsShowAbsenceIcon,
    IsAllowedToViewDetail,
    AdminSiteId,
    OWPMatchId,
    IsSaba
  } = dataSource;
  const dispatch = useStableDispatch();

  const isSelected = useAppSelector(state =>
    state.taskList.matchesSelected.some(x => x.OWPMatchId === OWPMatchId)
  );

  const [isChecked, setIsChecked] = useState(isSelected);

  const renderIcon = () => {
    const classNames: { [key: string]: string } = {
      'Live': 'live',
      'In-play': 'inplay',
      'Completed': 'completed',
      'Cancel': 'cancelled',
      'Postponed': 'postponed',
      'Suspended': 'suspended'
    };

    return (
      <>
        <span
          className={`badge-label ${classNames[MatchStatus]} `}
          style={{ display: 'none' }}>
          {MatchStatus}
        </span>
        {MatchStatus === 'Live' ? (
          <i className={classNames[MatchStatus]}></i>
        ) : (
          <>
            <i
              className={`icon-status icon-${classNames[MatchStatus]}`}
              id={`link-match-status-${rowIndex}`}></i>
            <UncontrolledTooltip
              placement='bottom'
              target={`link-match-status-${rowIndex}`}>
              {MatchStatus}
            </UncontrolledTooltip>
          </>
        )}
      </>
    );
  };

  const handleSelectedChanged = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    if (event?.target?.checked) {
      if (
        firstMatchCheckedIsSaba !== undefined &&
        firstMatchCheckedIsSaba !== IsSaba
      ) {
        ToastrWrapper.showWarning(
          <span>Please choose only one: SABA or Real matches!</span>
        );
        setIsChecked(false);
        return;
      }
    }

    setIsChecked(event?.target?.checked);

    dispatch(
      setMatchesSelected({
        match: dataSource,
        isAdded: event?.target?.checked
      })
    );
  };

  const renderCheckbox = () => {
    if (
      !hasDetailInformationPermission ||
      !IsAllowedToViewDetail ||
      !Boolean(AdminSiteId)
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
    <div className='no-cell'>
      {IsShowAbsenceIcon && <AbsencePersonIcon />}
      {MatchNo && <span>{MatchNo}</span>}
      {renderCheckbox()}
      {renderIcon()}
    </div>
  );
};

export default NoCell;
