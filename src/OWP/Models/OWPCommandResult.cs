namespace OWP.Model
{
    public class OWPCommandResult
    {
        private int code;
        private string message;

        public OWPCommandResult()
        {
            Code = 1;
        }

        public int Code
        {
            get { return this.code; }
            set { this.code = value; }
        }

        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }

        public virtual bool IsSuccessful
        {
            get { return true; }
        }
    }
}