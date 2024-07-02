import MaxbetSpreadCell from './MaxbetSpreadCell';
import { fireEvent, render, screen } from '@testing-library/react';

describe('<MaxbetSpreadCell/>', () => {
  it('should render <MaxbetSpreadCell/> component success', () => {
    const props = {
      Maxbet: 1,
      Spread: 2,
      isEditing: true,
      labelMaxbet: 'maxbet',
      labelSpread: 'spread',
      highlightInput: false,
      handleMaxbetChange: jest.fn(),
      handleSpreadChange: jest.fn()
    };
    const { container } = render(
      <table>
        <tbody>
          <tr>
            <MaxbetSpreadCell {...props} />
          </tr>
        </tbody>
      </table>
    );

    expect(container.firstChild).toMatchSnapshot();
  });

  it('should disable at mode view', () => {
    const props = {
      labelMaxbet: 'maxbet',
      labelSpread: 'spread',
      highlightInput: false,
      isEditing: false,
      Maxbet: 1,
      Spread: 0.3
    };
    const { container } = render(
      <table>
        <tbody>
          <tr>
            <MaxbetSpreadCell {...props} />
          </tr>
        </tbody>
      </table>
    );

    const input = container.querySelectorAll('.input-field');

    expect(input[0]).toHaveAttribute('disabled');
    expect(input[1]).toHaveAttribute('disabled');
  });

  it('should enable at mode edit', () => {
    const props = {
      labelMaxbet: 'maxbet',
      labelSpread: 'spread',
      highlightInput: false,
      isEditing: true,
      Maxbet: 1,
      Spread: 0.3
    };
    const { container } = render(
      <table>
        <tbody>
          <tr>
            <MaxbetSpreadCell {...props} />
          </tr>
        </tbody>
      </table>
    );

    const input = container.querySelectorAll('.input-field');

    expect(input[0]).not.toHaveAttribute('disabled');
    expect(input[1]).not.toHaveAttribute('disabled');
  });

  it('should highlight input when invalid', () => {
    const props = {
      labelMaxbet: 'maxbet',
      labelSpread: 'spread',
      highlightInput: true,
      isEditing: true
    };
    const { container } = render(
      <table>
        <tbody>
          <tr>
            <MaxbetSpreadCell {...props} />
          </tr>
        </tbody>
      </table>
    );

    expect(container.querySelectorAll('.input-invalid').length).toBe(2);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('should not highlight input when valid', () => {
    const props = {
      labelMaxbet: 'maxbet',
      labelSpread: 'spread',
      highlightInput: false,
      isEditing: true
    };
    const { container } = render(
      <table>
        <tbody>
          <tr>
            <MaxbetSpreadCell {...props} />
          </tr>
        </tbody>
      </table>
    );
    expect(container.querySelectorAll('.input-invalid').length).toBe(0);
  });

  it('handleMaxbetChange tobe call when maxbet change', () => {
    const handleMaxbetChange = jest.fn();
    const props = {
      Maxbet: 1,
      labelMaxbet: 'maxbet',
      labelSpread: 'spread',
      highlightInput: false,
      isEditing: true,
      handleMaxbetChange
    };
    const { container } = render(
      <table>
        <tbody>
          <tr>
            <MaxbetSpreadCell {...props} />
          </tr>
        </tbody>
      </table>
    );

    const input = container.querySelectorAll('.input-field');
    fireEvent.change(input[0], {
      target: {
        value: '12'
      }
    });

    expect(handleMaxbetChange).toBeCalled();
  });

  it('handleSpreadChange tobe call when spread change', () => {
    const handleSpreadChange = jest.fn();
    const props = {
      Maxbet: 1,
      Spread: 0.5,
      labelMaxbet: 'maxbet',
      labelSpread: 'spread',
      highlightInput: false,
      isEditing: true,
      handleSpreadChange
    };
    const { container } = render(
      <table>
        <tbody>
          <tr>
            <MaxbetSpreadCell {...props} />
          </tr>
        </tbody>
      </table>
    );

    const input = container.querySelectorAll('.input-field');
    fireEvent.change(input[1], {
      target: {
        value: '12'
      }
    });

    expect(handleSpreadChange).toBeCalled();
  });
});
