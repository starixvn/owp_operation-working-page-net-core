type TNode={
  id: number|string
  label: string,
  childNode?: TNode[]
}
type TDataSource= {
  dataSource: Array<TNode>
}

export {
  TNode,
  TDataSource
};