using System;

namespace ScsmClient.SharedModels.Models
{
    public interface IScsmClassProperty
    {
        Guid Id { get; }
        string Name { get; }
        string DisplayName { get;}
        bool AutoIncrement { get; }
        bool CaseSensitive { get; }
        bool Key { get; }
        bool Required { get; }
        string DefaultValue { get; }
        int MaxLength { get;  }
        int MinLength { get;  }
        int MaxValue { get; }
        int MinValue { get;  }
        string Type { get; }
    }
}