import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { updateObjectAddPopup } from '../slice/addSettingSlice';
import Select from 'react-select-ts';
import { NoOfferLive as NoOfferLiveOptions } from '../../../../constants/MaxbetConst';
import { TOption } from '../../component/Type';

const NoOfferLive = () => {
  const { noOfferLive } = useAppSelector(state => state.addSettingSlice);
  const dispatch = useAppDispatch();

  const handleOnChange = (selectedOption: TOption | null) => {
    const value = selectedOption?.value || 0;
    dispatch(updateObjectAddPopup({ property: 'noOfferLive', value }));
  };

  const selectedOption = NoOfferLiveOptions.find(option => option.value === noOfferLive);

  return (
    <div className='list'>
      <div className='title'>No Offer Live</div>
      <div className='content-column-2'>
        <div className='input-item no-offer-live'>
          <Select
            options={NoOfferLiveOptions}
            value={selectedOption}
            className='basic-multi-select'
            classNamePrefix='select'
            name='no-offer-live'
            onChange={handleOnChange}
          />
          {noOfferLive === 3 && (
            <div className='hint-message' style={{
              marginTop: '8px',
              padding: '8px 12px',
              backgroundColor: '#f0f8ff',
              border: '1px solid #b3d9ff',
              borderRadius: '4px',
              fontSize: '12px',
              color: '#0066cc',
              fontStyle: 'italic'
            }}>
              Hint: Peak hours range from 8:00 PM to 2:59 AM (GMT+8).
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default NoOfferLive;
