import PropTypes from 'prop-types';
import FlagDetail from './FlagDetail';
import { FIFA_FLAG, PES_FLAG, PES_FLAG_23 } from './FlagStore/FlagsCondition';

const propTypes = {
  flagInfo: PropTypes.string,
  isExpand: PropTypes.bool
};

const FlagMatch = props => {
  const { flagInfo, isExpand } = props;
  const convertSize = (type, isExpandFlag) => {
    if (isExpandFlag) {
      switch (type) {
        case FIFA_FLAG.type:
          return { width: '172', height: '241' };
        case PES_FLAG.type:
          return { width: '337', height: '185' };
        case PES_FLAG_23.type:
          return { width: '376', height: '279' };
        default:
          return { width: '290', height: '280' };
      }
    }
    switch (type) {
      case FIFA_FLAG.type:
        return { width: '100', height: '150' };
      case PES_FLAG.type:
        return { width: '200', height: '100' };
      case PES_FLAG_23.type:
        return { width: '196', height: '145' };
      default:
        return { width: '175', height: '150' };
    }
  };

  const { width, height } = convertSize(flagInfo, isExpand);

  const sizeImage = { width, height };
  return <FlagDetail {...props} {...sizeImage} />;
};

FlagMatch.propTypes = propTypes;
export default FlagMatch;
