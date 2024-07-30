import { Component } from 'react';
import Select from 'react-select';
import AuthorizeService from '../Authorize/Service';
import ResourceTable from './ResourceTable';

import { FunctionKeys } from '../Shared/Constants';
import Service from './Service';
const getResourceOptions = () => {
  return [
    { value: 'MatchOffer', label: 'Match Offer' },
    { value: 'NoOfHDP', label: 'No Of HDP' },
    { value: 'UserGroup', label: 'User Group' },
    { value: 'Section', label: 'Section' }
  ];
};

export default class ResourceManagement extends Component {
  state = {
    policy: {},
    selectedRefName: '',
    dataSource: []
  };

  componentDidMount() {
    const defaultRefName = getResourceOptions()[0].value;
    getResourceOptions().forEach(e => {
      sessionStorage.removeItem(`all-ref-${e.value}`);
    });

    (async () => {
      const policy = await AuthorizeService.authorize(
        FunctionKeys.UserManagement
      );
      const dataSource = await Service.getByRefName(defaultRefName);
      this.setState({
        policy,
        dataSource,
        selectedRefName: defaultRefName
      });
    })();
  }

  async onSelectedChangeHandle(evt) {
    const selectedRefName = evt.value;
    const dataSource = await Service.getByRefName(selectedRefName);
    this.setState({ dataSource, selectedRefName });
  }

  render() {
    const { policy, selectedRefName, dataSource } = this.state;
    return (
      <div className='resource-management animated fadeIn'>
        <div className=''>
          <span>Field </span>
          <Select
            name='resourceOptions'
            options={getResourceOptions()}
            className='basic-multi-select'
            classNamePrefix='select'
            defaultValue={getResourceOptions()[0]}
            onChange={evt => this.onSelectedChangeHandle(evt)}
          />
        </div>

        {dataSource && (
          <ResourceTable
            policy={policy}
            refName={selectedRefName}
            dataSource={dataSource}
          />
        )}
      </div>
    );
  }
}
