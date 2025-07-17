

type TQuickMessage = {
  id: number;
  shortcut: string;
  message: string;
};

type TServer = {
  id: number;
  name: string;
  url: string;
  tradingPort: number;
  laport: number;
};

export { TQuickMessage, TServer };