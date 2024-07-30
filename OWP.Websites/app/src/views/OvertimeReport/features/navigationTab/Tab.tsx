import { lazy, Suspense, useState } from 'react';
import Loading from '../../../components/Loading';
const MonthTabContent = lazy(() =>
  import('../ByMonthTab/MonthTabContent')
);

const YearTabContent = lazy(() =>
  import('../YearTabContent/YearTabContent')
);

const Tab = () => {
  const [toggleState, setToggleState] = useState(1);
  const navList = [
    {
      tabName: 'By Month',
      index: 1,
      component: <MonthTabContent />
    },
    {
      tabName: 'By Year',
      index: 2,
      component: <YearTabContent />
    }
  ];

  const toggleTab = (index: number) => {
    setToggleState(index);
  };

  const getActiveClass = (index: number, className: string) =>
    toggleState === index ? className : '';

  const renderTabList = () =>
    navList.map(x => (
      <li
        key={x.index}
        className={`tabs ${getActiveClass(x.index, 'active-tabs')}`}
        onClick={() => toggleTab(x.index)}>
        {x.tabName}
      </li>
    ));

  const renderContentList = () =>
    navList.map(x => (
      <div
        key={x.index}
        className={`content ${getActiveClass(x.index, 'active-content')}`}>
        {x.component}
      </div>
    ));

  return (
    <div className='tab-header'>
      <ul className='tab-list'>{renderTabList()}</ul>
      <div className='content-container'>
        <Suspense fallback={<Loading />}>{renderContentList()}</Suspense>
      </div>
    </div>
  );
};

export default Tab;
