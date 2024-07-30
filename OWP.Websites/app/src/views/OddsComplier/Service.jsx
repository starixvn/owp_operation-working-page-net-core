import axios from 'axios';

const query = async eventModel => {
  const response = await axios.post(`/api/odds-compiler/query`, eventModel);
  return response.data;
};

const insertOdds = async formModel => {
  const response = await axios.post(
    `/api/odds-compiler/insert-odds`,
    formModel
  );
  return response.data;
};

const deleteOdds = async formModel => {
  const response = await axios.post(
    `/api/odds-compiler/delete-odds`,
    formModel
  );
  return response.data;
};

const updateOdds = async formModel => {
  const response = await axios.post(
    `/api/odds-compiler/update-odds`,
    formModel
  );
  return response.data;
};

const updateMarket = async formModel => {
  const response = await axios.post(
    `/api/odds-compiler/save-market`,
    formModel
  );
  return response.data;
};

const exportExcel = formModel => {
  const isExcludeSpecialProducts = `isExcludeSpecialProducts=${formModel.isExcludeSpecialProducts}`;
  const params = `date=${formModel.date}&league=${formModel.league}&matchStatuses=${formModel.matchStatuses}&${isExcludeSpecialProducts}`;
  const url = `/api/odds-compiler/export-excel?${params}`;
  return url;
};

const Service = {
  query,
  insertOdds,
  deleteOdds,
  updateOdds,
  updateMarket,
  exportExcel
};

export default Service;
