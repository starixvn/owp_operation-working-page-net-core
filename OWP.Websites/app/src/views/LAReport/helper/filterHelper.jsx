import { includes } from 'lodash';
import { FilterConst } from '../constants/reportConst';

const filterByLAColumn = props => {
  const { laInfo, laKeyWord, ...rest } = props;
  const newLaInfo = laKeyWord
    ? laInfo.filter(info =>
        info.Username
          ? includes(
              info.Username.toLowerCase().trim(),
              laKeyWord.toLowerCase().trim()
            )
          : info.Username
      )
    : laInfo;
  return {
    laInfo: newLaInfo,
    rest
  };
};

const filterBySectionColumn = props => {
  const { laInfo } = props;
  const { sectionKeyWord } = props.rest;
  return sectionKeyWord
    ? laInfo.filter(info =>
        info.Section
          ? includes(
              info.Section.toLowerCase().trim(),
              sectionKeyWord.toLowerCase().trim()
            )
          : info.Section
      )
    : laInfo;
};

const pipe = (...fns) => x => fns.reduce((y, func) => func(y), x);

const executeFilter = (laInfo, laKeyWord, sectionKeyWord) => {
  const filterPipe = pipe(filterByLAColumn, filterBySectionColumn);
  const filterModel = {
    laInfo,
    laKeyWord,
    sectionKeyWord
  };
  return filterPipe(filterModel);
};

const isDateRangeValid = (fromDate, toDate) => {
  const { millisecondsPerDay, dateRange } = FilterConst;
  return (
    (toDate.getTime() - fromDate.getTime()) / millisecondsPerDay - dateRange > 0
  );
};

export { executeFilter, isDateRangeValid };
