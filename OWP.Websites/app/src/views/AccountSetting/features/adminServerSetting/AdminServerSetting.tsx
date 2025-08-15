import { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { queryAdminServerSettingThunk, querySelectedAdminServerThunk, saveAdminServerSettingThunk } from './slice/AdminServerSettingThunk';
import Select from 'react-select';
import { UncontrolledTooltip } from 'reactstrap';
import InformationOutlineIcon from 'mdi-react/InformationOutlineIcon';
import { setSelectedServer } from './slice/adminServerSettingSlice';

interface ServerOption {
  value: number;
  label: string;
  url: string;
}

const AdminServerSetting = () => {
  const servers = useAppSelector(state => state.adminServerSettingSlice.servers);
  const selectedServer = useAppSelector(state => state.adminServerSettingSlice.selectedServer);
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(queryAdminServerSettingThunk());
    dispatch(querySelectedAdminServerThunk());
  }, []);

  const options: ServerOption[] = servers.map(server => ({
    value: server.id,
    label: server.name,
    url: server.url
  }));

  const selectedServerOption = options.find(option => option.value === selectedServer) || null;

  const handleOnChange = (option: ServerOption | null) => {
    if (option) {
      dispatch(setSelectedServer(option.value));
      dispatch(saveAdminServerSettingThunk({ value: option.value }));
    }
  };

  const Option = (props: { data: ServerOption; children: React.ReactNode }) => (
    <div title={props.data.url}>
      {props.children}
    </div>
  );

  return (
    <div className="admin-server-setting">
      <div className="head">
        <h4 className="title" style={{ display: 'inline-flex', alignItems: 'center' }}>
          My AdminSite Server
          <span id="adminServerInfoIcon" style={{ marginLeft: 6, cursor: 'pointer' }}>
            <InformationOutlineIcon />
          </span>
          <UncontrolledTooltip placement="right" target="adminServerInfoIcon">
            This server will be used as the default for all AdminSite-related links, including Trading Link, Saba Batch, and RT3 Batch.
          </UncontrolledTooltip>
        </h4>
      </div>
      <div className="server-setting-content">
        <label>Server</label>
        <Select
          className="server-select"
          value={selectedServerOption}
          onChange={handleOnChange}
          options={options}
          components={{ Option }}
        />
      </div>
    </div>
  );
};

export default AdminServerSetting;