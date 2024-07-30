import isEqual from 'lodash.isequal';
import { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Route, Switch, useHistory } from 'react-router-dom';
import Loading from '../../components/Loading';
import NavigationBar from '../../Shared/NavigationBar/NavigationBar';
import { queryPolicy } from '../actions/action';
import MaxbetConst from '../constants/MaxbetConst';
import { buildMaxbetUrl } from '../helper/commonHelper';
import { selectPolicy } from '../selector/SelectorPolicy';
import Maxbet from './maxbet/Maxbet';
import MaxbetDetail from './maxbet/maxbetdetail/MaxbetDetail';
import MaxbetSettingTab from './maxbetSetting/MaxbetSettingTab';
import SpecialProduct from './SpecialProduct';
import LeagueInfoManagement from './leagueInfo/LeagueInfoManagement';

const MaxbetManagement = () => {
  const dispatch = useDispatch();
  const [navigationId, setNavigationId] = useState(MaxbetConst.leagueInfo.id);
  const maxbetUrl = buildMaxbetUrl();
  const model = useSelector(selectPolicy);
  const stableDispatch = useCallback(dispatch, [dispatch]);
  const history = useHistory();

  useEffect(() => {
    stableDispatch(queryPolicy());
  }, [stableDispatch]);

  const navbarList = [
    { ...MaxbetConst.leagueInfo },
    { ...MaxbetConst.maxbetInfo },
    { ...MaxbetConst.SpecialLiveProductInfo },
    { ...MaxbetConst.maxbetSetting }
  ];

  const handleNavigation = id => {
    history.push(maxbetUrl.maxbetGeneralUrl);
    setNavigationId(id);
  };

  if (model.policyPending) {
    return <Loading />;
  }
  return (
    <>
      <NavigationBar
        onUpdateNavigate={id => handleNavigation(id)}
        navbarList={navbarList}
        activeTabId={navigationId}
      />
      {isEqual(navigationId, MaxbetConst.leagueInfo.id) && (
        <LeagueInfoManagement />
      )}
      {isEqual(navigationId, MaxbetConst.maxbetInfo.id) && (
        <Switch>
          <Route
            exact
            path={`${maxbetUrl.maxbetGeneralUrl}`}
            render={() => <Maxbet />}
          />
          <Route
            exact
            path={`${maxbetUrl.maxbetDetailUrl}`}
            render={props => <MaxbetDetail {...props} />}
          />
        </Switch>
      )}
      {isEqual(navigationId, MaxbetConst.SpecialLiveProductInfo.id) && (
        <SpecialProduct />
      )}
      {isEqual(navigationId, MaxbetConst.maxbetSetting.id) && (
        <MaxbetSettingTab />
      )}
    </>
  );
};
export default MaxbetManagement;
