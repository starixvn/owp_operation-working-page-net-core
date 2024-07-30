import PropTypes from 'prop-types';

import { DefaultCellTrader, DefaultCellProvider } from '../../TableSchema.json';
import BettypeLabel from './BettypeLabel';
import Assignment from './Assignment';
import CollapseExpandTrader from './CollapseExpandTrader';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const TraderCell = props => {
  const { dataSource, rowIndex } = props;
  const { traderMaxbetModels } = dataSource[rowIndex];
  const traderMaxbetModelsCanDisplay = traderMaxbetModels.filter(
    traderMaxbet =>
      !traderMaxbet.hiden || typeof traderMaxbet.hiden === 'undefined'
  );

  const renderListTraderElement = traderMaxbetModelList => {
    const height =
      traderMaxbetModelList.length > 1
        ? DefaultCellTrader.height
        : DefaultCellProvider.height;
    return traderMaxbetModelList.map((traderMaxbetModel, index) => {
      return (
        <li
          style={{
            height
          }}
          key={index}>
          <div className='bettype-cell'>
            <BettypeLabel {...traderMaxbetModel} />
          </div>
          <div className='assignment-cell'>
            <Assignment
              {...traderMaxbetModel.traderAssignment}
              rowIndex={rowIndex}
            />
          </div>
        </li>
      );
    });
  };

  return (
    <>
      <div className='accordion-trader'>
        <ul className='list-trader-os'>
          {renderListTraderElement(traderMaxbetModelsCanDisplay)}
        </ul>
        <div className='cell-accordion'>
          {traderMaxbetModels.length > 1 && <CollapseExpandTrader {...props} />}
        </div>
      </div>
    </>
  );
};
TraderCell.propTypes = propTypes;
export default TraderCell;
