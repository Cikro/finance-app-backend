namespace finance_app.Types.Services.V1.Authorization {

    /// <summary>
    /// Authorization Policies for authorizing things....
    /// Right now, If you have access to the resource, you can do anything with it!
    /// In the future, maybe there could be a "View Only", "Update", "Create", "Close" etc. policies?
    /// </summary>
    public static class AuthorizationPolicies {
        public static string CanAccessResource = "CanAccessResourcePolicy";
    }
}