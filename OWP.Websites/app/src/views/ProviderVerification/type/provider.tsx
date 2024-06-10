type TProviderCount = {
  agentName: string;
  count: number;
}

type TAgentCompare = {
  koTime: string;
}

type TCompareCountResult = {
  koTime: string;
  dataCount: number;
  agentFileCount: number;
}

export { TProviderCount, TAgentCompare, TCompareCountResult };