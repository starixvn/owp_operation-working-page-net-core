import { useEffect, useState, useCallback } from 'react';
import { useDispatch } from 'react-redux';
import Select from 'react-select';
import MaxbetConst from '../../../constants/MaxbetConst';
import { QUERY_LOGHISTORY } from '../../../constants/action-type';
import { postData } from '../../../actions/action';
import PropTypes from 'prop-types';

const DropDownFilter = ({ productId }) => {
  const [totalRow, setTotalRow] = useState(
    MaxbetConst.numberRowsSpecialProducOption[0].value
  );
  const dispatch = useDispatch();
  const stableDispatch = useCallback(dispatch, [dispatch]);

  useEffect(() => {
    stableDispatch(
      postData(
        MaxbetConst.viewlogUrl,
        {
          ObjectId: productId,
          PageSize: totalRow
        },
        QUERY_LOGHISTORY
      )
    );
  }, [stableDispatch, productId, totalRow]);

  return (
    <Select
      options={MaxbetConst.numberRowsSpecialProducOption}
      defaultValue={MaxbetConst.numberRowsSpecialProducOption[0]}
      className='basic-multi-select'
      classNamePrefix='select'
      name='loghistory'
      onChange={filter => setTotalRow(filter.value)}
    />
  );
};

DropDownFilter.propTypes = {
  productId: PropTypes.number.isRequired
};

export default DropDownFilter;
