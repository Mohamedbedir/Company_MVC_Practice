using System;

namespace Company.PL.ViewModels
{
    public class RoleViewModel //id - name -timestamp-
    {
        public string Id { get; set; }
        public string RoleName { get; set; }



        public RoleViewModel()
        {
            Id=Guid.NewGuid().ToString();
        }
    }
}
