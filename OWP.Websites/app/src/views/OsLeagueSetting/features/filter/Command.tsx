import { Button } from 'reactstrap';
import useStore from '../../app/store';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { shallow } from 'zustand/shallow';
import { getURL } from '../../helper/OsLeagueSettingHelper';

const Command = () => {
  const {
    queryDataSource,
    setPopupType,
    filter: { status, round, category, league },
    getPolicy
  } = useStore(
    state => ({
      queryDataSource: state.queryDataSource,
      setPopupType: state.setPopupType,
      filter: state.filter,
      getPolicy: state.getPolicy
    }),
    shallow
  );

  const handleSearch = async () => {
    if (!status || !round || !category) {
      return ToastrWrapper.showError(
        'Please select at least one option for each filter'
      );
    }
    return queryDataSource();
  };

  const handleExport = () => {
    if (!status || !round || !category) {
      return ToastrWrapper.showError(
        'Please select at least one option for each filter'
      );
    }
    const url = getURL('export-league-setting');
    window.location.href = `${url}?CategoryId=${category}&League=${league}&RoundId=${round}&StatusId=${status}`;
    return url;
  };

  const policy = getPolicy();

  return (
    <div className='command'>
      <Button className='btn-search' onClick={handleSearch}>
        Search
      </Button>
      {policy.HasWrite && (
        <Button onClick={() => setPopupType('add')} className='btn-add-league'>
          Add New Setting
        </Button>
      )}
      <Button className='btn-export' onClick={handleExport}>
        Export
      </Button>
    </div>
  );
};

export default Command;
