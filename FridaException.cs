namespace PInvoke.FridaCore;

public class FridaException:Exception
{
    private int Code { get; set; }

    public FridaException(GError error):base(error.Message)
    {
        Code = error.Code;
    }
}