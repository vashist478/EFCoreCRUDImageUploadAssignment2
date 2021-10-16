using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreImageUploadAssignment2.Models
{
    public class Employee
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpId { get; set; }


        [Column(TypeName = "varchar(80)")]

        [Required(ErrorMessage ="Please enter your name")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Please enter your address")]
        [Column(TypeName = "varchar(250)")]
        public string Address { get; set; }

        [ForeignKey("Department")]
        [Required(ErrorMessage ="Please select department")]
        public int DeptId { get; set; }

        [ForeignKey("DeptId")]
        public Department Department { get; set; }

        [ForeignKey("ImageId")]
        public int? ImageId { get; set; }


        [NotMapped]
        [ForeignKey("ImageId")]
        public Image Image { get; set; }
    }
}
