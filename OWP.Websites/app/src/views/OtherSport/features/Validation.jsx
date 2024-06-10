import { Suspense } from 'react';
import { useSelector } from 'react-redux';
import Loading from '../../components/Loading';
import PropTypes from 'prop-types';
import LazyValidationSuccess from '../lazyExport/LazyValidationSuccess';
import LazyValidationError from '../lazyExport/LazyValidationError';

const propTypes = {
  onClose: PropTypes.func.isRequired
};

const Validation = ({ onClose }) => {
  const validation = useSelector(state => state.validation);
  const { validationResult, loading } = validation;
  const { validationResults, isSuccessful, recordCount } = validationResult;
  if (loading) {
    return <Loading />;
  }
  const componentValidation = isSuccessful ? (
    <LazyValidationSuccess totalRow={recordCount} onClose={onClose} />
  ) : (
    <LazyValidationError validationResults={validationResults} />
  );

  return <Suspense fallback={<Loading />}>{componentValidation}</Suspense>;
};

Validation.propTypes = propTypes;
export default Validation;
