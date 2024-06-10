import { Suspense } from 'react';
import WizardWrapper from './WizardWrapper';
import {
  acceptedFileFormat,
  noFileIsSelected,
  fileExceed5Mb
} from './stepWizardValidator';
import setup from '../../Shared/testHelper/reduxEnv';
import { fireEvent, render } from '@testing-library/react';
import '@testing-library/jest-dom';

jest.mock('../lazyExport/LazyFileLoader', () => require('./FileLoader'));
jest.mock('../lazyExport/LazyValidation', () => require('./Validation'));
jest.mock('../lazyExport/LazyStepControl', () => require('./StepControl'));
jest.mock('../lazyExport/LazyWizardLoader', () =>
  require('../../Shared/WizardControl/Wizard')
);

const reduxInfo = {
  store: {
    validation: {
      validationResult: {
        isSuccessful: true
      }
    }
  },
  dispatchReturnValue: {
    isSuccessful: true
  },
  selectorReturnValue: {
    fileLoader: {
      fromDate: '25-06-2022',
      toDate: '26-06-2022',
      validationResult: {
        hasError: false
      },
      warning: {
        hasWarning: false
      }
    },
    validation: {
      validationResult: {},
      loading: true
    }
  }
};

const { store, dispatchReturnValue, selectorReturnValue } = reduxInfo;

const { mockedDispatch } = setup(
  store,
  dispatchReturnValue,
  selectorReturnValue
);

jest.mock('./stepWizardValidator', () => ({
  acceptedFileFormat: jest.fn(),
  noFileIsSelected: jest.fn(),
  fileExceed5Mb: jest.fn()
}));

const defaultProps = {
  onClose: jest.fn()
};
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn()
}));

describe('WizardWrapper under test', () => {
  beforeEach(() => {
    jest.resetModules();
    jest.clearAllMocks();
  });

  it('should call dispatch 1 time when no file is selected', () => {
    noFileIsSelected.mockReturnValue({ hasError: true });
    const { container } = render(
      <Suspense>
        <WizardWrapper {...defaultProps} />
      </Suspense>
    );
    const button = container.querySelector('.btn-next');
    fireEvent.click(button);
    expect(mockedDispatch).toHaveBeenCalledTimes(3);
  });

  it('should call dispatch 1 time when file is correct format', () => {
    noFileIsSelected.mockReturnValue({ hasError: false });
    acceptedFileFormat.mockReturnValue({ hasError: true });
    const { container } = render(
      <Suspense>
        <WizardWrapper {...defaultProps} />
      </Suspense>
    );
    const button = container.querySelector('.btn-next');
    fireEvent.click(button);
    expect(mockedDispatch).toHaveBeenCalledTimes(3);
  });

  it('should call dispatch 1 time when file exceed 5MB', () => {
    noFileIsSelected.mockReturnValue({ hasError: false });
    acceptedFileFormat.mockReturnValue({ hasError: false });
    fileExceed5Mb.mockReturnValue({ hasError: true });
    const { container } = render(
      <Suspense>
        <WizardWrapper {...defaultProps} />
      </Suspense>
    );
    const button = container.querySelector('.btn-next');
    fireEvent.click(button);
    expect(mockedDispatch).toHaveBeenCalledTimes(3);
  });

  it('should render button submit', () => {
    noFileIsSelected.mockReturnValue({ hasError: false });
    acceptedFileFormat.mockReturnValue({ hasError: false });
    fileExceed5Mb.mockReturnValue({ hasError: false });
    const { container } = render(
      <Suspense>
        <WizardWrapper {...defaultProps} />
      </Suspense>
    );
    const button = container.querySelector('.btn-next');
    fireEvent.click(button);
    expect(container.querySelectorAll('.btn-submit').length).toEqual(1);
  });

  it('should call dispatch when file pass validate', () => {
    noFileIsSelected.mockReturnValue({ hasError: false });
    acceptedFileFormat.mockReturnValue({ hasError: false });
    fileExceed5Mb.mockReturnValue({ hasError: false });
    const { container } = render(
      <Suspense>
        <WizardWrapper {...defaultProps} />
      </Suspense>
    );
    const buttonNext = container.querySelector('.btn-next');
    fireEvent.click(buttonNext);

    const buttonSubmit = container.querySelector('.btn-submit');
    fireEvent.click(buttonSubmit);

    expect(mockedDispatch).toHaveBeenCalledTimes(4);
  });

  it('should call dispatch when file pass validate', () => {
    noFileIsSelected.mockReturnValue({ hasError: false });
    acceptedFileFormat.mockReturnValue({ hasError: false });
    fileExceed5Mb.mockReturnValue({ hasError: false });
    const { container } = render(
      <Suspense>
        <WizardWrapper {...defaultProps} />
      </Suspense>
    );
    const button = container.querySelector('.btn-close');
    fireEvent.click(button);

    expect(defaultProps.onClose).toHaveBeenCalledTimes(1);
  });
});
