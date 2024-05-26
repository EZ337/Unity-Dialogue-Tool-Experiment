using System;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
public class ConditionAttribute : Attribute
{
    Type type;
    public ConditionAttribute(Type type) => this.type = type;
}
