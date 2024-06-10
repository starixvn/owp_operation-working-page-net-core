interface ParamFilter {
  [key: string]: string | null;
}

const checkIsShow = (rest: ParamFilter) => {
  for (const property in rest) {
    if (rest[property]) {
      return true;
    }
  }
  return false;
};

export { checkIsShow };
