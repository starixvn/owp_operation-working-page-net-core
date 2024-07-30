import { buildRequestURL } from './Const';
const specialProductGetAllAPIKey = 'league-management-get-all-special-product';
const specialProductDeleteAPIKey = 'league-management-delete-special-product';

const specialProductUpdateAPIRoute = buildRequestURL('update-special-product');

export const SpecialProductConst = {
  specialProductGetAllAPIKey,
  specialProductUpdateAPIRoute,
  specialProductDeleteAPIKey
};
