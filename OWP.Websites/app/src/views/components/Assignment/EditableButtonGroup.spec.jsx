import EditableButtonGroup from './EditableButtonGroup';
import EditableTextSuggestion from './EditableTextSuggestion';
import EditableTextMention from './EditableTextMention';
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import '@testing-library/jest-dom';
const middleWares = [];
const mockStore = configureStore(middleWares);
EditableTextSuggestion.displayName = 'EditableTextSuggestion';
EditableTextMention.displayName = 'EditableTextMention';
const event = jest.fn();
const onSaved = jest.fn();
const onDeleted = jest.fn();
const store = mockStore({
  tableReducer: {
    filterDate: 'abc'
  }
});
const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;
describe('EditableButtonGroup Component Tests', () => {
  it('test onClickButtonPlus event change MainTrade ', async () => {
    const props = {
      policy: { HasRead: true, HasWrite: true },
      isAllowAdd: true,
      main: {
        AssignmentId: 211919,
        OWPMatchId: 279044,
        ParentMatchId: 279044,
        Bettype: 1,
        MinorBettypeId: null,
        AssignmentType: 2,
        UserId: 0,
        Username: null,
        AssignmentNote: 'kane'
      },
      others: [],
      data: {},
      rowIndex: null,
      index: 'EditableButtonGroup_Key',
      betType: null,
      event: event,
      onSaved: onSaved,
      onDeleted: onDeleted,
      matchId: 2468
    };

    const { container } = render(
      <Wrapper>
        <EditableButtonGroup {...props} />
      </Wrapper>
    );
    const findButtonAdd = container.getElementsByClassName('btn-add');
    const element = screen.getByRole('button');
    await userEvent.click(element);
    const mentionElement = container.querySelector('.auto-suggest');
    expect(mentionElement).toBeInTheDocument();
    expect(findButtonAdd).toHaveLength(1);
  });

  it('test onClickButtonPlus event change OtherTrader ', async () => {
    const props = {
      policy: { HasRead: true, HasWrite: true },
      isAllowAdd: true,
      main: {
        AssignmentId: 211919,
        OWPMatchId: 279044,
        ParentMatchId: 279044,
        Bettype: 1,
        MinorBettypeId: null,
        AssignmentType: 2,
        UserId: 8,
        Username: null,
        AssignmentNote: 'kane'
      },
      others: [
        {
          AssignmentId: 123,
          AssignmentNote: 'abc',
          AssignmentType: 2
        }
      ],
      data: {},
      rowIndex: null,
      index: 'EditableButtonGroup_Key',
      betType: null,
      event,
      onSaved,
      onDeleted,
      matchId: 2468
    };

    const { container } = render(
      <Wrapper>
        <EditableButtonGroup {...props} />
      </Wrapper>
    );
    const mentionElement = container.querySelector('.list-edit-mentions');
    expect(mentionElement).toBeInTheDocument();
  });
});
