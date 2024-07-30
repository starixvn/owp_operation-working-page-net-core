import { TCellModel } from '../../type/type';
import ReactTooltip from 'react-tooltip';

const MBCell = (props: TCellModel) => {
  const { dataSource, rowIndex } = props;
  const { OWPMatchId, HandicapSetting, OverUnderSetting } =
    dataSource[rowIndex];

  const renderMaxbetSpreadMbc = () => {
    const uuidKey = `mb_${OWPMatchId}_${rowIndex}`;
    const isDisplayHDPSetting =
      HandicapSetting?.SpreadToDisplay || OverUnderSetting?.SpreadToDisplay;
    const isHDPSpreadExisting = HandicapSetting?.SpreadToDisplay;

    return (
      <div>
        <span data-for={uuidKey} data-tip='Maxbet/Spread of MBC'>
          {HandicapSetting?.MaxBetToDisplay}
          {isDisplayHDPSetting && (
            <span>({HandicapSetting?.SpreadToDisplay}</span>
          )}
          {OverUnderSetting?.SpreadToDisplay && (
            <>
              {isHDPSpreadExisting && <span>-</span>}
              {OverUnderSetting.SpreadToDisplay}
            </>
          )}
          {isDisplayHDPSetting && <span>)</span>}
        </span>
        <ReactTooltip place='right' id={uuidKey} effect='solid' />
      </div>
    );
  };

  return <div className='maxbet-cell'>{renderMaxbetSpreadMbc()}</div>;
};

export default MBCell;
