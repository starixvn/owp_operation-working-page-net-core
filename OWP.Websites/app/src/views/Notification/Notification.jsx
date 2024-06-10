import { Component } from 'react';
import AuthorizeService from '../Authorize/Service';
import List from './List';
import Loading from '../components/Loading';
import { FunctionKeys } from '../Shared/Constants';

export default class Notification extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isLoading: true,
      activeTab: '1',
      notifications: []
    };
  }

  componentDidMount() {
    (async () => {
      const policy = await AuthorizeService.authorize(
        FunctionKeys.NotificationSetting_MySectionMessage
      );
      this.setState({
        policy,
        isLoading: false
      });
    })();
  }

  render() {
    const { isLoading } = this.state;

    if (isLoading) {
      return <Loading />;
    } else {
      return (
        <div className='notification-setting animated fadeIn'>
          <List policy={this.state.policy} />
        </div>
      );
    }
  }
}
