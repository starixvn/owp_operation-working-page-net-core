import { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import Dialog from '../../../common/Dialog';
import isEqual from 'lodash.isequal';
import { useDispatch, useStore } from 'react-redux';
import { buildModelLeagueDetail } from '../../../../helper/buildPostModelHelper';
import { postDataWithExtraModel } from '../../../../actions/action';
import { LEAGUEDETAIL_UPDATEINFO } from '../../../../constants/action-type';
import { Input, UncontrolledTooltip } from 'reactstrap';
import { EditIcon } from 'mdi-react';
import { Cell } from 'fixed-data-table-2';
import { hasWriteOnPageMaxbetGeneral } from '../../../../helper/commonHelper';
import { buildUpdateSimpleApi } from '../../../../../Shared/Constants';

const MatchOfferCell = props => {
  const dispatch = useDispatch();
  const store = useStore();
  const { modelKey, data, rowIndex } = props;
  const rowData = data[modelKey];
  const { policyData } = store.getState().policyReducer;
  const hasWrite = hasWriteOnPageMaxbetGeneral(policyData);
  const { matchOffers } = store.getState().matchOffersReducer;
  const [filteredMatchOffers, setFilteredMatchOffers] = useState([]);
  const [selectedMatchOffers, setSelectedMatchOffers] = useState([]);

  const [displayModal, setDisplayModal] = useState(false);

  useEffect(() => {
    setFilteredMatchOffers(matchOffers);
    setSelectedMatchOffers(rowData ? rowData.split(',') : []);
  }, [matchOffers, rowData]);

  const handleChange = e => {
    if (e.target.checked) {
      setSelectedMatchOffers([...selectedMatchOffers, e.target.name]);
    } else {
      setSelectedMatchOffers(
        selectedMatchOffers.filter(item => !isEqual(item, e.target.name))
      );
    }
  };

  const handleSave = e => {
    e.preventDefault();
    const matchOfferToString = selectedMatchOffers.join(',');
    const model = buildModelLeagueDetail(data, modelKey, matchOfferToString);
    const extraModel = {
      rowIndex,
      key: modelKey,
      value: matchOfferToString
    };
    dispatch(
      postDataWithExtraModel(
        buildUpdateSimpleApi(),
        model,
        extraModel,
        LEAGUEDETAIL_UPDATEINFO
      )
    );
    setDisplayModal(false);
  };

  const handleClose = () => {
    setFilteredMatchOffers(matchOffers);
    setSelectedMatchOffers(rowData ? rowData.split(',') : []);
    setDisplayModal(false);
  };

  const handleInputChange = e => {
    const newArray = matchOffers.filter(x =>
      x.Value.toLowerCase().includes(e.target.value.toLowerCase())
    );
    setFilteredMatchOffers(newArray);
  };

  const renderBody = () => {
    return (
      <>
        <input
          placeholder='Search'
          className='input-search'
          onChange={handleInputChange}
        />
        <div className='match-offer-fix'>
          {filteredMatchOffers.map(item => (
            <div key={item.Id} className='checkbox'>
              <Input
                type='checkbox'
                name={item.Value}
                id={`${item.Value}`}
                defaultChecked={selectedMatchOffers.some(m =>
                  isEqual(m, item.Value)
                )}
                onChange={handleChange}
              />
              <label className='form-check-label' htmlFor={`${item.Value}`}>
                {item.Value}
              </label>
            </div>
          ))}
        </div>
      </>
    );
  };
  const propsModal = {
    titleHeader: 'Match Offer',
    onClose: () => handleClose(),
    onSave: handleSave,
    renderBody,
    hasWrite: true,
    size: 'sm',
    className: 'match-offer-dialog'
  };
  return (
    <Cell className='match-offer'>
      <label id={`Matchoffer${rowIndex}${modelKey}`}>{data[modelKey]}</label>
      {hasWrite && (
        <EditIcon
          name='iconEdit'
          className='icon-edit'
          size={13}
          onClick={() => setDisplayModal(true)}
        />
      )}
      {data[modelKey] && (
        <UncontrolledTooltip
          placement='top-start'
          target={`Matchoffer${rowIndex}${modelKey}`}>
          {data[modelKey]}
        </UncontrolledTooltip>
      )}
      {displayModal && <Dialog {...propsModal} />}
    </Cell>
  );
};

MatchOfferCell.propTypes = {
  modelKey: PropTypes.string.isRequired,
  data: PropTypes.object.isRequired,
  rowIndex: PropTypes.number.isRequired
};

export default MatchOfferCell;
