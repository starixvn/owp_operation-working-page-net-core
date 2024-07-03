import { useEffect } from 'react';
import { configSignalR } from './NetCoreSignalRService';
import PropTypes from 'prop-types';

const NetCoreSignalR = props => {
  const connection = configSignalR(props.hubName);

  const registerSignalRHandler = () => {
    try {
      if (connection) {
        props.handlers.forEach(x => {
          connection && connection.on(x.method, x.handler);
        });
      }
    } catch (error) {}
  };

  const executeDisconnect = () => {
    connection && connection.stop();
  };
  useEffect(() => {
    registerSignalRHandler();
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
