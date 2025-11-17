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
      <div className='content-column'>
        <div className='input-item'>
          <Select
            options={NoOfferLiveOptions}
            value={selectedOption}
            className='basic-multi-select'
            classNamePrefix='select'
            name='no-offer-live'
            onChange={handleOnChange}
          />
        </div>
      </div>
    </div>
  );
};

export default NoOfferLive;
