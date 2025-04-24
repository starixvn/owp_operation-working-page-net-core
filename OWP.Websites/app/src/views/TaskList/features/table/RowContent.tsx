import { useState } from 'react';
import KOTimeCellFactory from '../factory/KOTimeCellFactory';
import IPTVCell from './cell/IPTVCell';
import LACell from './cell/LACell';
import LeagueCell from './cell/LeagueCell';
import NoCell from './cell/NoCell';
import ProviderCell from './cell/ProviderCell';
import RemarkCell from './cell/RemarkCell';
import ReadyFactory from '../factory/ReadyFactory';
import { uniqueId } from 'lodash';
import TicketCell from './cell/TicketCell';
import MatchCellFactory from '../factory/MatchCellFactory';
import FtHdpTraderCell from './cell/FtHdpTraderCell';
import FtOuTraderCell from './cell/FtOuTraderCell';
import FirstHalfHdpTraderCell from './cell/FirstHalfHdpTraderCell';
import FirstHalfOuTraderCell from './cell/FirstHalfOuTraderCell';
import { DataSource } from '../../type/TTaskListTable';
import { buildSpecialListModel } from '../utils/utils';
import { twoRow, two } from '../const/TableSchema.json';
import OtherProductTraderCell from './cell/OtherProductTraderCell';

type Props = {
  rowIndex: number;
  dataSource: DataSource;
  hasDetailPermission: boolean;
  firstMatchCheckedIsSaba: boolean;
};

const RowContent = ({
  rowIndex,
  dataSource,
  hasDetailPermission,
  firstMatchCheckedIsSaba
}: Props) => {
  const { Rowspan } = dataSource;
  const [isHovering, setIsHovering] = useState(false);

  const handleMouseOver = () => {
    setIsHovering(true);
  };

  const handleMouseOut = () => {
    setIsHovering(false);
  };

  const rowClass = isHovering
    ? `row-hover`
    : `${dataSource.MatchStatus === 'Cancel' ? 'match-cancelled' : ' '}
     ${rowIndex % two === 1 ? 'odd' : 'even'}`;
  const rowSpanMaxBet = Rowspan > twoRow ? twoRow : 1;
  const ouSpecificBetTypeGroupListModel = buildSpecialListModel(dataSource);

  const buildElementOtherTrader = () => {
    return ouSpecificBetTypeGroupListModel.map(
      (value, index) =>
        ouSpecificBetTypeGroupListModel[index + 1] && (
          <tr className={rowClass} key={`other-${rowIndex}-${index}`}>
            <>
              <OtherProductTraderCell
                name={ouSpecificBetTypeGroupListModel[index + 1].BetTypeName}
                maxBet={ouSpecificBetTypeGroupListModel[index + 1].Maxbet}
                traders={
                  ouSpecificBetTypeGroupListModel[index + 1].MainAssignment
                }
                others={
                  ouSpecificBetTypeGroupListModel[index + 1].OthersAssignment
                }
                rowIndex={rowIndex}
              />
            </>
          </tr>
        )
    );
  };
  return (
    <>
      <tr
        className={rowClass}
        onMouseOver={handleMouseOver}
        onMouseOut={handleMouseOut}>
        <td rowSpan={Rowspan}>
          <NoCell
            hasDetailInformationPermission={hasDetailPermission}
            dataSource={dataSource}
            rowIndex={rowIndex}
            firstMatchCheckedIsSaba={firstMatchCheckedIsSaba}
          />
        </td>
        <td rowSpan={Rowspan}>
          <KOTimeCellFactory dataSource={dataSource} rowIndex={rowIndex} />
        </td>
        <td rowSpan={Rowspan}>
          <LeagueCell dataSource={dataSource} rowIndex={rowIndex} />
        </td>
        <td rowSpan={Rowspan}>
          <MatchCellFactory
            hasDetailInformationPermission={hasDetailPermission}
            dataSource={dataSource}
            rowIndex={rowIndex}
          />
        </td>
        <td rowSpan={Rowspan}>
          <TicketCell dataSource={dataSource} rowIndex={rowIndex} />
        </td>
        <td rowSpan={Rowspan}>
          <RemarkCell dataSource={dataSource} rowIndex={rowIndex} />
        </td>
        <>
          <FtHdpTraderCell
            rowSpan={rowSpanMaxBet}
            dataSource={dataSource}
            rowIndex={rowIndex}
          />
        </>
        <>
          <FirstHalfHdpTraderCell
            rowSpan={rowSpanMaxBet}
            dataSource={dataSource}
            rowIndex={rowIndex}
          />
        </>
        <>
          <OtherProductTraderCell
            name='FT1X2'
            maxBet={dataSource.MatchSetting.OneXTwo}
            traders={dataSource.Assignment.OneXTwo}
            others={dataSource.Assignment.OneXTwoOthers}
            rowIndex={rowIndex}
          />
        </>
        <td rowSpan={Rowspan}>
          <LACell dataSource={dataSource} rowIndex={rowIndex} />
        </td>
        <td rowSpan={Rowspan}>
          <IPTVCell dataSource={dataSource} rowIndex={rowIndex} />
        </td>
        <td rowSpan={Rowspan}>
          <ProviderCell dataSource={dataSource} rowIndex={rowIndex} />
        </td>
        <td rowSpan={Rowspan}>
          <ReadyFactory
            key={uniqueId()}
            dataSource={dataSource}
            rowIndex={rowIndex}
          />
        </td>
      </tr>
      {Rowspan > twoRow && (
        <tr className={rowClass}>
          <>
            <OtherProductTraderCell
              name='HT1X2'
              maxBet={dataSource.MatchSetting.OneXTwo1st}
              traders={dataSource.Assignment.OneXTwo1st}
              others={dataSource.Assignment.OneXTwo1stOthers}
              rowIndex={rowIndex}
            />
          </>
        </tr>
      )}
      <tr
        className={rowClass}
        onMouseOver={handleMouseOver}
        onMouseOut={handleMouseOut}>
        <>
          <FtOuTraderCell
            rowSpan={Rowspan - rowSpanMaxBet}
            dataSource={dataSource}
            rowIndex={rowIndex}
          />
        </>
        <>
          <FirstHalfOuTraderCell
            rowSpan={Rowspan - rowSpanMaxBet}
            dataSource={dataSource}
            rowIndex={rowIndex}
          />
        </>
        {rowSpanMaxBet === 1 && (
          <>
            <OtherProductTraderCell
              name='HT1X2'
              maxBet={dataSource.MatchSetting.OneXTwo1st}
              traders={dataSource.Assignment.OneXTwo1st}
              others={dataSource.Assignment.OneXTwo1stOthers}
              rowIndex={rowIndex}
            />
          </>
        )}
        {Rowspan - rowSpanMaxBet >= 1 &&
          ouSpecificBetTypeGroupListModel.length > 0 && (
            <>
              <OtherProductTraderCell
                name={ouSpecificBetTypeGroupListModel[0].BetTypeName}
                maxBet={ouSpecificBetTypeGroupListModel[0].Maxbet}
                traders={ouSpecificBetTypeGroupListModel[0].MainAssignment}
                others={ouSpecificBetTypeGroupListModel[0].OthersAssignment}
                rowIndex={rowIndex}
              />
            </>
          )}
      </tr>
      {ouSpecificBetTypeGroupListModel.length > 0 && buildElementOtherTrader()}
    </>
  );
};
export default RowContent;
