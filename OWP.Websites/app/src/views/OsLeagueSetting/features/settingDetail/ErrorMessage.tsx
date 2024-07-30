interface Props {
  message: string | null;
}

function ErrorMessage(props: Props) {
  return <span className='require-field'>{props.message}</span>;
}

export default ErrorMessage;
