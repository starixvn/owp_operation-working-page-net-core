import { Option } from '../../../../Shared/MultipleSelection/type';
import { MultiSelect } from 'react-multi-select-component';
import { useAppDispatch, useAppSelector } from '../../../app/hookNative';
import { setAgentConfirm } from '../tableSlice';
import { saveAgentConfirm } from '../tableThunk';

const AgentConfirmCell = ({
  agentConfirm,
  owpMatchId
}: {
  agentConfirm: Option[];
  owpMatchId: number;
}) => {
  const agentConfirmOption = useAppSelector(
    store => store.events.agentConfirmOption
  );

  const hasWrite = useAppSelector(
    store => store.providerPolicy.permission.HasWrite
  );
  const dispatch = useAppDispatch();

  const handleOnChange = (event: Option[]) => {
    const agentSelected = event.map((a: Option) => a.value).join(',');
    dispatch(saveAgentConfirm({ owpMatchId, agentSelected }));
    dispatch(
      setAgentConfirm({
        owpMatchId,
        value: event.map((a: Option) => ({
          Value: a.value,
          Label: a.label
        }))
      })
    );
  };

  return (
    <td>
      <span className='agent__confirm'>
        <MultiSelect
          onChange={handleOnChange}
          value={agentConfirm}
          options={agentConfirmOption}
          ClearSelectedIcon={null}
          className='multi-select'
          hasSelectAll
          disabled={!hasWrite}
          labelledBy='Select...'
          overrideStrings={{
            selectAll: '-- Select All --',
            allItemsAreSelected: 'All'
          }}
        />
      </span>
    </td>
  );
};

export default AgentConfirmCell;
