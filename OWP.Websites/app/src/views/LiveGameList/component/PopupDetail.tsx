import { useRef } from 'react';
import { Modal, ModalHeader, ModalBody } from 'reactstrap';
import Toastr from '../../components/ToastrWrapper';
import Utils from '../../Shared/Utils';
import { useSelector } from 'react-redux';
import { RootState } from '../app/storeNative';
import useStableDispatch from './../../Shared/customHook/useStableDispatch';
import { closePopupDetail } from '../table/tableSlice/tableSlice';
import { getEvents } from '../table/tableSlice/tableThunk';

const PopupDetail = () => {
  const iframeRef = useRef<HTMLIFrameElement>(null);
  const dispatch = useStableDispatch();
  const { popupState, refreshModel } = useSelector(
    (state: RootState) => state.events
  );
  const { isOpen, matchId } = popupState;

  const onHideDetailInformation = () => {
    if (iframeRef.current) {
      const editGeneralInfo = iframeRef.current.getAttribute(
        'edit-general-info'
      );
      const editMatchInfo = iframeRef.current.getAttribute('edit-match-info');
      const editTradingInfo = iframeRef.current.getAttribute(
        'edit-trading-info'
      );
      const editProviderInfo = iframeRef.current.getAttribute(
        'edit-provider-info'
      );

      const isEditGeneralAndMatchInfo =
        editGeneralInfo === '1' || editMatchInfo === '1';
      const isEditTradingAndProviderInfo =
        editTradingInfo === '1' || editProviderInfo === '1';
      if (isEditGeneralAndMatchInfo || isEditTradingAndProviderInfo) {
        Toastr.confirm('Your unsaved changes will be lost. Are you sure?').then(
          (confirmResult: { value: boolean }) => {
            confirmResult.value && onCloseModal();
          }
        );
      } else {
        onCloseModal();
      }
    }
  };

  const onCloseModal = () => {
    const item = iframeRef.current && iframeRef.current.getAttribute('updated');
    const updateResult = item && parseInt(item, 10);
    dispatch(closePopupDetail());
    if (updateResult && updateResult > 0) {
      iframeRef.current && iframeRef.current.setAttribute('updated', '0');
      dispatch(getEvents(refreshModel));
    }
  };

  return (
    <>
      {isOpen && (
        <Modal
          tag='div'
          size='lg'
          isOpen
          className='modal-dark-title modal-edit-event'>
          <ModalHeader toggle={onHideDetailInformation}>
            Detail Information
          </ModalHeader>
          <ModalBody tag='div'>
            <div className='masonry'>
              <iframe
                ref={iframeRef}
                style={{
                  overflow: 'hidden',
                  height: '100%',
                  width: '100%',
                  minHeight: '700px',
                  border: '0'
                }}
                src={
                  Utils.resolveUrl('/EventManagement/LiveDetail/Index/') +
                  `${matchId}`
                }
              />
            </div>
          </ModalBody>
        </Modal>
      )}
    </>
  );
};

export default PopupDetail;
