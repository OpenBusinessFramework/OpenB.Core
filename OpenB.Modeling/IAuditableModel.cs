using OpenB.Core;

namespace OpenB.Modeling
{
    public interface IAuditableModel : IModel
    {
        AuditRegistration Created { get;  }
        AuditRegistration Modified { get;  }
        AuditRegistration LastAccessed { get;  }
    }
}