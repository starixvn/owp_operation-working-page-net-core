import { render } from '@testing-library/react';
import ReadOnlyRemark from './ReadOnlyRemark';

const createProps = Remark => {
  const dataSource = [
    {
      Remark
    }
  ];
  const rowIndex = 0;
  return { dataSource, rowIndex };
};

describe('ReadOnlyRemark under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render Remark', () => {
    const props = createProps('RemarkTest');
    const { container } = render(<ReadOnlyRemark {...props} />);
    const element = container.querySelector('#Remark0');
    expect(element.textContent).toEqual('RemarkTest');
  });

  it('when remark empty should not render Remark', () => {
    const props = createProps(null);
    const { container } = render(<ReadOnlyRemark {...props} />);
    const element = container.querySelector('#Remark0');
    expect(element.textContent).toEqual('');
  });
});
