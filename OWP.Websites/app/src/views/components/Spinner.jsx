import { Component } from 'react';
import Loading from './Loading';

let spinnerContainer;

export class SpinnerContainer extends Component {
  constructor(props) {
    super(props);
    spinnerContainer = this;
    this.state = {
      isShow: false
    };
  }

  show() {
    this.setState({ isShow: true });
  }

  hide() {
    this.setState({ isShow: false });
  }

  render() {
    const { isShow } = this.state;
    return isShow ? <Loading /> : <div />;
  }
}

const Spinner = {
  show: () => spinnerContainer.show(),
  hide: () => spinnerContainer.hide()
};
export default Spinner;
