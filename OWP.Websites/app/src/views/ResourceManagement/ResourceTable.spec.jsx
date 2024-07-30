import { fireEvent, render, waitFor } from '@testing-library/react';
import EditableRow from './EditableRow';
import ResourceTable from './ResourceTable';
import Service from './Service';

EditableRow.displayName = 'EditableRow';
const onEdit = jest.fn();
const onSave = jest.fn();
const onCancel = jest.fn();
const onChange = jest.fn();
const onKeyPress = jest.fn();

jest.mock('./Service');

describe('ResourceTable', () => {
  const response = { Id: 1, Value: '123', refName: 'MatchOffer' };
  Service.create.mockResolvedValue(response);

  describe('ResourceTable Render', () => {
    it('should match snapshot when dataSource is empty and HasWrite is equal true', () => {
      const props = {
        policy: { HasWrite: true },
        refName: 'MatchOffer',
        dataSource: []
      };
      const { container } = render(<ResourceTable {...props} />);
      expect(container.querySelector('.option-table')).toBeInTheDocument();
    });

    it('should match snapshot when dataSource is not null and HasWrite is equal true', () => {
      const props = {
        policy: { HasWrite: true },
        refName: 'MatchOffer',
        dataSource: [
          { Id: 1, Value: '1', RefName: 'MatchOffer' },
          { Id: 2, Value: '2', RefName: 'MatchOffer' }
        ]
      };
      const state = {
        valueText: '123',
        rows: props.dataSource
      };
      const { container } = render(<ResourceTable {...props} {...state} />);
      expect(container.querySelector('.add-value')).toBeInTheDocument();
    });

    it('test button Add when value duplicated', () => {
      const props = {
        policy: { HasWrite: true },
        refName: 'MatchOffer',

        dataSource: [
          { Id: 1, Value: '1', RefName: 'MatchOffer' },
          { Id: 2, Value: '2', RefName: 'MatchOffer' }
        ]
      };
      const state = {
        valueText: '1',
        rows: props.dataSource
      };
      const { container } = render(<ResourceTable {...props} {...state} />);
      const addButton = container.querySelector('.btn-add');
      fireEvent.click(addButton);
    });

    it('should match snapshot when dataSource is not null and HasWrite is equal false', () => {
      const props = {
        policy: { HasWrite: false },
        refName: 'MatchOffer',
        dataSource: [
          { Id: 1, Value: '1' },
          { Id: 2, Value: '2' }
        ]
      };
      render(<ResourceTable {...props} />);
    });
  });

  describe('ResourceTable Event Handle', () => {
    it("should trigger onChange when change value's value", () => {
      onChange.mockClear();
      const props = {
        policy: { HasWrite: true },
        refName: 'MatchOffer',
        dataSource: [{ id: 1, value: '1' }],
        valueText: '',
        onChange
      };
      const { container } = render(<ResourceTable {...props} />);
      const textbox = container.querySelector('.value-textinput');
      fireEvent.change(textbox, { target: { value: '123' } });
    });

    it("should trigger onKeyPress Enter when change value's value", async () => {
      const props = {
        policy: { HasWrite: true },
        refName: 'MatchOffer',
        dataSource: [{ Id: 1, Value: '1' }],
        valueText: '',
        onChange,
        onKeyPress
      };
      const { container, getByText } = render(<ResourceTable {...props} />);
      const textbox = container.querySelector('.value-textinput');
      fireEvent.change(textbox, { target: { value: '123' } });
      fireEvent.keyPress(textbox, {
        key: 'Enter',
        code: 13,
        charCode: 13
      });
      await waitFor(() => expect(getByText('123')).toBeInTheDocument());
    });

    it('test call Cancel function on editCell', () => {
      const props = {
        policy: { HasWrite: true },
        refName: 'MatchOffer',
        dataSource: [{ Id: 1, Value: '1' }]
      };
      const state = {
        isEditing: true,
        editingIndex: 0
      };
      const { container } = render(<ResourceTable {...props} {...state} />);

      const actionEdit = container.querySelector('.icon-edit');
      fireEvent.click(actionEdit);
      const actionCancel = container.querySelector('.icon-cancel');
      fireEvent.click(actionCancel);
    });

    it('test call Cancel function on newRow', () => {
      const dataSource = [{ Id: 1, Value: '1' }];
      const props = {
        policy: { HasWrite: true },
        refName: 'MatchOffer',
        dataSource: dataSource
      };
      const state = {
        isCreating: true,
        rows: dataSource
      };
      const { container } = render(<ResourceTable {...props} {...state} />);
      const actionEdit = container.querySelector('.icon-edit');
      fireEvent.click(actionEdit);
    });
  });
});
