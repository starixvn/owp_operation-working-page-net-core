import { render } from '@testing-library/react';
import { Ref } from 'react';
import GroupName from './GroupName';

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useRef: jest.fn()
}));

const ref = { current: { value: '222' } } as Ref<HTMLInputElement>;

describe('test GroupName', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('mount GroupName, should render Form', () => {
    const { container } = render(<GroupName ref={ref} defaultValue='' />);
    expect(container.querySelector('.group-form')).toBeInTheDocument();
  });
});
