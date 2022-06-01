namespace WorkerManagementAPI.ExceptionsTemplate
{
    public class ExceptionCodeTemplate
    {
        public const string BCKND_USER_NOTFOUND = "User not found";
        public const string BCKND_USER_CONFLICT = "User already exist";
        public const string BCKND_USER_LIST_NOTFOUND = "List of Users is empty";

        public const string BCKND_COMPANY_NOTFOUND = "Company not found";
        public const string BCKND_COMPANY_CONFLICT = "Company already exist";
        public const string BCKND_COMPANY_LIST_NOTFOUND = "List of Companies is empty";

        public const string BCKND_TECHNOLOGY_NOTFOUND = "Technology not found";
        public const string BCKND_TECHNOLOGY_CONFLICT = "Technology already exist";
        public const string BCKND_TECHNOLOGY_LIST_NOTFOUND = "List of Technologies is empty";

        public const string BCKND_PROJECT_NOTFOUND = "Project not found";
        public const string BCKND_PROJECT_LIST_NOTFOUND = "List of Projects is empty";

        public const string BCKND_ROLE_NOTFOUND = "Role not found";
        public const string BCKND_ROLE_LIST_NOTFOUND = "List of Roles is empty";

        public const string BCKND_RELATION_NOTFOUND = "Relation not exist";
        public const string BCKND_RELATION_CONFLICT = "Relation already exist";

        public const string BCKND_TOKEN_NOTFOUND = "Token not found";
        public const string BCKND_TOKEN_EXPIRED_UNAUTHORIZED = "Token expired";

        public const string BCKND_PASSWORD_NOT_MATCH_NOTFOUND = "Password dont match to this account";
    }
}
