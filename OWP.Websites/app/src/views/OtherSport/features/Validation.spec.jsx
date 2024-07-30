import { Suspense } from 'react';
import Validation from './Validation';
import { useSelector, useDispatch } from 'react-redux';
import ToastrWrapper from '../../components/ToastrWrapper';
import { setImmediate } from 'timers';
import { render } from '@testing-library/react';
import '@testing-library/jest-dom';

function flushPromises() {
  return new Promise(resolve => setImmediate(resolve));
}

jest.mock('../../components/ToastrWrapper');

jest.mock('../lazyExport/LazyValidationSuccess', () =>
  require('./ValidationSuccess')
);
jest.mock('../lazyExport/LazyValidationError', () =>
  require('./ValidationError')
);

ToastrWrapper.showError = jest.fn().mockReturnValue('OK');

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockedDispatch = jest.fn();

useDispatch.mockReturnValue(mockedDispatch);

const defaultProps = {
  onClose: jest.fn()
};

describe('Validation under test', () => {
  beforeEach(() => {
    jest.clearAllMocks();
    jest.resetModules();
  });

  it('should show <Loading/>', () => {
    useSelector.mockImplementation(selectorFn =>
      selectorFn({
        fileLoader: {
          fromDate: '25-06-2022',
          toDate: '26-06-2022',
          warning: {
            hasWarning: false,
            message: ''
          }
        },
        validation: {
          validationResult: {},
          loading: true
        }
      })
    );
    const { container } = render(<Validation {...defaultProps} />);
    const loadingComponent = container.querySelector('.loading-process');

    expect(loadingComponent).toBeInTheDocument();
  });

  it('should render error component', async () => {
    useSelector.mockImplementation(selectorFn =>
      selectorFn({
        fileLoader: {
          fromDate: '25-06-2022',
          toDate: '26-06-2022',
          warning: {
            hasWarning: false,
            message: ''
          }
        },
        validation: {
          validationResult: {
            validationResults: [],
            isSuccessful: false,
            recordCount: 10
          }
        },
        submit: {
          loading: true,
          submitResult: {
            message: ''
          }
        }
      })
    );

    const { container } = render(
      <Suspense fallback={<div>loading...</div>}>
        <Validation {...defaultProps} />
      </Suspense>
    );
    const validationError = container.querySelector('.error-info');
    expect(validationError).toBeInTheDocument();
  });

  it('should render success component', async () => {
    useSelector.mockImplementation(selectorFn =>
      selectorFn({
        fileLoader: {
          fromDate: '25-06-2022',
          toDate: '26-06-2022',
          warning: {
            hasWarning: false,
            message: ''
          }
        },
        validation: {
          validationResult: {
            validationResults: [],
            isSuccessful: true,
            recordCount: 10
          }
        },
        submit: {
          loading: true,
          submitResult: {
            message: 'Os submit successfully'
          }
        }
      })
    );

    const { container } = render(
      <Suspense fallback={<div>loading...</div>}>
        <Validation {...defaultProps} />
      </Suspense>
    );

    const validationSuccess = container.querySelector('.success-info');

    expect(validationSuccess).toBeInTheDocument();
  });

  it('should call onClose', async () => {
    useSelector.mockImplementation(selectorFn =>
      selectorFn({
        fileLoader: {
          fromDate: '25-06-2022',
          toDate: '26-06-2022',
          warning: {
            hasWarning: false,
            message: ''
          }
        },
        validation: {
          validationResult: {
            validationResults: [],
            isSuccessful: true,
            recordCount: 10
          }
        },
        submit: {
          loading: true,
          submitResult: {
            message: 'Os submit successfully'
          }
        }
      })
    );
    render(
      <Suspense fallback={<div>loading...</div>}>
        <Validation {...defaultProps} />
      </Suspense>
    );
    await flushPromises();
    expect(defaultProps.onClose).toHaveBeenCalledTimes(1);
  });
});
