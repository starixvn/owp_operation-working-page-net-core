import { fireEvent, render, screen } from '@testing-library/react';
import useStore from '../../app/store';
import EditTarget from './EditTarget';

const createProps = (isGap = true) =>
  useStore.setState({
    dataTable: {
      entities: [],
      dataSourcesFiltered: [],
      loadingStatus: 'idle',
      isGap: isGap
    },
    permission: {
      HasWrite: true,
      ActorId: 8,
      HasRead: true,
      SiteId: 99,
      Username: 'liam'
    }
  });

describe('EditTarget Component', () => {
  beforeEach(() => createProps());

  it('renders Edit Target span when isGap is true', () => {
    const { getByText } = render(<EditTarget />);

    const editTargetSpan = getByText('Edit Target');
    expect(editTargetSpan).toBeInTheDocument();
  });

  it('does not render Edit Target span when isGap is false', () => {
    createProps(false);
    const { queryByText } = render(<EditTarget />);

    const editTargetSpan = queryByText('Edit Target');
    expect(editTargetSpan).not.toBeInTheDocument();
  });

  it('opens the EditTargetPopup when clicked', () => {
    const { container } = render(<EditTarget />);

    const editTargetSpan = container.querySelector('.edit-target');

    fireEvent.click(editTargetSpan!);

    const editTargetPopup = screen.getByText('Username');
    expect(editTargetPopup).toBeInTheDocument();
  });
});
