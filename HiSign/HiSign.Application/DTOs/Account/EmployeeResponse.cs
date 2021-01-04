using System.Collections.Generic;

namespace HiSign.Application.DTOs.Account
{
    public class EmployeeResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<string> Roles { get; set; }
        public IList<UserPermissionViewModel> Permissions { get; set; }

    }

    public class UserPermissionViewModel
    {
        public string UserId { get; set; }
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public bool Enabled { get; set; }
    }
}
