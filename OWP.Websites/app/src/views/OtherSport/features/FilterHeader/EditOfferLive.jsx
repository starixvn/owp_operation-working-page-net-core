import PropTypes from 'prop-types';

const propTypes = {
  editOfferLiveMatches: PropTypes.array,
  handleOnSave: PropTypes.func.isRequired,
  handleCancel: PropTypes.func.isRequired,
  handleOnOpenEditOfferLive: PropTypes.func.isRequired,
  isEdit: PropTypes.bool.isRequired
};
const EditOfferLive = props => {
  const {
    editOfferLiveMatches,
    handleOnSave,
    handleCancel,
    handleOnOpenEditOfferLive,
    isEdit
  } = props;
  return (
    <>
      {isEdit ? (
        <>
          <button className='button-cancel' onClick={() => handleCancel()}>
            Cancel
          </button>
          <button
            disabled={editOfferLiveMatches.length === 0}
            className={`button-save ${
              editOfferLiveMatches.length === 0 ? 'button-disable' : ''
            }`}
            onClick={() => handleOnSave()}>
            Save
          </button>
        </>
      ) : (
        <button
          className='button-edit-offer-live'
          onClick={() => handleOnOpenEditOfferLive()}>
          Edit
        </button>
      )}
    </>
  );
};
EditOfferLive.propTypes = propTypes;
export default EditOfferLive;
