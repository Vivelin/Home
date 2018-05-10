namespace Vivelin.AspNetCore.Headers
{
    public static class ReferrerPolicyExtensions
    {
        public static ResponseHeadersOptionsBuilder AddReferrerPolicy(this ResponseHeadersOptionsBuilder builder, params string[] values)
        {
            return builder.Add("Referrer-Policy", values);
        }
    }
}
