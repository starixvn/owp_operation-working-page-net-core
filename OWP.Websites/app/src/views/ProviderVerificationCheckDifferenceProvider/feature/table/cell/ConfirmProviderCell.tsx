import { Option } from '../../../../Shared/MultipleSelection/type';
import { MultiSelect } from 'react-multi-select-component';
import { useAppDispatch, useAppSelector } from '../../../app/hooks';
import { setConfirmProvider } from '../tableSlice';
import { saveConfirmProvider } from '../tableThunk';

const ConfirmProviderCell = ({
  confirmProvider,
  owpMatchId,
  canEditDate
}: {
  confirmProvider: Option[];
  owpMatchId: number;
  canEditDate: boolean;
}) => {
  const { confirmProviderOption, hasWrite } = useAppSelector(store => ({
    confirmProviderOption: store.events.confirmProviderOption,
    hasWrite: store.providerPolicy.permission.HasWrite,
  }));

  const dispatch = useAppDispatch();

  const handleOnChange = (event: Option[]) => {
    const agentSelected = event.map((a: Option) => a.value).join(',');
    dispatch(saveConfirmProvider({ owpMatchId, agentSelected }));
    dispatch(
      setConfirmProvider({
        owpMatchId,
        value: event.map((a: Option) => ({
          value: a.value,
          label: a.label
        }))
      })
    );
  };

  return (
    <td>
      <span className='confirm-provider'>
        <MultiSelect
          onChange={canEditDate ? handleOnChange : () => { }}
          value={confirmProvider}
          options={confirmProviderOption}
          ClearSelectedIcon={null}
          className={`multi-select ${!canEditDate ? 'checkbox-disable' : ''}`}
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

export default ConfirmProviderCell;
