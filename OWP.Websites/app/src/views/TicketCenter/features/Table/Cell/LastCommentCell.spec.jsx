import { render } from '@testing-library/react';
import LastCommentCell from './LastCommentCell';

jest.mock('../../../app/storeNative.tsx');

const props = {
  rowIndex: 0,
  dataSource: [
    {
      messages: [
        {
          username: 'esther',
          createdDate: '2020-09-17 01:14:51.750',
          message: 'Hello world1'
        },
        {
          username: 'carlos',
          message: 'Hello world2',
          createdDate: '2020-09-17 01:14:51.750'
        },
        {
          username: 'Liam',
          message: 'test Last Comment',
          createdDate: '2021-09-17 01:14:51.750'
        }
      ]
    }
  ]
};

describe('LastCommentCell under test', () => {
  it('should render Username', () => {
    const { container } = render(<LastCommentCell {...props} />);
    const element = container.querySelector(`[class="comment-name"]`);
    expect(element).toBeInTheDocument();
  });

  it('should render CreatedDate', () => {
    const { container } = render(<LastCommentCell {...props} />);
    const element = container.querySelector(`[class="comment-name"]`);
    expect(element).toBeInTheDocument();
  });

  it('should render Message', () => {
    const { container } = render(<LastCommentCell {...props} />);
    const element = container.querySelector(`[class="comment-message"]`);
    expect(element).toBeInTheDocument();
  });
});
