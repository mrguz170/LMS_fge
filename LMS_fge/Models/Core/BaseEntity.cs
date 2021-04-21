using System;
using System.ComponentModel.DataAnnotations;

namespace LMS_fge.Models.Core
{
    // Base for all Entities of data
    public class BaseEntity : SharedData
    {
        public Guid Id { get; set; }
    }

    // Shared info for all elements
    public class SharedData
    {
        [Display(Name = "Fecha de registro")]
        public DateTime CreationDate { get; set; }
        public StatusCore Status { get; set; }
    }

    // Base for catalogs
    public class BaseCatalog : BaseEntity
    {
        [StringLength(150)]
        [Display(Name = "Nombre")]
        public virtual string Value { get; set; }
        public int Order { get; set; }
    }

    // Base for recursive catalogs
    public class BaseSubCatalog : BaseCatalog
    {
        public Guid ParentId { get; set; }
    }

    // For logic status of store data
    public enum StatusCore
    {
        [Display(Name = "Activo")]
        Active, 
        Inactive, 
        [Display(Name = "Eliminado")]
        Deleted
    }
}
