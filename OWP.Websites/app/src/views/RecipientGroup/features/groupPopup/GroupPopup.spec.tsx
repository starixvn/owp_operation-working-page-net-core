import { fireEvent, render, screen, waitFor } from '@testing-library/react';
import { Dispatch, SetStateAction } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { GroupDataSource } from '../Type';
import GroupPopup from './GroupPopup';
import { post } from '../../../Shared/Service/Service';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

jest.mock('../../../Shared/Service/Service', () => {
  return {
    post: jest.fn(() => Promise.resolve({ data: '1,2,3' }))
  };
});

const mockDispatch = jest.fn();
const mockSetIsOpenPopup = jest.fn() as Dispatch<SetStateAction<boolean>>;

const setUp = (isOpenPopup: boolean = false) => {
  (useDispatch as jest.Mock).mockReturnValue(mockDispatch);

  (useSelector as jest.Mock).mockImplementation(selectorFn =>
    selectorFn({
      recipientGroup: {
        groups: [
          {
            groupId: 1,
            groupName: 'LST',
            recipients: []
          }
        ],
        loading: 'pending'
      },
      policy: {
        permission: { Username: 'sa', UserId: 8, HasWrite: true, SiteId: 99 }
      }
    })
  );
};

const data = {
  groupId: 1,
  groupName: 'LST',
  recipients: [
    { recipientId: 1, recipientName: 'Mimi' },
    { recipientId: 2, recipientName: 'Mira' }
  ]
} as GroupDataSource;

describe('test GroupPopup', () => {
  beforeEach(() => {
    setUp();
  });
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('render GroupPopup, should render Form and 2 row', () => {
    render(
      <GroupPopup
        model={data}
        title='Edit'
        setIsOpenPopup={mockSetIsOpenPopup}
      />
    );
    waitFor(() => {
      expect(screen.queryByText('Save')).toBeInTheDocument();
    });
  });

  it('click button-cancel, should call mockSetIsOpenPopup', () => {
    render(
      <GroupPopup
        model={data}
        title='Edit'
        setIsOpenPopup={mockSetIsOpenPopup}
      />
    );
    fireEvent.click(screen.queryByText('Cancel')!);
    waitFor(() => {
      expect(mockSetIsOpenPopup).toHaveBeenCalledTimes(1);
    });
  });

  it('click save button, should call mockSetIsOpenPopup, mockDispatch', done => {
    (post as jest.Mock).mockResolvedValue(0);
    render(
      <GroupPopup
        model={data}
        title='Edit'
        setIsOpenPopup={mockSetIsOpenPopup}
      />
    );
    waitFor(() => {
      fireEvent.click(screen.queryByText('Save')!);
    });
    process.nextTick(async () => {
      expect(post).toHaveBeenCalledTimes(2);
      expect(mockSetIsOpenPopup).toHaveBeenCalledTimes(1);
      expect(mockDispatch).toHaveBeenCalledTimes(1);
      done();
    });
  });
});
