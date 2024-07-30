import { Option } from './typing/type';

const joinKey = (selected: Option[]) =>
  selected.reduce((previous: string | null, current, index) => {
    return !index ? current.value.toString() : `${previous},${current.value}`;
  }, null);

export { joinKey };
