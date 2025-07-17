import { CellProps } from '../../../type/TableType';
import { UncontrolledTooltip, Input } from 'reactstrap';
import { useAppDispatch } from '../../../app/hookNative';
import AbsencePersonIcon from '../../../../../assets/svg-element/AbsencePersonIcon';
import { useContext, useState } from 'react';
import store from '../../../app/storeNative';
import ToastrWrapper from '../../../../components/ToastrWrapper';
import { setMatchesSelected } from '../../tableSlice/tableSlice';
import { AppContext } from '../../../../Shared/Context';

const NoCell = ({ rowIndex, dataSource }: CellProps): JSX.Element => {
  const { MatchNo, MatchStatus, IsShowAbsenceIcon, OWPMatchId,
    AdminSiteId,
    IsAllowedToViewDetail
  } = dataSource[rowIndex];
  const { detailInformationPolicy } = useContext(AppContext);
  const dispatch = useAppDispatch();

  const isSelected = store.getState().events.matchesSelected.some(
    x => x.OWPMatchId === OWPMatchId);

  const [isChecked, setIsChecked] = useState(isSelected);


  const renderIcon = () => {
    const classNames = {
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
    const { IsSaba } = dataSource[rowIndex];
    const { matchesSelected } = store.getState().events;
    if (
      event?.target?.checked &&
      matchesSelected.length && matchesSelected.some(x => x.IsSaba !== IsSaba)) {
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
    <div className='no-cell'>
      {IsShowAbsenceIcon && <AbsencePersonIcon />}
      {MatchNo && <span>{MatchNo}</span>}
      {renderCheckbox()}
      {renderIcon()}
    </div>
  );
};

export default NoCell;
