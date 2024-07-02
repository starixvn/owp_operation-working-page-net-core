import Converter from '../Shared/Converter';
import FormProviderInfo from './FormProviderInfo';
import { render, screen, act, fireEvent } from '@testing-library/react';

jest.mock('../Shared/Converter');

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

const props = {
  televisions: allTelevisions,
  providers: allProviders,
  otherProviders: allOtherProviders,
  agents: allAgents
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

const defaultModel = {
  Channel: 'BBC',
  WB: 'XXX',
  Phone: '11111111'
};

describe('<FormProviderInfo/>', () => {
  let defaultProps;
  beforeAll(() => {
    defaultProps = {
      ...props,
      ...defaultModel
    };
  })
  it('should render <FormProviderInfo/>', async () => {
    const { container } = render(<FormProviderInfo {...props} />);
    await act(async () => {
      await Promise.resolve();
    });
    expect(container.firstChild).toMatchSnapshot();
  });


  it('should update state when change Channel', async () => {
    const { container } = render(<FormProviderInfo {...props} />);
    await act(async () => {
      await Promise.resolve();
    });

    fireEvent.change(container.querySelector('.input-channel'), {
      target: {
        value: 'test-channel'
      }
    })

    expect(container.querySelector('.input-channel').value).toEqual('test-channel');
  });

  it('should update state when change WB', async () => {
    const { container } = render(<FormProviderInfo {...props} />);
    await act(async () => {
      await Promise.resolve();
    });

    fireEvent.change(container.querySelector('.input-wb'), {
      target: {
        value: 'test-wb'
      }
    })

    expect(container.querySelector('.input-wb').value).toEqual('test-wb');

  });

  it('should update state when change Phone', async () => {
    const { container } = render(<FormProviderInfo {...props} />);
    await act(async () => {
      await Promise.resolve();
    });

    fireEvent.change(container.querySelector('.input-phone'), {
      target: {
        value: 'test-phone'
      }
    })

    expect(container.querySelector('.input-phone').value).toEqual('test-phone');

  });
});
