namespace Vivelin.AspNetCore.Headers
{
    public static class ReferrerPolicyExtensions
    {
        public static ResponseHeadersOptionsBuilder AddReferrerPolicy(this ResponseHeadersOptionsBuilder builder, ReferrerPolicy referrerPolicy)
        {
            return builder.Add("Referrer-Policy", referrerPolicy.ToString());
        }
    }
}
