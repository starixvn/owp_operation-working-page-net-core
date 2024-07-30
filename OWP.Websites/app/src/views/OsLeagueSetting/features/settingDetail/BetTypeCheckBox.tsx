import { useState } from 'react';
import useQueryDropdownOption from '../../hook/useQueryDropdownOption';
import { simpleApiKey, localStoreKey, betTypeGroup } from '../../osLeague.json';
import { IBettypeType } from '../../typing/bettypeType';
import useStore from '../../app/store';
import ErrorMessage from './ErrorMessage';
import Loading from '../../../components/Loading';

const { betType } = simpleApiKey;
const { betTypeOption } = localStoreKey;

const BetTypeCheckBox = (props: IBettypeType) => {
  const [selectedBetTypes, setSelectedBetTypes] = useState<
    Array<string | number>
  >(props.value);
  const { options, loading } = useQueryDropdownOption({
    apiKey: betType,
    localStorageKey: betTypeOption
  });

  const errorMessage = useStore(state => state.errorMessage.betTypes);

  if (loading) {
    return <Loading />;
  }
  const handleOnChange = (value: string) => {
    const checked = selectedBetTypes?.includes(value);

    const newValue = checked
      ? selectedBetTypes?.filter(x => x !== value)
      : [...selectedBetTypes, value];

    setSelectedBetTypes(newValue);
    props.onChange(newValue);
  };

  const renderBody = () => {
    return options.map((option, index) => {
      return (
        <div key={index}>
          <div
            className={`checkbox-field ${index / betTypeGroup &&
              'border-left-1'}`}>
            <input
              type='checkbox'
              id={`bet-type-checkbox-${option.value}`}
              data-testid={`checkbox-${index}`}
              name={option.label}
              value={option.value}
              disabled={props.disabled}
              onChange={() => handleOnChange(option.value.toString())}
              checked={selectedBetTypes.includes(option.value.toString())}
            />
            <label htmlFor={`bet-type-checkbox-${option.value}`}>
              {option.label}
            </label>
          </div>
        </div>
      );
    });
  };

  return (
    <div className='bet-type-content'>
      <label>
        Bet Type(s) <span className='required-field'>*</span>:{' '}
        <span className='number-bettype'>{selectedBetTypes.length}</span>
      </label>
      <div className='checkboxs-content'>{renderBody()}</div>
      <ErrorMessage message={errorMessage}></ErrorMessage>
    </div>
  );
};

export default BetTypeCheckBox;
