import axios from 'axios';
import { post } from '../../Shared/Service/Service';
import { buildUrl } from '../UtilsNative';
import {
  REMOVE_SPECIAL_PRODUCT,
  ADD_SPECIAL_PRODUCT
} from '../const/const.json';
import { TActionOfSpecialProduct } from '../types/dataTableType';

const removeSpecialProduct = async (model: TActionOfSpecialProduct) => {
  const url = buildUrl(REMOVE_SPECIAL_PRODUCT);
  const response = await axios.post(url, { RemoveInfo: model });
  return response;
};

const addSpecialProduct = async (model: TActionOfSpecialProduct) => {
  const url = buildUrl(ADD_SPECIAL_PRODUCT);
  const response = await axios.post(url, { AddInfo: model });
  return response;
};

export { removeSpecialProduct, addSpecialProduct };
