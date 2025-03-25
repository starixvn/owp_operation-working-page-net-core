import { useRef, useEffect, useState } from 'react';
import { UncontrolledTooltip } from 'reactstrap';
import PropTypes from 'prop-types';
import { LinkIcon, LinkOffIcon } from 'mdi-react';
import { useSelector } from 'react-redux';
import DropdownIcon from '../../../../../../assets/svg-element/DropdownIcon';
import { useAppDispatch } from '../../../../app/hooks';
import { upDateDataEntities } from '../../OSTableSlice/reducer';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired,
  handleShowDropdown: PropTypes.func,
  isLastRow: PropTypes.bool
};

const StatusCell = props => {
  const stableDispatch = useAppDispatch();
  const model = useSelector(state => state.osAssignmentTable);
  const [isOpenDropdown, setIsOpenDropdown] = useState(false);
  const { isEdit } = model;
  const { dataSource, rowIndex, handleShowDropdown, isLastRow } = props;
  const { adminsiteMatchId, offerLive, matchNo, isLinked, osMatchId } =
    dataSource[rowIndex];

  const linkRef = useRef();
  const offerRef = useRef();
  const dropdownRef = useRef();

  if (!offerLive) {
    return null;
  }

  const offerLiveLowerCase = offerLive.toLowerCase();

  const offerLiveColors = {
    postponed: '#4F4F4F',
    live: '#2FB159',
    canceled: '#EB5757'
  };

  const buildOfferLiveBorderColor = () =>
    offerLiveColors[offerLiveLowerCase] || null;

  const buildOfferLive = () => (offerLiveLowerCase === 'live' ? 'LIVE' : '');

  const renderLinkedMatch = row => {
    const renderIcon = () =>
      isLinked ? (
        <LinkIcon color='#00920A' size={15} />
      ) : (
        <LinkOffIcon color='#FF0000' size={15} />
      );

    return (
      <>
        <span id={`link-tooltip-${row}`} className='link' ref={linkRef}>
          {renderIcon()}
        </span>
        <UncontrolledTooltip placement='right' target={linkRef}>
          AdminSiteId: {adminsiteMatchId}
        </UncontrolledTooltip>
      </>
    );
  };

  const handleOnchange = value => {
    setIsOpenDropdown(false);
    stableDispatch(upDateDataEntities({ osMatchId, value }));
  };

  useEffect(() => {
    isOpenDropdown && handleShowDropdown && handleShowDropdown(isOpenDropdown);
    // eslint-disable-next-line no-warning-comments
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [isOpenDropdown]);

  useEffect(() => {
    const handleClickOutside = event => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setIsOpenDropdown(false);
      }
    };
    const abort = new AbortController();
    document.addEventListener('mousedown', handleClickOutside, abort);
    return () => abort.abort();
  }, [dropdownRef]);

  return (
    <div className='status-cell'>
      <span>{matchNo}</span>
      {renderLinkedMatch(rowIndex)}
      <span
        id={`offer-live-${rowIndex}`}
        ref={offerRef}
        className={`icon-offer-live icon-${offerLiveLowerCase}`}>
        {buildOfferLive()}
      </span>
      {isEdit && (
        <div className={`offer-live-label ${offerLiveLowerCase}`}>
          <span className='offer-live-text'>{offerLive}</span>
          <span
            className='offer-live-dropdown'
            onClick={() => setIsOpenDropdown(true)}>
            <DropdownIcon colorDropdown={buildOfferLiveBorderColor()} />
          </span>
          {isOpenDropdown && (
            <div
              ref={dropdownRef}
              className={`dropdown-offer-live is-show ${
                isLastRow ? 'is-last-row' : ''
              }`}>
              <ul>
                <li onClick={() => handleOnchange('Live')}>Live</li>
                <li onClick={() => handleOnchange('Postponed')}>Postponed</li>
                <li onClick={() => handleOnchange('Canceled')}>Canceled</li>
              </ul>
            </div>
          )}
        </div>
      )}

      <UncontrolledTooltip placement='right' target={offerRef}>
        {offerLive}
      </UncontrolledTooltip>
    </div>
  );
};

StatusCell.propTypes = propTypes;
export default StatusCell;
