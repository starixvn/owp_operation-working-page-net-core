import MatchSettingRow from './MatchSettingRow';
import { fireEvent, render, screen } from '@testing-library/react';

describe('<MatchSettingRow/>', () => {
  const matchSetting = {
    BettypeId: 1,
    Maxbet: 1,
    Spread: 2,
  };

  it('should render component <MatchSettingRow/> success', () => {
    const props = {
      matchSetting: matchSetting,
      policy: {
        HasWrite: true
      },
      isCreating: true,
      isEditing: false,
      allowEdit: true,
      MinorBettypeId: 1,
      onSave: jest.fn(),
      onCancel: jest.fn(),
      onEdit: jest.fn(),
      onDelete: jest.fn()
    };

    const { container } = render(<table><tbody>
      <MatchSettingRow {...props} />
    </tbody></table>);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('should hide column Action at mode edit', () => {
    const props = {
      matchSetting: matchSetting,
      policy: {
        HasWrite: false
      },
      isCreating: true,
      isEditing: false,
      allowEdit: true,
      MinorBettypeId: 1,
      onSave: jest.fn(),
      onCancel: jest.fn(),
      onEdit: jest.fn(),
      onDelete: jest.fn()
    };

    const { container } = render(<table><tbody>
      <MatchSettingRow {...props} />
    </tbody></table>);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('should update state when maxbet change', () => {
    const props = {
      matchSetting: matchSetting,
      policy: {
        HasWrite: false
      },
      isCreating: true,
      isEditing: true,
      allowEdit: true,
      MinorBettypeId: 1,
      onSave: jest.fn(),
      onCancel: jest.fn(),
      onEdit: jest.fn(),
      onDelete: jest.fn()
    };
    const { container } = render(<table><tbody>
      <MatchSettingRow {...props} />
    </tbody></table>);
    const maxbet = container.querySelectorAll('.input-field');

    fireEvent.change(maxbet[0], {
      target: {
        value: 4
      }
    });
    expect(maxbet[0].value).toBe('4');
  });

  it('should update state when spread change', () => {
    const props = {
      matchSetting: matchSetting,
      policy: {
        HasWrite: false
      },
      isCreating: true,
      isEditing: true,
      allowEdit: true,
      MinorBettypeId: 1,
      onSave: jest.fn(),
      onCancel: jest.fn(),
      onEdit: jest.fn(),
      onDelete: jest.fn()
    };
    const { container } = render(<table><tbody>
      <MatchSettingRow {...props} />
    </tbody></table>);
    const maxbet = container.querySelectorAll('.input-field');

    fireEvent.change(maxbet[1], {
      target: {
        value: 3
      }
    });
    expect(maxbet[1].value).toBe('3');
  });

  describe('onEdit/onCancel/onSave/onDelete tobe call when click Edit/Cancel/Save/Delete', () => {
    const onSave = jest.fn();
    const onCancel = jest.fn();
    const onEdit = jest.fn();
    const onDelete = jest.fn();
    const props = {
      matchSetting: matchSetting,
      policy: {
        HasWrite: true
      },
      isCreating: true,
      isEditing: true,
      allowEdit: true,
      MinorBettypeId: 1,
      onEdit,
      onSave,
      onCancel,
      onDelete
    };

    it('onEdit tobe call when click edit button', () => {
      onEdit.mockClear();
      const defaultProp = {
        ...props,
        isEditing: false,
        isCreating: false
      }
      const { container } = render(<table><tbody>
        <MatchSettingRow {
          ...defaultProp
        } />
      </tbody></table>);

      const editButton = container.querySelector('.icon-edit');
      fireEvent.click(editButton);
      expect(onEdit).toBeCalled();
    });

    it('onCancel tobe call when click cancel button', () => {
      onCancel.mockClear();
      const { container } = render(<table><tbody>
        <MatchSettingRow {
          ...props
        } />

      </tbody></table>);
      const editButton = container.querySelector('.icon-cancel');
      fireEvent.click(editButton);
      expect(onCancel).toBeCalled();
    });

    it('onDelete tobe call when click delete button', () => {
      onDelete.mockClear();
      onEdit.mockClear();
      const defaultProp = {
        ...props,
        isEditing: false,
        isCreating: false
      }
      const { container } = render(<table><tbody>
        <MatchSettingRow {
          ...defaultProp
        } />
      </tbody></table>);

      const editButton = container.querySelector('.icon-delete');
      fireEvent.click(editButton);
      expect(onDelete).toBeCalled();
    });

    it('onSave tobe call when data valid', () => {
      onCancel.mockClear();
      const { container } = render(<table><tbody>
        <MatchSettingRow {
          ...props
        } />

      </tbody></table>);
      const saveButton = container.querySelector('.icon-save');
      fireEvent.click(saveButton);
      expect(onSave).toBeCalled();
    });

    it('onSave not call when data invalid', () => {
      onSave.mockClear();
      const defaultProps = {
        ...props,
        matchSetting: {
          BettypeId: null,
          Maxbet: 1,
          Spread: 2,
        }
      }
      const { container } = render(<table><tbody>
        <MatchSettingRow {
          ...defaultProps
        } />

      </tbody></table>);
      const saveButton = container.querySelector('.icon-save');
      fireEvent.click(saveButton);
      expect(onSave).not.toBeCalled();
    });
  });
});
