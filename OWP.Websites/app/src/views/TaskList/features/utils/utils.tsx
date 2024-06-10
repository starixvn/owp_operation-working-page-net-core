import { DataSource, ModelSpecialProduct } from '../../type/TTaskListTable';
import { ROOT_URL } from '../const/url.json';

const getNoteOrUserName = (
  note: string | null,
  isCurrentUser: boolean,
  userName: string
) => {
  const className = isCurrentUser ? 'my-assign' : 'assignee';
  const userNameText = () =>
    `<span
    className=${className}>
    ${userName}
  </span>`;
  return note ? note.replace('#<user>', userNameText()) : '';
};

const buildRequestURL = (key: string) =>
  process.env.NODE_ENV === 'development'
    ? `${ROOT_URL}/${key}`
    : `${window.netCoreRootUrl}/api/TaskList/${key}`;

const buildSpecialListModel = (dataSource: DataSource) => {
  const specialListModel: Array<ModelSpecialProduct> = [
    {
      IsMajorBetType: true,
      BetTypeName: 'FM',
      Maxbet: dataSource.MatchSetting.FastMarket,
      IsHasMaxbetSpread: dataSource.MatchSetting.FastMarket.IsHasMaxbetSpread,
      MainAssignment: dataSource.Assignment.FastMarket,
      OthersAssignment: dataSource.Assignment.FastMarketOthers
    },
    {
      IsMajorBetType: true,
      BetTypeName: 'AIO',
      Maxbet: null,
      IsHasMaxbetSpread: false,
      MainAssignment: dataSource.Assignment.AllInOne,
      OthersAssignment: dataSource.Assignment.AllInOneOthers
    },
    {
      IsMajorBetType: false,
      BetTypeName: '15mins',
      Maxbet: dataSource.MatchSetting.Specific15Mins,
      IsHasMaxbetSpread:
        dataSource.MatchSetting.Specific15Mins.IsHasMaxbetSpread,
      MainAssignment: dataSource.Assignment.Specific15Mins,
      OthersAssignment: dataSource.Assignment.Specific15MinsOthers
    },
    {
      IsMajorBetType: false,
      BetTypeName: '10mins',
      Maxbet: dataSource.MatchSetting.Specific10Mins,
      IsHasMaxbetSpread:
        dataSource.MatchSetting.Specific10Mins.IsHasMaxbetSpread,
      MainAssignment: dataSource.Assignment.Specific10Mins,
      OthersAssignment: dataSource.Assignment.Specific10MinsOthers
    }
  ];
  return specialListModel.filter(
    x => x.MainAssignment.length > 0 || x.IsHasMaxbetSpread
  );
};

export { getNoteOrUserName, buildRequestURL, buildSpecialListModel };
