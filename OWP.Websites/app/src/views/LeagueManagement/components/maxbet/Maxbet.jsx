import { useState, useCallback, useEffect } from 'react';
import Filter from './filter/Filter';
import TVList from './tvList/TVList';
import TableMaxbetGeneral from './tablemaxbetgeneral/TableMaxbetGeneral';
import { v4 as uuidv4 } from 'uuid';
import { useDispatch, useStore } from 'react-redux';
import { queryMatchOfferLists } from '../../actions/action';
import MaxbetConst from '../../constants/MaxbetConst';
import { QUERY_MATCH_OFFER } from '../../constants/action-type';

const Maxbet = () => {
  const store = useStore();
  const dispatch = useDispatch();
  const { policyData } = store.getState().policyReducer;
  const [model, setModel] = useState({
    displayTVList: false,
    displayMaxbetGeneralTable: false,
    tableKey: uuidv4()
  });

  useEffect(() => {
    dispatch(
      queryMatchOfferLists(MaxbetConst.getMatchOffers, QUERY_MATCH_OFFER)
    );
  }, [dispatch]);

  const handleDisplayTVList = value => {
    setModel({
      ...model,
      displayTVList: value
    });
  };

  const handleDisplayMaxbetGeneralTable = useCallback(() => {
    setModel({
      ...model,
      displayMaxbetGeneralTable: true,
      tableKey: uuidv4()
    });
  }, [model]);

  const renderTVList = () => (
    <TVList onClose={() => handleDisplayTVList(false)} />
  );

  const renderRightBlock = () => {
    return (
      <div className='filter-control btn-block btn-block--right'>
        <button
          className='button-working-shift'
          onClick={() => handleDisplayTVList(true)}>
          TV List
        </button>
      </div>
    );
  };
  return (
    <div className='maxbet-content'>
      <div className='filter maxbet-management'>
        <Filter displayMaxbetGeneralTable={handleDisplayMaxbetGeneralTable} />
        {model.displayTVList && renderTVList()}
        {policyData.tvListPolicy.HasRead && renderRightBlock()}
      </div>
      {model.displayMaxbetGeneralTable && (
        <div className='tbl-maxbet-wrapper'>
          <TableMaxbetGeneral key={model.tableKey} />
        </div>
      )}
    </div>
  );
};

export default Maxbet;
