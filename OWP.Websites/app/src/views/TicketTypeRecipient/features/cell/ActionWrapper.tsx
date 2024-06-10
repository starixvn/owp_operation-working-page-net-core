import { useState } from 'react';
import Dialog from '../dialog/Dialog';
import { TicketTypeRecipientDataSource } from '../TypeNative';
import ActionCell from './ActionCell';
import useStore from '../../app/zustand/ticketTypeStore';
import ToastrWrapper from '../../../components/ToastrWrapper';
import Spinner from '../../../components/Spinner';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { getTicketTypeRecipients } from '../ticketTypeThunkNative';
const ActionWrapper = (data: TicketTypeRecipientDataSource) => {
  const [isOpenPopup, setIsOpenPopup] = useState<boolean>(false);
  const postData = useStore(state => state.saveData);
  const dispatch = useStableDispatch();

  const handleSave = async () => {
    Spinner.show();
    const result = await postData(data.ticketType.typeId);
    if (result) {
      ToastrWrapper.showInfo('Recipients have been updated successfully!');
      dispatch(getTicketTypeRecipients());
    }

    Spinner.hide();
  };

  const props = {
    ...data,
    onClose: () => setIsOpenPopup(false),
    onSave: handleSave
  };

  return (
    <>
      <ActionCell onOpenPopup={() => setIsOpenPopup(true)} />
      {isOpenPopup && <Dialog {...props} />}
    </>
  );
};

export default ActionWrapper;
