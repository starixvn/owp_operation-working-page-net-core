import renderGroupHeader from './renderGroupHeader';
import renderColumn from './renderColumn';

const renderMaxbetWeekday = (dataSource, spreadHeaderInfo) => {
  const { hdp, ou, oneh, noHdp, matchOffer } = spreadHeaderInfo.subHeader;
  const renderSubheaderMaxbetWeekday = () => {
    return [
      renderColumn(dataSource, hdp),
      renderColumn(dataSource, ou),
      renderColumn(dataSource, oneh),
      renderColumn(dataSource, noHdp),
      renderColumn(dataSource, matchOffer)
    ];
  };
  return renderGroupHeader(spreadHeaderInfo, renderSubheaderMaxbetWeekday);
};

export default renderMaxbetWeekday;
