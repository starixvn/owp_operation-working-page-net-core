import PropTypes from 'prop-types';
import CloseCircleIcon from 'mdi-react/CloseCircleIcon';

const propTypes = {
  validationResults: PropTypes.array.isRequired
};

const ValidationError = ({ validationResults = [] }) => {
  return (
    <>
      <div className='error-info'>
        <CloseCircleIcon size={20} />
        <span>Number of failure(s): {validationResults.length}</span>
      </div>
      <div className="overlayTable">
        <table className='table'>
          <thead>
            <tr>
              <th>Row</th>
              <th>Message</th>
            </tr>
          </thead>
          <tbody>
            {validationResults.map((result, index) =>
              <tr key={index}>
                <td><span>{result.row}</span></td>
                <td><span>{result.message}</span></td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </>
  );
};

ValidationError.propTypes = propTypes;

export default ValidationError;
