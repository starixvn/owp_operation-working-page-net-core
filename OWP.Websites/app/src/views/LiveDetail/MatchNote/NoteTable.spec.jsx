import NoteTable from './NoteTable';
import Service from './Service';
import { fireEvent, render, screen } from '@testing-library/react';
import ToastrWrapper from '../../components/ToastrWrapper';

jest.mock('./Service');
jest.mock('../../components/ToastrWrapper');

ToastrWrapper.confirm.mockResolvedValue({
  value: true
});


const onSave = jest.fn();
describe('NoteTable', () => {
  const defaultProps = {
    matchId: 1,
    onSave,
    policy: {
      HasWrite: true
    },
    rows: []
  };

  const rows = [
    {
      MatchNoteId: 1,
      Username: 'sa',
      Note: 'abc'
    }
  ];

  beforeEach(() => {
    Service.delete.mockResolvedValue();
    Service.update.mockResolvedValue();
    Service.create.mockResolvedValue({
      MatchNoteId: 2,
      Username: 'leo'
    });
  });

  it('should render "There is no data" if data is empty', () => {
    const props = {
      ...defaultProps,
      onSave: jest.fn(),
      rows: []
    };
    const { container: { firstChild }, getByText } = render(<NoteTable {...props} />);
    expect(firstChild).toMatchSnapshot();
    expect(getByText('There is no data')).toBeInTheDocument();
  });

  describe('test input to create new note', () => {
    it('should trigger service to create note if input is not empty', () => {
      Service.create.mockClear();
      const props = {
        ...defaultProps,
        policy: {
          HasWrite: true
        }
      };
      const { container, getByTestId } = render(<NoteTable {...props} />);

      const input = screen.getByTestId('note-testId');

      fireEvent.change(input, { target: { value: 'test by Carlos' } });
      fireEvent.keyDown(input, { key: 'enter', keyCode: 13 })

      fireEvent.submit(input);

      expect(Service.create).toBeCalledWith({
        OWPMatchId: 1,
        Note: 'test by Carlos'
      });
    });
  });

  describe('test actions', () => {
    function setupComponent() {
      const props = {
        ...defaultProps,
        rows
      };
      const utils = render(<NoteTable {...props} />);
      return {
        ...utils
      };
    }

    it('should trigger service to update note when saving from row', () => {
      Service.update.mockClear();
      const { container } = setupComponent();

      fireEvent.click(container.querySelector('.icon-edit'));
      fireEvent.change(container.querySelectorAll('.note-textinput')[1], {
        target: {
          value: "vlxx"
        }
      })

      fireEvent.click(container.querySelector('.icon-save'));
      expect(Service.update).toBeCalled();
    });

    it('should trigger onSave note when deleting a row', () => {
      Service.delete.mockClear();
      const { container } = setupComponent();
      fireEvent.click(container.querySelector('.icon-delete'));
      expect(onSave).toBeCalled();
    });
  });
});
