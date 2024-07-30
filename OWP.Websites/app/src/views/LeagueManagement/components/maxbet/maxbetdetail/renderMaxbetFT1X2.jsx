import renderGroupHeader from './renderGroupHeader';
import renderColumn from './renderColumn';

const renderMaxbetFT1X2 = (dataSource, maxbetFT1X2HeaderInfo) => {
  const {
    spread,
    ft1X2,
    oneh1X2,
    matchOffer
  } = maxbetFT1X2HeaderInfo.subHeader;
  const renderSubheaderMaxbetFT1X2 = () => {
    return [
      renderColumn(dataSource, spread),
      renderColumn(dataSource, ft1X2),
      renderColumn(dataSource, oneh1X2),
      renderColumn(dataSource, matchOffer)
    ];
  };
  return renderGroupHeader(maxbetFT1X2HeaderInfo, renderSubheaderMaxbetFT1X2);
};

export default renderMaxbetFT1X2;
