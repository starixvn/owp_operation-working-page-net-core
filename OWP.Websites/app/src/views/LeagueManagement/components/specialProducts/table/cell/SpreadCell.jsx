import { createRef } from 'react';
import PropTypes from 'prop-types';
import { useStore, useSelector, useDispatch } from 'react-redux';
import NumericTextbox from 'react-numeric-textbox';
import { selectSpecialProductData } from '../../../../selector/SelectorSpecialProduct';
import { SpecialProductConst } from '../../../../constants/SpecialProductConst';
import { postDataWithExtraModel } from '../../../../actions/action';
import { SPECIALPRODUCT_UPDATEINFO } from '../../../../constants/action-type';
import { buildPostModelUpdateSpecialProduct } from '../../../../helper/buildPostModelHelper';

const SpreadCell = props => {
  const selector = useSelector(selectSpecialProductData);
  const data = selector.specialProductData.filter(
    x => x.ProductId === props.productId
  )[0];
  const store = useStore();
  const { policyData } = store.getState().policyReducer;
  const hasWrite = policyData.generalPolicy.HasWrite;
  const dispatch = useDispatch();
  const input = createRef();

  const convertToPercent = number => {
    if (!number) {
      return null;
    }
    return number / 100;
  };

  const handleOnBlur = () => {
    const newSpread =
      typeof input.current.value === 'number'
        ? convertToPercent(input.current.value)
        : null;
    if (data[props.propKey] !== newSpread) {
      const model = buildPostModelUpdateSpecialProduct(
        data,
        props.propKey,
        newSpread > 0 ? newSpread : null
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
    <div className='input-value'>
      <>
        <NumericTextbox
          ref={input}
          className='input-field'
          disabled={!hasWrite}
          value={props.value}
          min={0}
          max={100}
          decimals={2}
          autoCorrect
          format=''
          name='spread'
          onBlur={handleOnBlur}
        />
        {props.value > 0 && <span>%</span>}
      </>
    </div>
  );
};

SpreadCell.propTypes = {
  value: PropTypes.number,
  productId: PropTypes.number.isRequired,
  propKey: PropTypes.string.isRequired
};

export default SpreadCell;
