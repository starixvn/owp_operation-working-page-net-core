import { useEffect } from 'react';
import Spinner from '../../components/Spinner';
import { configSignalR } from './NetCoreSignalRService';
import PropTypes from 'prop-types';

const NetCoreSignalR = props => {
  const connection = configSignalR(props.hubName);

  const registerSignalRHandler = () => {
    debugger
    props.handlers.forEach(x => {
      connection.on(x.method, x.handler);
    });
  };

  const startConnect = async () => {
    if (connection) {
      await connection.stop();
      Spinner.show();
      
      connection
        .start()
        .then(() => {
          
          Spinner.hide();
          registerSignalRHandler();
        })
        .catch(err => {
          debugger
          console.error(err);
          Spinner.hide();
        });
    }
  };

  const executeDisconnect = () => {
    if (connection) {
      connection.stop();
    }
  };
  useEffect(() => {
    startConnect();
    return () => executeDisconnect();
    // eslint-disable-next-line no-warning-comments
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [connection, props.handlers]);
  return null;
};

NetCoreSignalR.propTypes = {
  hubName: PropTypes.string.isRequired,
  handlers: PropTypes.array.isRequired
};

export default NetCoreSignalR;
