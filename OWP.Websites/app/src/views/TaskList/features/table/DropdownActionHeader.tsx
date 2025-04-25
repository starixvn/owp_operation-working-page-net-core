import ToastrWrapper from '../../../components/ToastrWrapper';
import { RootState } from '../../app/storeNative';
import { useAppSelector } from '../../app/hooks';

type TDropdownActionHeader = {
  refValue: React.RefObject<HTMLDivElement>;
  closePopup: () => void;
};

const DropdownActionHeader = ({
  refValue,
  closePopup
}: TDropdownActionHeader) => {
  const matchesSelected = useAppSelector(
    (state: RootState) => state.taskList.matchesSelected
  );

  const handleOpenLink = (key: string) => {
    closePopup();
    if (matchesSelected.length === 0) {
      ToastrWrapper.showInfo(<span>No match has been selected!</span>);
      return;
    }
  };

  return (
    <div className='dropdown-action is-show' ref={refValue}>
      <li
        className='add-special-product'
        onClick={() => handleOpenLink('Saba Batch')}>
        Saba Batch
      </li>
      <li
        className='remove-special-product'
        onClick={() => handleOpenLink('RT3 Batch')}>
        RT3 Batch
      </li>
    </div>
  );
};

export default DropdownActionHeader;
