namespace _6tactics.Cms.Services.Common
{
    public interface ISimpleCaptchaService
    {
        string GenerateCaptcha();
        bool Check(int result, int a, int b, string operation);
    }
}