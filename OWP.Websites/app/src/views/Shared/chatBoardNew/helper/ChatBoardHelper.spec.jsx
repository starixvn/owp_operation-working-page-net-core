import { buildMessageListClassName } from './ChatBoardHelper';

describe('test NotePopover', () => {
  it('windowHeight <= 350, height return 100', () => {
    const windowHeight = 350;
    const height = buildMessageListClassName(windowHeight, true);
    expect(height).toEqual('message-list-100');
  });

  it('350 <windowHeight <= 530, height return 150', () => {
    const windowHeight = 500;
    const height = buildMessageListClassName(windowHeight, true);
    expect(height).toEqual('message-list-150');
  });

  it('530 < windowHeight <= 650, height return 220', () => {
    const windowHeight = 600;
    const height = buildMessageListClassName(windowHeight, true);
    expect(height).toEqual('message-list-220');
  });

  it('650 < windowHeight <= 700, height return 350', () => {
    const windowHeight = 700;
    const height = buildMessageListClassName(windowHeight, true);
    expect(height).toEqual('message-list-320');
  });

  it('750 < windowHeight <= 970, height return 700', () => {
    const windowHeight = 900;
    const height = buildMessageListClassName(windowHeight, false);
    expect(height).toEqual('message-list-700');
  });
  it('750 < windowHeight <= 970, height return 550', () => {
    const windowHeight = 900;
    const height = buildMessageListClassName(windowHeight, true);
    expect(height).toEqual('message-list-550');
  });

  it('default, height return 900', () => {
    const windowHeight = 1000;
    const height = buildMessageListClassName(windowHeight, false);
    expect(height).toEqual('message-list-900');
  });
  it('default, height return 750', () => {
    const windowHeight = 1000;
    const height = buildMessageListClassName(windowHeight, true);
    expect(height).toEqual('message-list-750');
  });
});
