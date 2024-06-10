import PropTypes from 'prop-types';
import MaxbetConst from '../../../../constants/MaxbetConst';
import isEqual from 'lodash.isequal';
import { Cell } from 'fixed-data-table-2';
import { useStore, useDispatch } from 'react-redux';
import { hasWriteOnPageMaxbetGeneral } from '../../../../helper/commonHelper';
import { buildModelLeagueDetail } from '../../../../helper/buildPostModelHelper';
import { postDataWithExtraModel } from '../../../../actions/action';
import { LEAGUEDETAIL_UPDATEINFO } from '../../../../constants/action-type';
import { buildUpdateSimpleApi } from '../../../../../Shared/Constants';

const propTypes = {
  data: PropTypes.object.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const CategoryCell = ({ data, rowIndex }) => {
  const dispatch = useDispatch();
  const store = useStore();
  const { policyData } = store.getState().policyReducer;
  const hasWrite = hasWriteOnPageMaxbetGeneral(policyData);
  const selectCategoy = MaxbetConst.leagueCategoryOption.find(item =>
    isEqual(data.Category, item.label)
  );
  const handleChangeLeagueGroup = event => {
    const index = event.nativeEvent.target.selectedIndex;
    const Category = event.nativeEvent.target[index].text;
    const model = buildModelLeagueDetail(data, 'Category', Category);
    const extraModel = {
      key: 'Category',
      value: Category,
      rowIndex
    };
    dispatch(
      postDataWithExtraModel(
        buildUpdateSimpleApi(),
        model,
        extraModel,
        LEAGUEDETAIL_UPDATEINFO
      )
    );
  };
  return (
    <Cell>
      <div className='select-wrapper-default'>
        <select
          disabled={!hasWrite}
          className='browser-default'
          defaultValue={selectCategoy.value}
          onChange={event => handleChangeLeagueGroup(event)}>
          {MaxbetConst.leagueCategoryOption.map(opt => {
            return (
              <option key={opt.value} value={opt.value}>
                {opt.label}
              </option>
            );
          })}
        </select>
      </div>
    </Cell>
  );
};

CategoryCell.propTypes = propTypes;

export default CategoryCell;
