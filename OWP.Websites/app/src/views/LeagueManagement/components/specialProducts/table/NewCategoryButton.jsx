import { useDispatch, useSelector } from 'react-redux';
import { crud } from '../../../actions/action';
import { ACTION_SPECIALPRODUCT } from '../../../constants/action-type';
import { selectSpecialProductData } from '../../../selector/SelectorSpecialProduct';
import { findIndex } from 'lodash';

const NewCategoryButton = () => {
  const dispatch = useDispatch();
  const selector = useSelector(selectSpecialProductData);
  const data = selector.specialProductData;
  const isDisabled = findIndex(data, x => x.ProductId === -1) > 0;

  const setFocus = () => {
    const cell = document.getElementsByClassName('table')[0].lastElementChild
      .firstElementChild.lastElementChild.children[data.length]
      .firstElementChild.children[0].firstElementChild.children[0];
    cell.focus();
  };

  const addNew = e => {
    e.preventDefault();
    const model = {
      ProductId: -1,
      Type: 'MBC'
    };
    dispatch(crud(ACTION_SPECIALPRODUCT.ADD, model));
    window.requestAnimationFrame(setFocus);
  };

  return (
    <button
      className={`button-submit ${isDisabled ? 'disabled' : ''}`}
      onClick={e => addNew(e)}
      disabled={isDisabled}>
      New Category
    </button>
  );
};

export default NewCategoryButton;
