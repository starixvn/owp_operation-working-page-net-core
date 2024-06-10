import { createRef, useState } from 'react';
import PropTypes from 'prop-types';
import { useStore, useSelector, useDispatch } from 'react-redux';
import { selectSpecialProductData } from '../../../../selector/SelectorSpecialProduct';
import { isEqual } from 'lodash';
import { postDataWithExtraModel } from '../../../../actions/action';
import { SpecialProductConst } from '../../../../constants/SpecialProductConst';
import { SPECIALPRODUCT_UPDATEINFO } from '../../../../constants/action-type';
import { buildPostModelUpdateSpecialProduct } from '../../../../helper/buildPostModelHelper';
import ReactTooltip from 'react-tooltip';

const propTypes = {
  value: PropTypes.string,
  productId: PropTypes.number.isRequired
};

const RemarkCell = props => {
  const [isEditing, setEdit] = useState(false);
  const store = useStore();
  const { policyData } = store.getState().policyReducer;
  const hasWrite = policyData.generalPolicy.HasWrite;
  const selector = useSelector(selectSpecialProductData);
  const data = selector.specialProductData.find(
    x => x.ProductId === props.productId
  );
  const dispatch = useDispatch();
  const input = createRef();

  const handleOnBlur = () => {
    setEdit(false);
    const remark = input.current.value;
    if (!isEqual(data.Remark, remark)) {
      const model = buildPostModelUpdateSpecialProduct(
        data,
        'Remark',
        remark.length > 0 ? remark : null
      );
      dispatch(
        postDataWithExtraModel(
          `${SpecialProductConst.specialProductUpdateAPIRoute}`,
          model,
          model,
          SPECIALPRODUCT_UPDATEINFO
        )
      );
    }
  };

  return (
    <div onClick={() => setEdit(hasWrite)} className='remark'>
      {!isEditing && (
        <label data-tip={props.value} onClick={() => setEdit(hasWrite)}>
          {props.value}
        </label>
      )}
      {isEditing && (
        <input
          ref={input}
          autoFocus
          type='text'
          className='input-field'
          disabled={!hasWrite}
          defaultValue={props.value}
          onBlur={handleOnBlur}
          maxLength='200'
        />
      )}
      {props.value && !isEditing && <ReactTooltip place='top' effect='solid' />}
    </div>
  );
};

RemarkCell.propTypes = propTypes;

export default RemarkCell;
