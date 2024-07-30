import { DragEvent } from 'react';
import DragIcon from '../../../../assets/svg-element/DragIcon';
import { ProviderData } from '../TypeNative';

type Props = {
  isEdit: boolean;
  data: ProviderData[];
  setData: Function;
};

const SortListNative = (props: Props) => {
  const { isEdit, data, setData } = props;

  const placeholder = document.createElement('li');
  placeholder.className = 'placeholder';
  let nodePlacement = '';
  let dragged: HTMLElement, dragOverItem: HTMLElement;

  const dragStart = (e: DragEvent<HTMLElement>) => {
    dragged = (e.currentTarget as HTMLElement).closest('li') ?? e.currentTarget;
    e.dataTransfer.effectAllowed = 'move';
    e.dataTransfer.setData('text/html', (e.target as HTMLElement).id);
  };

  const onDragOver = (e: DragEvent<HTMLElement>) => {
    e.preventDefault();
    dragged.style.display = 'none';
    const ulParent = document.getElementById('provider-list') ?? e.target as HTMLElement;
    const closetLi = (e.target as HTMLElement).closest('li') ?? e.target as HTMLElement;
    if (closetLi.className === 'placeholder') {
      return;
    }

    dragOverItem = closetLi;
    const relY = e.clientY - dragOverItem.offsetTop - (ulParent.offsetParent as HTMLElement).offsetTop;
    const HALF_HEIGHT = 2;
    const height = dragOverItem.offsetHeight / HALF_HEIGHT;
    if (relY > height) {
      if (height > 0) {
        nodePlacement = 'after';
      }
      ulParent.insertBefore(placeholder, closetLi.nextElementSibling);
    } else if (relY < height) {
      nodePlacement = 'before';
      ulParent.insertBefore(placeholder, closetLi);
    }
  };

  const onDragEnd = () => {
    dragged.style.display = 'block';
    const parentNode = dragged.parentNode;
    parentNode && parentNode.removeChild(placeholder);
    const from = Number(dragged.dataset.id);
    let to = Number(dragOverItem.dataset.id);
    if (from < to) {
      to--;
    }
    if (nodePlacement === 'after') {
      to++;
    }
    const dataCopy = [...data];
    dataCopy.splice(to, 0, dataCopy.splice(from, 1)[0]);
    setData(dataCopy);
  };

  return (
    <div className='provider-table'>
      <ul onDragOver={e => isEdit && onDragOver(e)} id='provider-list' draggable={isEdit}>
        {data && data.map((item, index) => (
          <li
            onDragStart={e => isEdit && dragStart(e)}
            onDragEnd={_ => isEdit && onDragEnd()}
            key={index}
            data-id={index}
            draggable={isEdit}>
            <div className='empty-div' />
            <div className='provider' draggable={isEdit}>
              {isEdit && <span className='drag-icon'><DragIcon /></span>}
              <span className='no'>{index + 1}</span>
              <span className='item'>{item.value}</span>
            </div>
            <div className='empty-div' />
          </li>
        ))}
      </ul>
    </div>
  );
};

export default SortListNative;
