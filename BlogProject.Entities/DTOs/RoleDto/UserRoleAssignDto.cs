using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Entities.DTOs.RoleDto
{
    public class UserRoleAssignDto
    {
        /// <summary>
        /// Değerimiz  interface olduğu için initialize olmadığı için ctorda  tanımladık.
        /// </summary>
        public UserRoleAssignDto()
        {
            RoleAssignDtos = new List<RoleAssignDto>();
        }
        
        public int UserId { get; set; }
        public string UserName { get; set; }
        public IList<RoleAssignDto> RoleAssignDtos { get; set; }
    }
}
