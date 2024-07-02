/**
 * @jest-environment jsdom
 */
import { setImmediate } from 'timers';
import ValidationSuccess from './ValidationSuccess';
import ToastrWrapper from '../../components/ToastrWrapper';
import { useSelector, useDispatch } from 'react-redux';
import { render } from '@testing-library/react';
import '@testing-library/jest-dom';

jest.mock('../../components/ToastrWrapper');

function flushPromises() {
  return new Promise(resolve => setImmediate(resolve));
}

jest.setTimeout(30000);

ToastrWrapper.showError = jest.fn().mockReturnValue('OK');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockedDispatch = jest.fn();
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
    submit: {
      submitResult: {
        message: ''
      },
      loading: true
    }
  })
);
useDispatch.mockReturnValue(mockedDispatch);

const props = {
  totalRow: 10,
  onClose: jest.fn()
};

describe('ValidationSuccess under test', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should match snapshot', () => {
    const { container } = render(<ValidationSuccess {...props} />);
    expect(container).toMatchSnapshot();
  });

  it('should render <Loading/>', () => {
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
        submit: {
          loading: true,
          submitResult: {
            message: ''
          }
        }
      })
    );
    const { container } = render(<ValidationSuccess {...props} />);
    const loadingComponent = container.querySelector('.loading-process');

    expect(loadingComponent).toBeInTheDocument();
  });

  it('should render <CheckboxMarkedCircleOutlineIcon/>', () => {
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
        submit: {
          loading: true,
          submitResult: {
            message: ''
          }
        }
      })
    );
    const { container } = render(<ValidationSuccess {...props} />);
    const svgElement = container.querySelectorAll('svg');
    expect(svgElement.length).toBe(2);
  });

  it('should render success info', () => {
    const { container } = render(<ValidationSuccess {...props} />);
    const successInfo = container.querySelector('.success-info');
    expect(successInfo).toBeInTheDocument();
  });

  describe('useEffect under test for message submit', () => {
    beforeEach(() => {
      jest.clearAllMocks();
      useSelector.mockImplementation(selectorFn =>
        selectorFn({
          fileLoader: {
            fromDate: '25-06-2022',
            toDate: '26-06-2022',
            warning: {
              hasWarning: false,
              messageWarning: ''
            }
          },
          submit: {
            submitResult: {
              message: 'Os submit successfully'
            },
            loading: false
          }
        })
      );
    });
    it('should call dispatch', async () => {
      render(<ValidationSuccess {...props} />);
      await flushPromises();
      expect(mockedDispatch).toHaveBeenCalled();
    });

    it('should call onClose', async () => {
      render(<ValidationSuccess {...props} />);
      await flushPromises();
      expect(props.onClose).toHaveBeenCalled();
    });
  });
  describe('useEffect under test for warning message', () => {
    beforeEach(() => {
      jest.clearAllMocks();
      useSelector.mockImplementation(selectorFn =>
        selectorFn({
          fileLoader: {
            fromDate: '25-06-2022',
            toDate: '26-06-2022',
            warning: {
              hasWarning: true,
              messageWarning: 'abc'
            }
          },
          submit: {
            submitResult: {
              message: ''
            },
            loading: false
          }
        })
      );
    });
    it('should call dispatch', async () => {
      render(<ValidationSuccess {...props} />);
      await flushPromises();
      expect(mockedDispatch).toHaveBeenCalled();
    });
  });
});
