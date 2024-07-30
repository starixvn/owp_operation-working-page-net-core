import PropTypes from 'prop-types';

const propTypes = {
  errors: PropTypes.array.isRequired
};

const ErrorTable = ({ errors = [] }) => {
  return (
    <>
      {errors.map((result, index) => (
        <div key={index}>
          <span>{result}</span>
        </div>
      ))}
    </>
  );
};

ErrorTable.propTypes = propTypes;

export default ErrorTable;
