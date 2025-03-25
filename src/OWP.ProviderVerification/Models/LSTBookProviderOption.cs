namespace OWP.ProviderVerification.Models;

public class LSTBookProviderOption
{
    public int LSTBookId { get; set; }

    public string LSTBookName { get; set; }
}

public class LSTBookProviderConfirmOption
{
    public int Value { get; set; }

    public string Label { get; set; }
}

public class LSTBookProviderModel
{
    public int OwpMatchId { get; set; }

    public int LSTBookId { get; set; }

    public string LSTBookName { get; set; }
}