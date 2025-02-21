import { Option } from '../../../../Shared/MultipleSelection/type';
import { MultiSelect } from 'react-multi-select-component';
import { useAppDispatch, useAppSelector } from '../../../app/hooks';
import { setConfirmProvider } from '../tableSlice';
import { saveConfirmProvider } from '../tableThunk';

const ConfirmProviderCell = ({
  confirmProvider,
  owpMatchId
}: {
  confirmProvider: Option[];
  owpMatchId: number;
}) => {
  const confirmProviderOption = useAppSelector(
    store => store.events.confirmProviderOption
  );

  const hasWrite = useAppSelector(
    store => store.providerPolicy.permission.HasWrite
  );
  const dispatch = useAppDispatch();

  const handleOnChange = (event: Option[]) => {
    const confirmProviderSelected = event.map((a: Option) => a.value).join(',');
    dispatch(saveConfirmProvider({ owpMatchId, confirmProviderSelected }));
    dispatch(
      setConfirmProvider({
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
      <span className='confirm-provider'>
        <MultiSelect
          onChange={handleOnChange}
          value={confirmProvider}
          options={confirmProviderOption}
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

export default ConfirmProviderCell;
