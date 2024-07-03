import { Fragment } from 'react';
import {
  NoCell,
  KOTimeCell,
  LeagueCell,
  MatchCell,
  TicketCell,
  RemarkCell,
  OtherProductTraderCell,
  LaCell,
  IpTvStreamingCell,
  ProviderCell,
  FirstHalfHdpTraderCell,
  FirstHalfOuTraderCell,
  FtHdpTraderCell,
  FtOuTraderCell
} from '../cell';
import { v4 as uuidv4 } from 'uuid';
import {
  buildSpecialListModel,
  buildColorRowTable
} from '../LiveGameListTableHelper';
import { itemRowHeight } from '../TableSchema.json';

const TableBody = ({ data = '', displayStart = 0, displayEnd = 0 }) => {
  const rows = [];

  rows.push(
    <tr
      key='startRowFiller'
      style={{ height: displayStart * itemRowHeight }}></tr>
  );

  for (let i = displayStart; i < displayEnd; ++i) {
    const row = data[i];
    const rowColor = `${buildColorRowTable(row)} ${
      i % 2 === 1 ? 'odd-row' : 'even-row'
    }`;
    if (row != null) {
      const ouSpecificBetTypeGroupListModel = buildSpecialListModel(row);
      const buildElementOtherTrader = () => {
        return ouSpecificBetTypeGroupListModel.map(
          (_value, index) =>
            ouSpecificBetTypeGroupListModel[index + 1] && (
              <tr className={rowColor} key={`other-${i}-${index}`}>
                <OtherProductTraderCell
                  name={ouSpecificBetTypeGroupListModel[index + 1].BetTypeName}
                  maxBet={ouSpecificBetTypeGroupListModel[index + 1].Maxbet}
                  traders={
                    ouSpecificBetTypeGroupListModel[index + 1].MainAssignment
                  }
                  others={
                    ouSpecificBetTypeGroupListModel[index + 1].OthersAssignment
                  }
                  rowIndex={i}
                />
              </tr>
            )
        );
      };
      const rowSpan = row.Rowspan;
      const rowSpanMaxBet = rowSpan > 2 ? 2 : 1;
      rows.push(
        <Fragment key={uuidv4()}>
          <tr className={rowColor}>
            <td rowSpan={rowSpan}>
              <NoCell dataSource={data} rowIndex={i} />
            </td>
            <td rowSpan={rowSpan}>
              <KOTimeCell dataSource={data} rowIndex={i} />
            </td>
            <td rowSpan={rowSpan}>
              <LeagueCell dataSource={data} rowIndex={i} />
            </td>
            <td rowSpan={rowSpan}>
              <MatchCell dataSource={data} rowIndex={i} />
            </td>
            <td rowSpan={rowSpan}>
              <TicketCell dataSource={data} rowIndex={i} />
            </td>
            <td rowSpan={rowSpan}>
              <RemarkCell dataSource={data} rowIndex={i} />
            </td>
            <>
              <FtHdpTraderCell
                rowSpan={rowSpanMaxBet}
                dataSource={data}
                rowIndex={i}
              />
            </>
            <>
              <FirstHalfHdpTraderCell
                rowSpan={rowSpanMaxBet}
                dataSource={data}
                rowIndex={i}
              />
            </>
            <>
              <OtherProductTraderCell
                name='FT1X2'
                maxBet={row.MatchSetting.OneXTwo}
                traders={row.Assignment.OneXTwo}
                others={row.Assignment.OneXTwoOthers}
                rowIndex={i}
              />
            </>
            <td rowSpan={rowSpan}>
              <LaCell dataSource={data} rowIndex={i} />
            </td>
            <td rowSpan={rowSpan}>
              <IpTvStreamingCell dataSource={data} rowIndex={i} />
            </td>
            <td rowSpan={rowSpan}>
              <ProviderCell dataSource={data} rowIndex={i} />
            </td>
          </tr>
          {rowSpan > 2 && (
            <tr className={rowColor}>
              <OtherProductTraderCell
                name='HT1X2'
                maxBet={row.MatchSetting.OneXTwo1st}
                traders={row.Assignment.OneXTwo1st}
                others={row.Assignment.OneXTwo1stOthers}
                rowIndex={i}
              />
            </tr>
          )}
          <tr className={rowColor}>
            <FtOuTraderCell
              rowSpan={rowSpan - rowSpanMaxBet}
              dataSource={data}
              rowIndex={i}
            />
            <FirstHalfOuTraderCell
              rowSpan={rowSpan - rowSpanMaxBet}
              dataSource={data}
              rowIndex={i}
            />
            {rowSpanMaxBet === 1 && (
              <OtherProductTraderCell
                name='HT1X2'
                maxBet={row.MatchSetting.OneXTwo1st}
                traders={row.Assignment.OneXTwo1st}
                others={row.Assignment.OneXTwo1stOthers}
                rowIndex={i}
              />
            )}
            {rowSpan - rowSpanMaxBet >= 1 &&
              ouSpecificBetTypeGroupListModel.length > 0 && (
                <OtherProductTraderCell
                  name={ouSpecificBetTypeGroupListModel[0].BetTypeName}
                  maxBet={ouSpecificBetTypeGroupListModel[0].Maxbet}
                  traders={ouSpecificBetTypeGroupListModel[0].MainAssignment}
                  others={ouSpecificBetTypeGroupListModel[0].OthersAssignment}
                  rowIndex={i}
                />
              )}
          </tr>
          {ouSpecificBetTypeGroupListModel.length > 0 &&
            buildElementOtherTrader()}
        </Fragment>
      );
    }
  }
  rows.push(
    <tr
      key='endRowFiller'
      style={{ height: (data.length - displayEnd) * itemRowHeight }}></tr>
  );
  return <>{rows}</>;
};

export default TableBody;
