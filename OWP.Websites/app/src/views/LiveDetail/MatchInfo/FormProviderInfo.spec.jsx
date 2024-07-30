import { cloneDeep } from 'lodash';
import Converter from '../../Shared/Converter';
import FormProviderInfo from './FormProviderInfo';
import { act, fireEvent, render, screen } from '@testing-library/react';
import Toastr from '../../components/ToastrWrapper';
import service from '../MatchInfo/Service';

jest.mock('../../components/Spinner');
jest.mock('lodash');
jest.mock('../../Shared/Converter');
jest.mock('./Service');
jest.mock('../../components/ToastrWrapper');
jest.mock('../MatchInfo/Service');

const onSave = jest.fn();
const onEdit = jest.fn();
const onCancel = jest.fn();

const allTelevisions = [
  {
    TVId: 1,
    TVCode: 'TV 1',
    Color: '#111'
  },
  {
    TVId: 2,
    TVCode: 'TV 2',
    Color: '#222'
  },
  {
    TVId: 3,
    TVCode: 'TV 3',
    Color: '#333'
  }
];

const allOtherProviders = [
  {
    OtherProviderId: 1,
    OtherProviderName: 'Provider 1'
  },
  {
    OtherProviderId: 2,
    OtherProviderName: 'Provider 2'
  },
  {
    OtherProviderId: 3,
    OtherProviderName: 'Provider 3'
  }
];

const allProviders = [
  {
    ProviderId: 1,
    ProviderName: 'Provider 1'
  },
  {
    ProviderId: 2,
    ProviderName: 'Provider 2'
  },
  {
    ProviderId: 3,
    ProviderName: 'Provider 3'
  }
];

const allAgents = [
  {
    AgentId: 1,
    AgentName: 'Agent 1'
  },
  {
    AgentId: 2,
    AgentName: 'Agent 2'
  }
];

const defaultModel = {
  isEditing: false,
  Channel: 'BBC',
  WB: 'XXX',
  Phone: '11111111',
  TVs: allTelevisions,
  Providers: allOtherProviders,
  OtherProviders: allOtherProviders,
  Agents: allAgents
};

const props = {
  televisions: allTelevisions,
  providers: allProviders,
  otherProviders: allOtherProviders,
  agents: allAgents,
  policy: { HasWrite: true },
  data: defaultModel,
  onSave,
  onEdit,
  onCancel
};

Converter.convertTVsToOptions.mockReturnValue([
  { value: 1, label: 'TV1' },
  { value: 2, label: 'TV2' },
  { value: 3, label: 'TV3' }
]);
Converter.convertProvidersToOptions.mockReturnValue([
  { value: 1, label: 'Provider 1' },
  { value: 2, label: 'Provider 2' },
  { value: 3, label: 'Provider 3' }
]);
Converter.convertOtherProvidersToOptions.mockReturnValue([
  { value: 1, label: 'Provider 1' },
  { value: 2, label: 'Provider 2' },
  { value: 3, label: 'Provider 3' }
]);
Converter.convertAgentsToOptions.mockReturnValue([
  { value: 1, label: 'Agent 1' },
  { value: 2, label: 'Agent 2' }
]);

describe('<FormProviderInfo/>', () => {
  it('Should match snapshot when init', async () => {
    const { container } = render(<FormProviderInfo {...props} />);
    await act(async () => {
      await Promise.resolve()
    })
    expect(container.firstChild).toMatchSnapshot();
  });

  it('should update state when change Channel', async () => {
    const { container } = render(<FormProviderInfo {...props} />);
    await act(async () => {
      await Promise.resolve()
    })

    fireEvent.click(container.querySelector('.icon-edit'));

    const channel = document.getElementById('ChannelTooltip');

    fireEvent.change(channel, {
      target: {
        value: 'channel-test'
      }
    })
    expect(channel.value).toEqual('channel-test');

  });

  it('should update state when change WB', async () => {
    const { container } = render(<FormProviderInfo {...props} />);
    await act(async () => {
      await Promise.resolve()
    })

    fireEvent.click(container.querySelector('.icon-edit'));

    const wb = document.getElementById('WB');

    fireEvent.change(wb, {
      target: {
        value: 'WB-test'
      }
    })
    expect(wb.value).toEqual('WB-test');
  });

  it('should update state when change Phone', async () => {
    Toastr.confirm.mockResolvedValue({ value: 1 });
    service.updateProviderInfo.mockReturnValue({ IsSuccess: true })

    const { container } = render(<FormProviderInfo {...props} />);

    fireEvent.click(container.querySelector('.icon-edit'));

    const phone = document.getElementById('Phone');

    fireEvent.change(phone, {
      target: {
        value: 'Phone-test'
      }
    })
    fireEvent.click(container.querySelector('.icon-save'));
    await act(async () => {
      await Promise.resolve()
    })

    expect(phone.value).toEqual('Phone-test');
  });

  it('should call onSave when click save button', async () => {
    Toastr.confirm.mockResolvedValue({ value: 1 });
    jest.clearAllMocks();
    service.updateProviderInfo.mockResolvedValue({ IsSuccess: true })

    const { container } = render(<FormProviderInfo {...props} />);

    fireEvent.click(container.querySelector('.icon-edit'));

    fireEvent.click(container.querySelector('.icon-save'));

    await act(async () => {
      await Promise.resolve()
    })

    expect(onSave).toBeCalledTimes(1);
  });

  it('should not call onSave when click save button', async () => {
    Toastr.confirm.mockResolvedValue({ value: 1 });
    jest.clearAllMocks();
    service.updateProviderInfo.mockResolvedValue({ IsSuccess: false })

    const { container } = render(<FormProviderInfo {...props} />);

    fireEvent.click(container.querySelector('.icon-edit'));

    fireEvent.click(container.querySelector('.icon-save'));

    await act(async () => {
      await Promise.resolve()
    })

    expect(onSave).not.toBeCalledTimes(1);
  });

  it('should allow to cancel editing & reset model', async () => {
    const { container } = render(<FormProviderInfo {...props} />);
    await act(async () => {
      await Promise.resolve()
    })

    fireEvent.click(container.querySelector('.icon-edit'));

    const phone = document.getElementById('Phone');
    const channel = document.getElementById('ChannelTooltip');
    const wb = document.getElementById('WB');

    fireEvent.change(channel, {
      target: {
        value: 'channel-test'
      }
    })
    fireEvent.change(phone, {
      target: {
        value: 'Phone-test'
      }
    })

    fireEvent.change(wb, {
      target: {
        value: 'WB-test'
      }
    })

    fireEvent.click(container.querySelector('.icon-cancel'));

    expect(onCancel).toBeCalledTimes(1);
  });
});
