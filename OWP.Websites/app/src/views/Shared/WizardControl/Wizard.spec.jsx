import Wizard from './Wizard';
import { fireEvent, render } from '@testing-library/react';

const defaultEvents = {
  onClose: jest.fn(),
  onStepChange: jest.fn(),
  onRenderContent: jest.fn(),
  onSubmit: jest.fn(),
  onCanNext: jest.fn(),
  onCanBack: jest.fn(),
  onCanSubmit: jest.fn(),
  onFinish: jest.fn()
};

const mockSetState = jest.fn();
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: initial => [initial, mockSetState]
}));

const defaultProps = {
  totalStep: 3,
  defaultStep: 0
};

describe('wizard under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
    jest.resetModules();
  });
  it('should match snapshot', () => {
    const { container } = render(
      <Wizard {...defaultEvents} {...defaultProps} />
    );
    expect(container).toMatchSnapshot();
  });

  it('should render close button', () => {
    const { container } = render(
      <Wizard {...defaultEvents} {...defaultProps} />
    );
    const btnClose = container.querySelector('.btn-close');
    expect(btnClose).toBeInTheDocument();
  });

  it('should call onRenderContent', () => {
    render(<Wizard {...defaultEvents} {...defaultProps} />);
    expect(defaultEvents.onRenderContent).toHaveBeenCalledTimes(1);
  });

  describe('back/next/submit button under test', () => {
    it('should render only button next', () => {
      const events = {
        ...defaultEvents,
        onCanNext: jest.fn().mockReturnValue(true)
      };
      const { container } = render(<Wizard {...events} {...defaultProps} />);
      expect(container.querySelector('.btn-next')).toBeInTheDocument();
      expect(container.querySelector('.btn-back')).not.toBeInTheDocument();
      expect(container.querySelector('.btn-submit')).not.toBeInTheDocument();
      expect(container.querySelector('.btn-finish')).not.toBeInTheDocument();
    });

    it('should call onStepchange', () => {
      const events = {
        ...defaultEvents,
        onStepChange: jest.fn().mockReturnValue(true),
        onCanNext: jest.fn().mockReturnValue(true)
      };

      const { container } = render(<Wizard {...events} {...defaultProps} />);
      fireEvent.click(container.querySelector('.btn-next'));
      expect(events.onStepChange).toHaveBeenCalledTimes(1);
    });

    it('should render button back/next', () => {
      const events = {
        ...defaultEvents,
        onCanBack: jest.fn().mockReturnValue(true),
        onCanNext: jest.fn().mockReturnValue(true)
      };
      const { container } = render(<Wizard {...events} {...defaultProps} />);
      expect(container.querySelector('.btn-next')).toBeInTheDocument();
      expect(container.querySelector('.btn-back')).toBeInTheDocument();
    });

    it('should call setCurrentStep', () => {
      const events = {
        ...defaultEvents,
        onCanBack: jest.fn().mockReturnValue(true)
      };
      const { container } = render(<Wizard {...events} {...defaultProps} />);
      fireEvent.click(container.querySelector('.btn-back'));
      expect(mockSetState).toHaveBeenCalledTimes(1);
    });

    it('should render button OK', () => {
      const events = {
        ...defaultEvents,
        onFinish: jest.fn().mockReturnValue(true)
      };
      const { container } = render(<Wizard {...events} {...defaultProps} />);
      expect(container.querySelector('.btn-finish')).toBeInTheDocument();
    });

    it('should call onClose', () => {
      const events = {
        ...defaultEvents,
        onClose: jest.fn(),
        onFinish: jest.fn().mockReturnValue(true)
      };
      const { container } = render(<Wizard {...events} {...defaultProps} />);
      fireEvent.click(container.querySelector('.btn-finish'));
      expect(events.onClose).toBeCalledTimes(1);
    });

    it('should render button submit button', () => {
      const events = {
        ...defaultEvents,
        onCanSubmit: jest.fn().mockReturnValue(true)
      };
      const { container } = render(<Wizard {...events} {...defaultProps} />);
      expect(container.querySelector('.btn-submit')).toBeInTheDocument();
    });

    it('should call onSubmit', () => {
      const events = {
        ...defaultEvents,
        onCanSubmit: jest.fn().mockReturnValue(true),
        onSubmit: jest.fn().mockReturnValue(true)
      };
      const { container } = render(<Wizard {...events} {...defaultProps} />);
      fireEvent.click(container.querySelector('.btn-submit'));
      expect(events.onSubmit).toBeCalledTimes(1);
    });

    it('should call setCurrentStep after submit', () => {
      const events = {
        ...defaultEvents,
        onCanSubmit: jest.fn().mockReturnValue(true),
        onSubmit: jest.fn().mockReturnValue(true)
      };
      const { container } = render(<Wizard {...events} {...defaultProps} />);
      fireEvent.click(container.querySelector('.btn-submit'));
      expect(mockSetState).toBeCalledTimes(1);
    });
  });
});
