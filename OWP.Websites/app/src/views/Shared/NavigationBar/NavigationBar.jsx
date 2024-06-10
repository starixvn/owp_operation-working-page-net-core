import PropTypes from 'prop-types';

const propTypes = {
  onUpdateNavigate: PropTypes.func,
  navbarList: PropTypes.arrayOf(PropTypes.object),
  activeTabId: PropTypes.string.isRequired
};

const NavigationBar = ({ onUpdateNavigate, navbarList, activeTabId }) => {
  const handleSelectChange = id => {
    onUpdateNavigate(id);
  };

  return (
    <div className='navigation-bar'>
      {navbarList.map(nav => {
        return (
          <a
            key={nav.id}
            onClick={() => handleSelectChange(nav.id)}
            className={`navigation-bar-item ${
              activeTabId === nav.id ? 'active' : ''
            }`}>
            {nav.label}
          </a>
        );
      })}
    </div>
  );
};

NavigationBar.propTypes = propTypes;
export default NavigationBar;
