import { Link, useParams } from 'react-router-dom';
import toggleColumnWrapper from './toggleColumnWrapper';
import { buildMaxbetUrl } from '../../../helper/commonHelper';
import PropTypes from 'prop-types';
import isEqual from 'lodash.isequal';
import { useStore, useDispatch } from 'react-redux';

const Header = ({ history }) => {
  const { leagueName, isActive } = useParams();
  const url = buildMaxbetUrl();
  const store = useStore();
  const dispatch = useDispatch();
  return (
    <div className='header-breadcrumb'>
      <div className='left-panel'>
        {isEqual(history.length, 1) ? (
          <span className='league-list'> League list</span>
        ) : (

            <Link
              to={{
                pathname: `${url.maxbetGeneralUrl}`
              }}
              className='league-list'>
              League list
            </Link>
        )}
        <span className='league-name'>{leagueName}</span>
        <span className='active button-submit'>
          {isEqual(isActive.toLowerCase(), 'true') ? 'Active' : 'Inactive'}
        </span>
      </div>
      <div className='right-panel'>{toggleColumnWrapper(store, dispatch)}</div>
    </div>
  );
};
Header.propTypes = {
  history: PropTypes.object.isRequired
};

export default Header;
