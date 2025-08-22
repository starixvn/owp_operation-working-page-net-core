const checkLeague = (arraySource, keyWordFormat) =>
  arraySource.filter(item =>
    item.league?.toLowerCase().includes(keyWordFormat)
  );

const columnFilter = {
  'League': checkLeague
};

const filterByKeyword = (
  arraySource,
  { keyword = undefined, columnKey = undefined }
) => {

  if (!keyword) {
    return arraySource;
  }
  if (columnKey in columnFilter) {
    const keyWordFormat = keyword.toLowerCase();
    return columnFilter[columnKey](arraySource, keyWordFormat);
  }

  return arraySource || [];
};

export default filterByKeyword;
