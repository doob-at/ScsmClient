using System;

namespace ScsmClient.SharedModels.Models
{
    public interface IScsmClass
    {
        Guid Id { get; }
        string Name { get;  }
        string DisplayName { get;  }
        IScsmClassProperty[] Properties { get; }
    }
}