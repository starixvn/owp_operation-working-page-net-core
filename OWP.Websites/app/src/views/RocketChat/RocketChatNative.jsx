import { Component } from 'react';
import AuthorizeService from '../Authorize/Service';
import Loading from '../components/Loading';
import { FunctionKeys } from '../Shared/Constants';

export default class RocketChatNative extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isLoading: true
    };
  }

  componentDidMount() {
    (async () => {
      const policy = await AuthorizeService.authorize(FunctionKeys.RocketChat);
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
      return <div>Rocket Chat</div>;
    }
  }
}
