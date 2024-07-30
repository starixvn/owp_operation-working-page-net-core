import { Component } from 'react';
import PropTypes from 'prop-types';
import { ToastContainer } from 'react-toastify';
import '../scss/fonts/icon-OWP/icon.scss';
import '../scss/style.scss';
import { SpinnerContainer } from './components/Spinner';
import { setAxiosInterceptors } from './Shared/Service/ServiceCommon';

const propTypes = {
  view: PropTypes.object.isRequired
};

class IFrame extends Component {
  componentDidMount() {
    setAxiosInterceptors();
  }

  render() {
    const { view } = this.props;

    return (
      <div className='app app-iframe'>
        <div className='cd-cover-layer' />
        <div className='app-body'>{view}</div>
        <ToastContainer
          position='bottom-right'
          hideProgressBar
        />
        <SpinnerContainer />
      </div>
    );
  }
}

IFrame.propTypes = propTypes;
export default IFrame;
