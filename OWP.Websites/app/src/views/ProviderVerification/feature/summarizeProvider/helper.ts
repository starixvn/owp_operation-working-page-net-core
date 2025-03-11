import { formatTime } from '../../../Shared/DateTime/dateTimeHelper';
import { Option } from '../../../Shared/MultipleSelection/type';
import {
  TAgentCompare,
  TCompareCountResult,
  TProviderCount
} from '../../type/provider';
import { TDataSource } from '../../type/table';

const summarize = (data: TDataSource[], agentOption: Option[]) => {
  const agents = data.map(x => x.agentConfirm.map(agent => agent.label)).flat();
  const agentLabels = agentOption.map(x => x.label);
  const agentCounts = agentLabels.map(agentName => {
    const count = agents.filter(agent => agent === agentName).length;
    return { agentName, count } as TProviderCount;
  });

  return agentCounts;
};

const compareDifferent = (agentRawData: string, dataSource: TDataSource[]) => {
  const agentData = mapRows(agentRawData);
  const agentCounts = groupAndCountAgent(agentData);
  const dataSourceCounts = groupAndCountDataSource(dataSource);

  const differentKeys: TCompareCountResult[] = [];
  for (const key in agentCounts) {
    if (dataSourceCounts[key] !== agentCounts[key]) {
      differentKeys.push({
        koTime: key,
        dataCount: dataSourceCounts[key] ?? 0,
        agentFileCount: agentCounts[key] ?? 0
      });
    }
  }
  return differentKeys;
};

const groupAndCountDataSource = (dataSource: TDataSource[]) => {
  const dataSourceCounts: { [key: string]: number } = {};
  dataSource.forEach(row => {
    const formattedTime = formatTime(row.eventDateTime, 'minute', 'hh:mm A');
    if (formattedTime !== null) {
      dataSourceCounts[formattedTime] =
        (dataSourceCounts[formattedTime] || 0) + 1;
    }
  });

  return dataSourceCounts;
};

const groupAndCountAgent = (agentData: TAgentCompare[]) => {
  const agentCounts: { [key: string]: number } = {};
  agentData.forEach(row => {
    agentCounts[row.koTime] = (agentCounts[row.koTime] || 0) + 1;
  });

  return agentCounts;
};

const mapRows = (agentData: string) => {
  return agentData
    .split('\n')
    .filter(row => row.trim() !== '')
    .filter(row => row.split('\t')[0].trim() !== 'Time')
    .map(row => {
      const [koTime] = row.split('\t');
      return { koTime };
    });
};

export { summarize, compareDifferent };
