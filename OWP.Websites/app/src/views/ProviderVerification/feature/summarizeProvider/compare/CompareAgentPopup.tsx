import React, { useState } from 'react';
import { Modal, ModalBody, ModalFooter } from 'reactstrap';
import Select from 'react-select-ts';
import { Option } from '../../../../Shared/MultipleSelection/type';
import { TCompareCountResult } from '../../../type/provider';
import { compareDifferent } from '../helper';
import { useAppSelector } from '../../../app/hookNative';

interface Props {
  setIsOpenPopup: React.Dispatch<React.SetStateAction<boolean>>;
}

const CompareAgentPopup: React.FC<Props> = ({
  setIsOpenPopup
}) => {
  const [agent, setAgent] = useState<Option | null>(null);
  const [results, setResults] = useState<TCompareCountResult[]>([]);
  const [textAreaValue, setTextAreaValue] = useState<string>('');

  const { confirmProviderOption, dataSource } = useAppSelector(state => state.events);

  const handleCheck = async () => {
    const dataSourceByAgent = dataSource.filter(x => x.ConfirmProvider.some(y => y.Label === agent?.label));
    setResults(compareDifferent(textAreaValue, dataSourceByAgent));
  };

  const renderResult = () => {
    return (
      <table className='provider-compare-table'>
        <thead>
          <tr>
            <td>Agent</td>
            <td>Agent verification</td>
            <td>Agent file</td>
          </tr>
        </thead>
        {
          results.map((result, i) => {
            return <tr key={i}>
              <td>{result.koTime}</td>
              <td>{result.dataCount}</td>
              <td>{result.agentFileCount}</td>
            </tr>;
          })
        }
      </table>
    );
  };

  return (
    <Modal isOpen scrollable className='compare-popup' centered>
      <ModalBody>
        <h3 className='title'>Check the difference with Agent File</h3>
        <span>Agent Type: </span>
        <Select
          defaultValue={confirmProviderOption[0]}
          className='basic-multi-select agent-select'
          classNamePrefix='select'
          options={confirmProviderOption}
          onChange={selectedOption => {
            if (selectedOption !== agent) {
              setAgent(selectedOption);
              setResults([]);
            }
          }}
        />
        <textarea
          placeholder='Copy all the KO time of an agent in excel file and paste in here.
Make sure the format time is XX:XX AM/PM GMT+8
Ex: 01:00 PM
     03:00 AM'
          name='postContent'
          rows={20}
          value={textAreaValue}
          onChange={e => {
            setTextAreaValue(e.target.value);
            setResults([]);
          }}
        />
        {results.length > 0 && renderResult()}
      </ModalBody>
      <ModalFooter>
        <div className='list-btn'>
          <button className='button-cancel' onClick={() => setIsOpenPopup(false)}>Cancel</button>
          <button className='button-export button-check' onClick={handleCheck}>Check</button>
        </div>
      </ModalFooter>
    </Modal>
  );
};

export default CompareAgentPopup;
