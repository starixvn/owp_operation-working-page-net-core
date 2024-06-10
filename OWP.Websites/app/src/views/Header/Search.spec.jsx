import { fireEvent, render, screen, waitFor } from '@testing-library/react';
import Search from './Search';
import QuickSearchService from '../QuickSearch/Service';
import SearchResultItem from './SearchResultItem';

jest.mock('../QuickSearch/Service');
jest.mock('./SearchResultItem', () => jest.fn(() => 'FakeSearchResult'));
jest.useFakeTimers();
jest.fn().mockImplementation(() => Promise.resolve());

describe('Search Component', () => {
  beforeEach(() => {
    QuickSearchService.query.mockReset();
  });

  it('render <Search/> component match snapshot when prop isVisible=true', () => {
    const props = {
      isVisible: true
    };
    const searchComponent = render(<Search {...props} />);

    expect(searchComponent).toMatchSnapshot();
  });

  it('render <Search/> component match snapshot when prop isVisible=false', () => {
    const props = {
      isVisible: false
    };
    const searchComponent = render(<Search {...props} />);

    expect(searchComponent).toMatchSnapshot();
  });

  it('ulSearchSuggestion has loading <li> when state isLoading = true', async () => {
    QuickSearchService.query.mockImplementation(() =>
      Promise.resolve([{ OWPMatchId: 222 }])
    );
    const { container } = render(<Search />);
    const element = container.querySelector(`[id="txtSearchKeywords"]`);
    jest.useFakeTimers();
    fireEvent.input(element, { target: { value: 'new value' } });

    await screen.findByText(/Loading.../);
    expect(screen.getByText('Loading...')).toBeInTheDocument();
    jest.runAllTimers();
    const searchResult = container.querySelector('#ulSearchSuggestion');
    fireEvent.scroll(searchResult, {
      target: {
        scrollX: 1,
        scrollY: 1
      }
    });
    await waitFor(() => {
      expect(QuickSearchService.query).toBeCalledTimes(1);
    });
  });

  it('ulSearchSuggestion has loading <li> when state isLoading = false', () => {
    const { container } = render(<Search />);
    expect(container.querySelector('.no-results')).toBeNull();
    expect(SearchResultItem).toBeCalled();
  });

  it('check icon close search form onClick callback', () => {
    const mockHandleVisibleSearchArea = jest.fn();
    const props = {
      handleVisibleSearchArea: mockHandleVisibleSearchArea
    };
    const { container } = render(<Search {...props} />);
    const iconClose = container.querySelector('a.close');
    fireEvent.click(iconClose);

    expect(mockHandleVisibleSearchArea).toBeCalledTimes(1);
    expect(mockHandleVisibleSearchArea).toBeCalledWith(false);
  });
});
