import { render, fireEvent, waitFor, screen } from '@testing-library/react';
import EditTargetPopup from './EditTargetPopup';
import useStore from '../../app/store';
import { TTotalGameInfo } from '../type/TableType.native';

describe('EditTargetPopup Component', () => {
  beforeEach(() => {
    useStore.setState({
      dataTable: {
        entities: [
          {
            userId: 1,
            userName: 'sa',
            progress: 'New',
            target: '',
            targetId: 2,
            gapWithTarget: {} as TTotalGameInfo,
            achievedGames: {} as TTotalGameInfo,
            reward: 5
          },
          {
            userId: 2,
            userName: 'mira',
            progress: 'Fail',
            target: '',
            targetId: 2,
            gapWithTarget: {} as TTotalGameInfo,
            achievedGames: {} as TTotalGameInfo,
            reward: 5
          }
        ],
        dataSourcesFiltered: [],
        loadingStatus: 'idle',
        isGap: true,
      },
      levelOption: [
        { value: 1, label: 'Level D' },
        { value: 2, label: 'New' },
      ]
    });
  });

  it('renders the EditTargetPopup component', () => {
    const { getByText } = render(<EditTargetPopup setIsOpenPopup={() => { }} />);
    const editTargetTitle = getByText('Edit Target');
    expect(editTargetTitle).toBeInTheDocument();
  });

  it('renders the Save button and cancels the popup', async () => {
    const setIsOpenPopup = jest.fn();

    const { getByText } = render(<EditTargetPopup setIsOpenPopup={setIsOpenPopup} />);
    const saveButton = getByText('Save');
    const cancelButton = getByText('Cancel');

    fireEvent.click(cancelButton);
    expect(setIsOpenPopup).toHaveBeenCalledWith(false);

    fireEvent.click(saveButton);

    await waitFor(() => {
      expect(setIsOpenPopup).toHaveBeenCalled();
    });
  });
});
