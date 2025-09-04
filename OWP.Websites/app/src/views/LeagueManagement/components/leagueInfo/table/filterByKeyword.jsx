const checkLeague = (arraySource, keyWordFormat) =>
  arraySource.filter(item =>
    item.league?.toLowerCase().includes(keyWordFormat)
  );

const checkShortLeague = (arraySource, keyWordFormat) =>
  arraySource.filter(item =>
    item.shortLeague?.toLowerCase().includes(keyWordFormat)
  );

const checkRemark = (arraySource, keyWordFormat) =>
  arraySource.filter(item =>
    item.condition.remark?.toLowerCase().includes(keyWordFormat)
  );

const checkRemark2 = (arraySource, keyWordFormat) => {
  return arraySource.filter(
    item =>
      item.remark2?.toLowerCase().includes(keyWordFormat) ||
      item.remark2WD?.toLowerCase().includes(keyWordFormat) ||
      item.remark2WK?.toLowerCase().includes(keyWordFormat)
  );
};

const checkLSTNote = (arraySource, keyWordFormat) =>
  arraySource.filter(item =>
    item.lstNote?.toLowerCase().includes(keyWordFormat)
  );

const columnFilter = {
  'League': checkLeague,
  'Short_League': checkShortLeague,
  'Remark': checkRemark,
  'Remark2': checkRemark2,
  'LST_Note': checkLSTNote
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
