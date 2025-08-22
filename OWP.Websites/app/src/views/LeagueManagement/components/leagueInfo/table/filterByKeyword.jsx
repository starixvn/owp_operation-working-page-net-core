const checkLeague = (arraySource, keyWordFormat) =>
  arraySource.filter(item =>
    item.league?.toLowerCase().includes(keyWordFormat)
  );

const checkShortLeague = (arraySource, keyWordFormat) =>
  arraySource.filter(item =>
    item.shortLeague?.toLowerCase().includes(keyWordFormat)
  );

const columnFilter = {
  'League': checkLeague,
  'Short_League': checkShortLeague
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
