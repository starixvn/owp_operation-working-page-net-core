import { Component } from 'react';
import PropTypes from 'prop-types';
import Sidebar from './Sidebar/Sidebar';
import { ToastContainer } from 'react-toastify';
import axios from 'axios';
import Loading from './components/Loading';
import Header from './Header/Header';
import HeaderService from './Header/Service';
import AuthorizeService from './Authorize/Service';
import { SpinnerContainer } from './components/Spinner';
import { FunctionKeys } from './Shared/Constants';
import { AppContext } from './Shared/Context';
import TicketLayoutWrapper from './TicketLayout/TicketLayoutWrapper';
import '../scss/fonts/icon-OWP/icon.scss';
import '../scss/style.scss';
import { setAxiosInterceptors } from './Shared/Service/ServiceCommon';

const propTypes = {
  view: PropTypes.object.isRequired,
  title: PropTypes.string.isRequired,
  isUseTicketLayout: PropTypes.bool
};
class Layout extends Component {
  constructor(props) {
    super(props);

    this.state = {
      isLoading: true,
      username: ''
    };
  }

  componentDidMount() {
    setAxiosInterceptors();

    (async () => {
      const [header, detailInformationPolicy, quickSearchPolicy] =
        await axios.all([
          HeaderService.query(this.props.title),
          AuthorizeService.authorize(FunctionKeys.DetailsInformation),
          AuthorizeService.authorize(FunctionKeys.QuickSearch)
        ]);
      this.setState({
        isLoading: false,
        header,
        detailInformationPolicy,
        quickSearchPolicy
      });
    })();
  }

  render() {
    const { isLoading, header, detailInformationPolicy, quickSearchPolicy } =
      this.state;
    if (isLoading) {
      return <Loading />;
    }

    const { title, view, isUseTicketLayout = true } = this.props;
    const {
      username,
      navigation,
      logoutLink,
      notificationSetingLink,
      accountSettingPath
    } = header;
    navigation.items.forEach(element => {
      element.attributes = { title: element.name };
    });
    return (
      <AppContext.Provider value={{ detailInformationPolicy }}>
        <div className='app'>
          <Header
            title={title}
            username={username}
            logoutLink={logoutLink}
            notificationSetingLink={notificationSetingLink}
            accountSettingLink={accountSettingPath}
            quickSearchPolicy={quickSearchPolicy}
          />
          <div className='cd-cover-layer' />
          <div className='app-body'>
            <Sidebar navigation={navigation}></Sidebar>
            <main className='main'>{view}</main>
          </div>
          <ToastContainer position='bottom-right' hideProgressBar />
          <SpinnerContainer />
          {isUseTicketLayout && <TicketLayoutWrapper />}
        </div>
      </AppContext.Provider>
    );
  }
}

Layout.propTypes = propTypes;
export default Layout;
