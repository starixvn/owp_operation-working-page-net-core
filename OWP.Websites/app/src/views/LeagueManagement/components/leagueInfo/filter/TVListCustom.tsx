import { useState, useEffect } from 'react';
import TVList from '../../maxbet/tvList/TVList';
import { v4 as uuidv4 } from 'uuid';
import { useDispatch, useStore } from 'react-redux';
import { queryMatchOfferLists } from '../../../actions/action';
import MaxbetConst from '../../../constants/MaxbetConst';
import { QUERY_MATCH_OFFER } from '../../../constants/action-type';

const TVListCustom = () => {
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

  const renderTVList = () => (
    <TVList onClose={() => handleDisplayTVList(false)} />
  );

  const renderRightBlock = () => {
    return (
      <div className='tv-list-block'>
        <button
          className='button-working-shift'
          onClick={() => handleDisplayTVList(true)}>
          TV List
        </button>
      </div>
    );
  };

  return (
    <>
      {model.displayTVList && renderTVList()}
      {policyData.tvListPolicy.HasRead && renderRightBlock()}
    </>
  );
};

export default TVListCustom;
