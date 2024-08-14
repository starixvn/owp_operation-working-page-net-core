type TNode={
  id: number|string
  label: string,
  childNode?: TNode[]
}
type TTreeDataSource= {
  dataSource: Array<TNode>
}

export {
  TNode,
  TTreeDataSource
};