import { createRef, useCallback } from 'react';
import PropTypes from 'prop-types';
import { useStore, useSelector, useDispatch } from 'react-redux';
import NumericTextbox from 'react-numeric-textbox';
import { selectSpecialProductData } from '../../../../selector/SelectorSpecialProduct';
import { SpecialProductConst } from '../../../../constants/SpecialProductConst';
import { postDataWithExtraModel, crud } from '../../../../actions/action';
import {
  SPECIALPRODUCT_UPDATEINFO,
  ACTION_SPECIALPRODUCT
} from '../../../../constants/action-type';
import { buildPostModelUpdateSpecialProduct } from '../../../../helper/buildPostModelHelper';
import ToastrWrapper from '../../../../../components/ToastrWrapper';
import withReactContent from 'sweetalert2-react-content';
import Swal from 'sweetalert2';
import { isEqual } from 'lodash';
const Toastr = withReactContent(Swal);

const convertToThousand = number => {
  if (!number) {
    return null;
  }
  return number * 1000;
};

const confirmPopup = async message =>
  Toastr.fire({
    icon: 'question',
    html: message,
    allowOutsideClick: false,
    allowEnterKey: false,
    showCancelButton: true,
    reverseButtons: true,
    cancelButtonText: 'No',
    confirmButtonText: 'Yes'
  });

const validateFTHDP = async props => {
  const { maxbet, existedIndex, rowIndex, dispatch } = props;
  const setFocus = () => {
    const cell =
      document.getElementsByClassName('table')[0].lastElementChild
        .firstElementChild.lastElementChild.children[rowIndex].firstElementChild
        .children[0].firstElementChild.children[0];
    cell.focus();
  };

  if (maxbet === 0 || maxbet === null) {
    const result = await confirmPopup(
      'MB FT.HDP must be filled. Do you want to continue?'
    );
    result.value
      ? window.requestAnimationFrame(setFocus)
      : dispatch(crud(ACTION_SPECIALPRODUCT.DELETE));

    return false;
  }

  if (existedIndex >= 0) {
    await ToastrWrapper.showWarning(
      'MB FT.HDP has already existed. Please insert another value!'
    );
    window.requestAnimationFrame(setFocus);
    return false;
  }
  return true;
};

const isDuplicated = props => {
  const { setting, value, productId } = props;
  return (
    setting.Maxbet_FTHDP === convertToThousand(value) &&
    setting.ProductId !== productId
  );
};

const validateCanSubmit = async props => {
  const { maxbet, existedIndex, rowIndex, dispatch, propKey } = props;
  if (isEqual(propKey, 'Maxbet_FTHDP')) {
    const validateProps = {
      maxbet,
      existedIndex,
      rowIndex,
      dispatch
    };
    return await validateFTHDP(validateProps);
  }
  return true;
};

const handleKeyDown = (e, props) => {
  if (isEqual(e.key, 'Tab') && isEqual(props.propKey, 'Maxbet_FTHDP')) {
    e.preventDefault();
    const cell =
      document.getElementsByClassName('table')[0].lastElementChild
        .firstElementChild.lastElementChild.children[props.rowIndex]
        .firstElementChild.children[0].firstElementChild.children[0];
    cell.blur();
  }
};

const MaxbetCell = props => {
  const selector = useSelector(selectSpecialProductData);
  const data = selector.specialProductData;
  const element = data.filter(x => x.ProductId === props.productId)[0];
  const store = useStore();
  const { policyData } = store.getState().policyReducer;
  const hasWrite = policyData.generalPolicy.HasWrite;
  const dispatch = useDispatch();
  const stableDispatch = useCallback(dispatch, [dispatch]);
  const input = createRef(props.value);

  const handlePostData = maxbet => {
    if (
      !isEqual(element[props.propKey], convertToThousand(maxbet)) &&
      (isEqual(props.propKey, 'Maxbet_FTHDP') || element.Maxbet_FTHDP)
    ) {
      const value = maxbet > 0 ? convertToThousand(maxbet) : null;
      const model = buildPostModelUpdateSpecialProduct(
        element,
        props.propKey,
        value
      );
      const extraModel = {
        ...model,
        LastUpdate: policyData.generalPolicy.Username
      };
      const url = `${SpecialProductConst.specialProductUpdateAPIRoute}`;
      const postModel = postDataWithExtraModel(
        url,
        model,
        extraModel,
        SPECIALPRODUCT_UPDATEINFO
      );
      stableDispatch(postModel);
    }
  };

  const handleOnBlur = async () => {
    const existedIndex = data.findIndex(x => {
      const checkDupModel = {
        setting: x,
        value: input.current.value,
        productId: props.productId
      };

      return isDuplicated(checkDupModel);
    });

    const maxbet = input.current.value;
    const validateCanSubmitProps = {
      maxbet,
      existedIndex,
      dispatch,
      rowIndex: props.rowIndex,
      propKey: props.propKey
    };
    const canSubmit = await validateCanSubmit(validateCanSubmitProps);

    if (canSubmit) {
      handlePostData(maxbet);
    }
  };

  if (selector.specialProductDuplicateMBFTHDP) {
    (async () => {
      await ToastrWrapper.showWarning(
        'MB FT.HDP has already existed. Please insert another value!'
      );
      window.requestAnimationFrame(() => {
        const cell =
          document.getElementsByClassName('table')[0].lastElementChild
            .firstElementChild.lastElementChild.children[props.rowIndex]
            .firstElementChild.children[0].firstElementChild.children[0];
        cell.focus();
      });
    })();
  }

  return (
    <div className='input-value' onKeyDown={e => handleKeyDown(e, props)}>
      <NumericTextbox
        className='input-field'
        ref={input}
        disabled={!hasWrite}
        value={props.value}
        min={0}
        max={999999}
        decimals={3}
        autoCorrect
        format=''
        name='maxbet'
        onBlur={handleOnBlur}
      />
      {props.value > 0 && <span>K</span>}
    </div>
  );
};

MaxbetCell.propTypes = {
  value: PropTypes.number,
  productId: PropTypes.number,
  propKey: PropTypes.string,
  rowIndex: PropTypes.number
};

export default MaxbetCell;
