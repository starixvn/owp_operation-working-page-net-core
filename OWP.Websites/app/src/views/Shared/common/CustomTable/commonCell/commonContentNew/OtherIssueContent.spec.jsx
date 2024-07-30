import { render } from '@testing-library/react';
import OtherIssueContent from './OtherIssueContent';

const props = {
  subject: 'test'
};

describe('OtherIssueContent under test', () => {
  it('should render subject', () => {
    const { container } = render(<OtherIssueContent {...props} />);
    expect(container.querySelector('.new-add-league-name')).toBeInTheDocument();
  });
});
