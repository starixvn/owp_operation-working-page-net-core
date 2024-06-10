import renderGroupHeader from './renderGroupHeader';
import renderColumn from './renderColumn';

const renderCondition = (dataSource, conditionHeaderInfo, hasWriteGeneral) => {
  const { hasTV, round, division, edit } = conditionHeaderInfo.subHeader;
  const renderSubheaderCondition = () => {
    const condition = [
      renderColumn(dataSource, hasTV),
      renderColumn(dataSource, round),
      renderColumn(dataSource, division)
    ];
    hasWriteGeneral && condition.push(renderColumn(dataSource, edit));
    return condition;
  };
  return renderGroupHeader(conditionHeaderInfo, renderSubheaderCondition);
};

export default renderCondition;
