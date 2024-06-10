import renderGroupHeader from './renderGroupHeader';
import renderColumn from './renderColumn';

const renderSpread = (dataSource, spreadHeaderInfo) => {
  const { hdp, ou, onehHdp, onehOu } = spreadHeaderInfo.subHeader;
  const renderSubheaderSpread = () => {
    return [
      renderColumn(dataSource, hdp),
      renderColumn(dataSource, ou),
      renderColumn(dataSource, onehHdp),
      renderColumn(dataSource, onehOu)
    ];
  };
  return renderGroupHeader(spreadHeaderInfo, renderSubheaderSpread);
};

export default renderSpread;
