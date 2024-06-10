import PropTypes from 'prop-types';

const propTypes = {
  children: PropTypes.object,
  classNameLiElement: PropTypes.string,
  classNameDivElement: PropTypes.string,
  title: PropTypes.string
};

const StepControlNodeTemplate = props => {
  const {
    classNameLiElement,
    classNameDivElement,
    title,
    children
  } = props;
  return (
    <li className={classNameLiElement} role='tab'>
      <div className={classNameDivElement}>
        {children}
      </div>
      <span>{title}</span>
    </li>
  );
};

StepControlNodeTemplate.propTypes = propTypes;

export default StepControlNodeTemplate;
