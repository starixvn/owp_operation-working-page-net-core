import PropTypes from 'prop-types';

const propTypes = {
  children: PropTypes.node,
  className: PropTypes.string.isRequired
};

const OWPBody = props => (
  <div className={`${props.className}-body`}>{props.children}</div>
);

const OWPFooter = props => (
  <div className={`${props.className}-footer`}>{props.children}</div>
);

const OWPHeader = props => (
  <div className={`${props.className}-header`}>{props.children}</div>
);

const OWPNavigationBar = props => (
  <div className={`${props.className}-navigationBar`}>{props.children}</div>
);

const OWPPage = props => (
  <div className={`${props.className}-page`}>{props.children}</div>
);

OWPPage.propTypes = propTypes;
OWPBody.propTypes = propTypes;
OWPFooter.propTypes = propTypes;
OWPHeader.propTypes = propTypes;
OWPNavigationBar.propTypes = propTypes;

export { OWPPage, OWPHeader, OWPBody, OWPFooter, OWPNavigationBar };
