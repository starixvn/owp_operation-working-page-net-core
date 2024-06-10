import NoteRow from './NoteRow';
import { fireEvent, render, screen } from '@testing-library/react';

describe('NoteRow', () => {
  const onDelete = jest.fn();
  const onSave = jest.fn();
  const defaultProps = {
    policy: {},
    index: 1,
    row: {},
    onDelete,
    onSave
  };

  describe("test policy's HasWrite = true", () => {
    const policy = {
      HasWrite: true
    };
    const props = {
      ...defaultProps,
      policy,
      row: {
        Username: 'leo',
        Note: 'ahihi'
      }
    };

    it('should match snapshot in case isEditing=false', () => {
      const component = render(<NoteRow {...props} />);

      expect(component.container.firstChild).toMatchSnapshot();
    });

    it('should match snapshot in case isEditing=true', () => {
      const { container } = render(<NoteRow {...props} />);
      expect(container.firstChild).toMatchSnapshot();
    });
  });

  describe("test policy's HasWrite = false", () => {
    const policy = {
      HasWrite: false
    };
    const props = {
      ...defaultProps,
      policy,
      row: {
        Username: 'leo',
        Note: 'ahihi'
      }
    };

    it('should match snapshot', () => {
      const { container } = render(<NoteRow {...props} />);
      expect(container.firstChild).toMatchSnapshot();
    });
  });

  describe('test actions', () => {
    const policy = {
      HasWrite: true
    };

    describe('View mode', () => {
      let props;
      beforeEach(() => {
        props = {
          ...defaultProps,
          policy,
          row: {
            Username: 'leo',
            Note: 'ahihi'
          }
        };
      });

      it('should update state when edit a specific row', () => {
        const { container } = render(<NoteRow {...props} />);;

        fireEvent.click(container.querySelector('.icon-edit'));

        expect(container.querySelector('.note-textinput')).toBeInTheDocument();
      });
    });

    describe('Edit mode with same data', () => {
      it('should update state but not trigger onSave when save a specific row', () => {
        const props = {
          ...defaultProps,
          policy,
          row: {
            Username: 'leo',
            Note: 'ahihi'
          }
        };
        onSave.mockClear();
        const { container, getByText } = render(<NoteRow {...props} />);

        fireEvent.click(container.querySelector('.icon-edit'));
        fireEvent.click(container.querySelector('.icon-save'));

        expect(getByText('ahihi')).toBeInTheDocument();
        expect(onSave).not.toBeCalled();
      });
    });

    describe('Edit mode with different data', () => {
      let props;

      beforeEach(() => {
        props = {
          ...defaultProps,
          policy,
          row: {
            Username: 'leo',
            Note: 'ahihi'
          }
        };

      });

      it('should update state & trigger onSave when save a specific row', () => {
        onSave.mockClear();
        const { container, getByText } = render(<NoteRow {...props} />);

        fireEvent.click(container.querySelector('.icon-edit'));
        fireEvent.change(container.querySelector('.note-textinput'), {
          target: {
            value: "vlxx"
          }
        })
        fireEvent.click(container.querySelector('.icon-save'));

        expect(getByText('vlxx')).toBeInTheDocument();
        expect(onSave).toBeCalledWith('vlxx');
      });

      it('should restore state when cancel to edit a specific row', () => {
        const { container, getByText } = render(<NoteRow {...props} />);

        fireEvent.click(container.querySelector('.icon-edit'));
        fireEvent.change(container.querySelector('.note-textinput'), {
          target: {
            value: "vlxx"
          }
        })
        fireEvent.click(container.querySelector('.icon-cancel'));

        expect(getByText('ahihi')).toBeInTheDocument();
      });
    });
  })
})
