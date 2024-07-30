import PropTypes from 'prop-types';
import DeleteIcon from 'mdi-react/DeleteIcon';
import { SpecialProductConst } from '../../../../constants/SpecialProductConst';
import { useDispatch } from 'react-redux';
import { postData } from '../../../../actions/action';
import { SPECIALPRODUCT_DELETE } from '../../../../constants/action-type';
import ToastrWrapper from '../../../../../components/ToastrWrapper';
import { buildUpdateSimpleApi } from '../../../../../Shared/Constants';

const propTypes = {
  productId: PropTypes.number.isRequired,
  maxbetFTHDP: PropTypes.number
};

const ActionCell = ({ productId, maxbetFTHDP }) => {
  const dispatch = useDispatch();

  const handleDelete = async () => {
    const result = await ToastrWrapper.confirm(
      `Do you want to delete all max bet of special products with MB of FT.HDP of ${maxbetFTHDP}K ? `
    );
    if (result.value) {
      const model = {
        productId,
        apiKey: SpecialProductConst.specialProductDeleteAPIKey
      };
      dispatch(postData(buildUpdateSimpleApi(), model, SPECIALPRODUCT_DELETE));
    }
  };

  return (
    productId !== -1 && (
      <DeleteIcon className='icon-delete' size={13} onClick={handleDelete} />
    )
  );
};

ActionCell.propTypes = propTypes;

export default ActionCell;
